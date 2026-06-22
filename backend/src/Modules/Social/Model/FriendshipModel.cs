using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Modules.Social.Config;
using Trippie.Modules.Social.Dtos;

namespace Trippie.Modules.Social.Model;

public sealed class Friendship()
{
	public required int Id { get; set; }
	public required int UserId1 { get; set; }
	public required int UserId2 { get; set; }
	public DateTime CreatedAt { get; set; }

	public static FriendshipDto ToDto(Friendship friendship) => new()
	{
		Id = friendship.Id,
		UserId1 = friendship.UserId1,
		UserId2 = friendship.UserId2,
		CreatedAt = friendship.CreatedAt,
	};
}

public sealed class FriendshipModel(AppDbContext db)
{
	public async Task<FriendshipDto> CreateAsync(CreateFriendshipDto dto, CancellationToken ct = default)
	{
		var existingFriendship = await db.Friendships.FirstOrDefaultAsync(
			f => (f.UserId1 == dto.UserId1 && f.UserId2 == dto.UserId2) ||
				 (f.UserId1 == dto.UserId2 && f.UserId2 == dto.UserId1), ct);

		if (existingFriendship != null)
			throw new InvalidOperationException("Friendship already exists between these users.");

		var request = await db.FriendRequests.FirstOrDefaultAsync(
			fr => (fr.SenderId == dto.UserId1 && fr.ReceiverId == dto.UserId2) ||
				  (fr.SenderId == dto.UserId2 && fr.ReceiverId == dto.UserId1), ct);

		if (request == null)
			throw new InvalidOperationException("No friend request exists between these users.");
		if (request.Status != FriendRequestStatus.Pending)
			throw new InvalidOperationException("Friend request is not pending.");

		var friendship = new Friendship()
		{
			Id = 0,
			UserId1 = dto.UserId1,
			UserId2 = dto.UserId2,
			CreatedAt = DateTime.UtcNow,
		};

		db.Friendships.Add(friendship);
		await db.SaveChangesAsync(ct);

		return Friendship.ToDto(friendship);
	}

	public async Task<List<FriendDto>> GetAllAsync(int myId, CancellationToken ct = default)
	{
		return await (
    		from f in db.Friendships
    		let friendId = f.UserId1 == myId ? f.UserId2 : f.UserId1
    		join u in db.Users on friendId equals u.Id
    		where f.UserId1 == myId || f.UserId2 == myId
    		select new FriendDto
    		{
    		    FriendId = u.Id,
    		    Username = u.Username,
    		    ProfilePhotoUrl = u.ProfilePhotoUrl
    		})
    		.ToListAsync(ct);
	}

	public async Task<int> FriendshipCountAsync(int myId, CancellationToken ct = default)
	{
		return await db.Friendships.CountAsync(
			f => f.UserId1 == myId || f.UserId2 == myId, ct);
	}

	public async Task<FriendshipDto?> GetById(int id, CancellationToken ct = default)
	{
		var friendship = await db.Friendships.FirstOrDefaultAsync(f => f.Id == id, ct);
		return friendship == null ? null : Friendship.ToDto(friendship);
	}

	public async Task<bool> DeleteAsync(int id, int userId, CancellationToken ct = default)
	{
		var friendship = await db.Friendships.FirstOrDefaultAsync(f => f.Id == id, ct);
		if (friendship is null)
			return false;
		var deleted = await db.Friendships.Where(f => f.Id == id && (f.UserId1 == userId || f.UserId2 == userId)).ExecuteDeleteAsync(ct);
		if (deleted == 0)
			throw new InvalidOperationException("Friendship not found or user is not a participant.");
		
		return deleted > 0;
	}
}