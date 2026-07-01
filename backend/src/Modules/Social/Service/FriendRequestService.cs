using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Trippie.Common.Database;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Modules.Social.Dtos;
using Trippie.Modules.Social.Model;

namespace Trippie.Modules.Social.Service;

public sealed class FriendRequestService(
	FriendRequestModel friendRequestModel,
	FriendshipModel friendshipModel,
	AppDbContext db)
{
	public async Task<CreateFriendRequestResult> CreateAsync(int callerId, CreateFriendRequestDto dto, CancellationToken ct = default)
	{
		if (callerId == dto.ReceiverId)
			throw new ValidationException("Self.Request", "Sender ID and Receiver ID cannot be the same.");


		if (dto.ReceiverId <= 0)
			throw new ValidationException("ReceiverID", "Receiver ID must be greater than zero.");

		var reverseRequest = await friendRequestModel.GetPendingFromAsync(dto.ReceiverId, callerId, ct);

		// If a reverse request exists, then both users intent to be friends
		// So, we can automatically accept the reverse request and create a friendship
		if (reverseRequest is not null)
		{
			var friendship = await AcceptCoreAsync(callerId, reverseRequest.Id, ct);
			return new CreateFriendRequestResult { Friendship = friendship };
		}

		try
		{
			var request = await friendRequestModel.CreateAsync(callerId, dto.ReceiverId, ct);
			return new CreateFriendRequestResult { FriendRequest = request };
		}
		catch (DbUpdateException ex) when (IsUniqueViolation(ex))
		{
			throw new ValidationException("FriendRequest.Duplicate", "A friend request was already sent to this user.");
		}
	}

	public async Task<List<FriendRequestDto>> GetSentAsync(int callerId, CancellationToken ct = default)
		=> await friendRequestModel.GetSentAsync(callerId, ct);

	public async Task<List<FriendRequestDto>> GetReceivedAsync(int callerId, CancellationToken ct = default)
		=> await friendRequestModel.GetReceivedAsync(callerId, ct);

	public async Task<FriendRequestDto?> GetByIdAsync(int callerId, int requestId, CancellationToken ct = default)
	{
		var request = await friendRequestModel.GetByIdAsync(requestId, ct);

		if (request is null)
			return null;

		if (request.SenderId != callerId && request.ReceiverId != callerId)
			throw new ValidationException("FriendRequest.Forbidden",
			"You are not a participant in this friend request.");

		return request;
	}

	public Task<FriendshipDto> AcceptAsync(
		int callerId, int requestId, CancellationToken ct = default)
		=> AcceptCoreAsync(callerId, requestId, ct);

	public async Task CancelOrRejectAsync(
		int callerId, int requestId, CancellationToken ct = default)
	{
		var request = await friendRequestModel.GetByIdAsync(requestId, ct);

		if (request is null)
			throw new NotFoundException("FriendRequest.NotFound", $"Friend request {requestId} not found.");
		
		if (request.SenderId != callerId && request.ReceiverId != callerId)
			throw new ValidationException("FriendRequest.Forbidden", "You are not a participant in this friend request.");
		
		await friendRequestModel.DeleteAsync(requestId, ct);
	}

	private async Task<FriendshipDto> AcceptCoreAsync(
		int callerId, int requestId, CancellationToken ct = default)
	{
		await using var transaction = await db.Database.BeginTransactionAsync(ct);

		try
		{
			var request = await friendRequestModel.GetByIdAsync(requestId, ct);

			if (request is null)
				throw new NotFoundException("FriendRequest.NotFound", 
					$"Friend request {requestId} not found or was already accepted/rejected.");
			
			if (request.ReceiverId != callerId)
				throw new ValidationException("FriendRequest.Forbidden",
					"Only the receiver of a friend request can accept it.");
			
			var friendship = await friendshipModel.CreateAsync(request.SenderId, request.ReceiverId, ct);

			await friendRequestModel.DeleteAsync(requestId, ct);
			await transaction.CommitAsync(ct);
			return friendship;
		}
		catch (DbUpdateException ex) when (IsUniqueViolation(ex))
		{
			await transaction.RollbackAsync(ct);

			throw new ValidationException("Friendship.AlreadyExists",
				"This friend request has already been accepted.");
		}
		catch
		{
			await transaction.RollbackAsync(ct);
			throw;
		}
	}

	private static bool IsUniqueViolation(DbUpdateException ex) =>
		ex.InnerException is PostgresException pg && pg.SqlState == "23505";
}
