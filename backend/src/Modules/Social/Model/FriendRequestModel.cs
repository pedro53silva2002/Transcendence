using Microsoft.EntityFrameworkCore;
using Trippie.Modules.Social.Config;
using Trippie.Modules.Social.Dtos;
using Trippie.Common.Database;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Exception;
using Trippie.Common.Services.Search.Linq;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Social.Model;

public sealed class FriendRequest
{
	public required int Id { get; set; }
	public required int SenderId { get; set; }
	public required int ReceiverId { get; set; }
	public required FriendRequestStatus Status { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }

	public static FriendRequestDto ToDto(FriendRequest fr) => new()
	{
		Id = fr.Id,
		SenderId = fr.SenderId,
		ReceiverId = fr.ReceiverId,
		Status = fr.Status,
		CreatedAt = fr.CreatedAt,
		UpdatedAt = fr.UpdatedAt,
	};
}

public sealed class FriendRequestModel(AppDbContext db)
{
	public async Task<FriendRequestDto> CreateAsync(int senderId, int receiverId, CancellationToken ct = default)
	{
		if (senderId <= 0 || receiverId <= 0)
			throw new SearchValidationException("Sender ID and Receiver ID must be greater than zero.");

		if (senderId == receiverId)
			throw new SearchValidationException("Sender ID and Receiver ID cannot be the same.");

		var existingRequest = await db.FriendRequests
			.FirstOrDefaultAsync(fr =>
				(fr.SenderId == senderId && fr.ReceiverId == receiverId) ||
				(fr.SenderId == receiverId && fr.ReceiverId == senderId),
				ct);

		if (existingRequest != null)
			throw new SearchValidationException("A friend request already exists between these users.");

		var friendRequest = new FriendRequest
		{
			Id = 0,
			SenderId = senderId,
			ReceiverId = receiverId,
			Status = FriendRequestStatus.Pending,
		};

		db.FriendRequests.Add(friendRequest);
		await db.SaveChangesAsync(ct);

		return FriendRequest.ToDto(friendRequest);
	}

	public async Task<FriendRequestDto?> GetByReceiverIdAsync(int id, CancellationToken ct = default)
	{
		var friendRequest = await db.FriendRequests
			.FirstOrDefaultAsync(fr => fr.ReceiverId == id && fr.Status == FriendRequestStatus.Pending, ct);

		if (friendRequest is null)
			return null;

		return FriendRequest.ToDto(friendRequest);
	}
}
