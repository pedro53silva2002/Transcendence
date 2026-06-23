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
	public async Task<FriendRequestDto> CreateAsync(CreateFriendRequestDto dto, CancellationToken ct = default)
	{
		var existingRequest = await db.FriendRequests
			.FirstOrDefaultAsync(fr =>
				(fr.SenderId == dto.SenderId && fr.ReceiverId == dto.ReceiverId) ||
				(fr.SenderId == dto.ReceiverId && fr.ReceiverId == dto.SenderId),
				ct);

		if (existingRequest != null)
			throw new SearchValidationException("A friend request already exists between these users.");

		var friendRequest = new FriendRequest
		{
			Id = 0,
			SenderId = dto.SenderId,
			ReceiverId = dto.ReceiverId,
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

	public async Task<FriendRequestDto?> GetBySenderIdAsync(int id, CancellationToken ct = default)
	{
		var friendRequest = await db.FriendRequests
			.FirstOrDefaultAsync(fr => fr.SenderId == id && fr.Status == FriendRequestStatus.Pending, ct);

		if (friendRequest is null)
			return null;

		return FriendRequest.ToDto(friendRequest);
	}

	public async Task<FriendRequestDto?> GetByIdAsync(int id, CancellationToken ct = default)
	{
		var friendRequest = await db.FriendRequests
			.FirstOrDefaultAsync(fr => fr.Id == id, ct);

		if (friendRequest is null)
			return null;

		return FriendRequest.ToDto(friendRequest);
	}

	public async Task<FriendRequestDto> AcceptAsync(int id, CancellationToken ct = default)
	{
		var friendRequest = await db.FriendRequests
			.FirstOrDefaultAsync(fr => fr.Id == id, ct);

		if (friendRequest is null)
			throw new NotFoundException("FriendRequest.Not.Found","Friend request not found.");

		if (friendRequest.Status != FriendRequestStatus.Pending)
			throw new SearchValidationException("Only pending friend requests can be accepted.");

		UpdateAsync(friendRequest, ct).Wait();

		friendRequest.Status = FriendRequestStatus.Accepted;
		friendRequest.UpdatedAt = DateTime.UtcNow;

		var friendshipdto = new FriendshipDto
		{
			Id = 0,
			UserId1 = friendRequest.SenderId,
			UserId2 = friendRequest.ReceiverId,
			CreatedAt = DateTime.UtcNow
		};

		await db.SaveChangesAsync(ct);

		return FriendRequest.ToDto(friendRequest);
	}

	public async Task<FriendRequestDto> UpdateAsync(FriendRequest friendRequest, CancellationToken ct = default)
	{//TODO
	}

	public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
	{
		var rows = await db.FriendRequests.Where(fr => fr.Id == id).ExecuteDeleteAsync(ct);
		return rows > 0;
	}
	
}
