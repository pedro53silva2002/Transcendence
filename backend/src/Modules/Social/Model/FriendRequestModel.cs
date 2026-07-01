using Microsoft.EntityFrameworkCore;
using Npgsql;
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
	public required FriendRequestStatus Status { get; set; } = FriendRequestStatus.Pending;
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }

	public static FriendRequestDto ToDto(FriendRequest fr, string senderUsername = "", string? senderProfilePhotoUrl = null) => new()
	{
		Id = fr.Id,
		SenderId = fr.SenderId,
		SenderUsername = senderUsername,
		SenderProfilePhotoUrl = senderProfilePhotoUrl,
		ReceiverId = fr.ReceiverId,
		Status = fr.Status,
		CreatedAt = fr.CreatedAt,
		UpdatedAt = fr.UpdatedAt,
	};
}

public sealed class FriendRequestModel(AppDbContext db)
{
	public async Task<FriendRequestDto> CreateAsync(
		int senderId, int receiverId,CancellationToken ct = default)
	{
		var sameDuplicate = await db.FriendRequests
			.AsNoTracking()
			.AnyAsync(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId, ct);

		if (sameDuplicate)
			throw new ValidationException("FriendRequest.Duplicate", "A friend request already exists between these users.");

		var existingFriendship = await db.Friendships
			.AsNoTracking()
			.AnyAsync(f =>
				(f.UserId == senderId && f.FriendId == receiverId) ||
			    (f.UserId == receiverId && f.FriendId == senderId), ct);

		if (existingFriendship)
			throw new ValidationException("Friendship.Exists", "A friendship already exists between these users.");

		var request = new FriendRequest
		{
			Id = 0,
			SenderId = senderId,
			ReceiverId = receiverId,
			Status = FriendRequestStatus.Pending,
			CreatedAt = DateTime.UtcNow,
		};

		db.FriendRequests.Add(request);

		try
		{
			await db.SaveChangesAsync(ct);
		}
		catch (DbUpdateException ex) when (IsUniqueViolation(ex))
		{
			throw new ValidationException("FriendRequest.Duplicate", "A friend request already exists between these users.");
		}

		return FriendRequest.ToDto(request);
	}

	public async Task<FriendRequest?> GetPendingFromAsync(
		int senderId, int receiverId, CancellationToken ct = default)
	{
		return await db.FriendRequests
			.AsNoTracking()
			.FirstOrDefaultAsync(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId, ct);
	}

	public async Task<List<FriendRequestDto>> GetReceivedAsync(int id, CancellationToken ct = default)
	{
		return await (
			from fr in db.FriendRequests.AsNoTracking()
			where fr.ReceiverId == id
			join u in db.Users.AsNoTracking() on fr.SenderId equals u.Id
			select new FriendRequestDto
			{
				Id = fr.Id,
				SenderId = fr.SenderId,
				SenderUsername = u.Username,
				SenderProfilePhotoUrl = u.ProfilePhotoUrl,
				ReceiverId = fr.ReceiverId,
				Status = fr.Status,
				CreatedAt = fr.CreatedAt,
				UpdatedAt = fr.UpdatedAt,
			})
			.ToListAsync(ct);
	}

	public async Task<List<FriendRequestDto>> GetSentAsync(int senderId, CancellationToken ct = default)
	{
		return await db.FriendRequests
			.AsNoTracking()
			.Where(fr => fr.SenderId == senderId)
			.Select(fr => new FriendRequestDto
			{
				Id = fr.Id,
				SenderId = fr.SenderId,
				SenderUsername = "",
				SenderProfilePhotoUrl = null,
				ReceiverId = fr.ReceiverId,
				Status = fr.Status,
				CreatedAt = fr.CreatedAt,
				UpdatedAt = fr.UpdatedAt,
			})
			.ToListAsync(ct);
	}

	public async Task<FriendRequestDto?> GetByIdAsync(int id, CancellationToken ct = default)
	{
		var friendRequest = await db.FriendRequests
			.FirstOrDefaultAsync(fr => fr.Id == id, ct);

		if (friendRequest is null)
			return null;

		return FriendRequest.ToDto(friendRequest);
	}

	public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
	{
		var rows = await db.FriendRequests
			.Where(fr => fr.Id == id).ExecuteDeleteAsync(ct);

		return rows > 0;
	}

	//to deal with race conditions where two friend requests are sent at the same time, we check for unique constraint violation
	private static bool IsUniqueViolation(DbUpdateException ex)
		=> ex.InnerException is PostgresException pg && pg.SqlState == "23505";
}
