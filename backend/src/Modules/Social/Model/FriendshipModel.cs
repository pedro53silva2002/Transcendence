using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Modules.Social.Config;
using Trippie.Modules.Social.Dtos;
using Trippie.Modules.Social.Model;

namespace Trippie.Modules.Social.Model;

public sealed class Friendship()
{
	public required int Id { get; set; }
	public required int UserId { get; set; }
	public required int FriendId { get; set; }
	public DateTime CreatedAt { get; set; }

	public static FriendshipDto ToDto(Friendship friendship) => new()
	{
		Id = friendship.Id,
		FriendId = friendship.FriendId,
		CreatedAt = friendship.CreatedAt,
	};
}

public sealed class FriendshipModel(AppDbContext db)
{
	public async Task<FriendshipDto> CreateAsync(int myId, CreateFriendshipDto dto, CancellationToken ct = default)
	{
		var existingFriendship = await db.Friendships.AnyAsync(
		    f => (f.UserId == myId && f.FriendId == dto.FriendId) ||
		         (f.UserId == dto.FriendId && f.FriendId == myId),
		    ct);

		if (existingFriendship)
		    throw new InvalidOperationException("Friendship already exists between these users.");

		var request = await db.FriendRequests.FirstOrDefaultAsync(
			fr => (fr.SenderId == myId && fr.ReceiverId == dto.FriendId) ||
				  (fr.SenderId == dto.FriendId && fr.ReceiverId == myId), ct);

		if (request == null)
			throw new InvalidOperationException("No friend request exists between these users.");
		if (request.Status != FriendRequestStatus.Pending)
			throw new InvalidOperationException("Friend request is not pending.");

		var friendship = new Friendship()
		{
			Id = 0,
			UserId = myId,
			FriendId = dto.FriendId,
			CreatedAt = DateTime.UtcNow,
		};
		var friendship2 = new Friendship()
		{
			Id = 0,
			UserId = dto.FriendId,
			FriendId = myId,
			CreatedAt = DateTime.UtcNow,
		};

		db.Friendships.Add(friendship);
		db.Friendships.Add(friendship2);

		if (await db.SaveChangesAsync(ct) == 0)
			throw new InvalidOperationException("Failed to create friendship.");

		if (request.Status == FriendRequestStatus.Accepted)
			await new FriendRequestModel(db).DeleteAsync(request.Id, ct);

		await db.SaveChangesAsync(ct);

		return Friendship.ToDto(friendship);
	}

	public async Task<List<FriendDto>> GetAllAsync(int myId, CancellationToken ct = default)
	{
		return await (
    		from f in db.Friendships
    		where f.UserId == myId
    		join u in db.Users on f.FriendId equals u.Id
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
			f => f.UserId == myId, ct);
	}

	public async Task<FriendshipDto?> GetById(int id, CancellationToken ct = default)
	{
		var friendship = await db.Friendships.FirstOrDefaultAsync(f => f.Id == id, ct);
		return friendship == null ? null : Friendship.ToDto(friendship);
	}

	public async Task<bool> DeleteAsync(int myId, int friendId, CancellationToken ct = default)
	{
		var deleted = await db.Friendships
        	.Where(f =>
        	    (f.UserId == myId && f.FriendId == friendId) ||
        	    (f.UserId == friendId && f.FriendId == myId))
        	.ExecuteDeleteAsync(ct);

    	return deleted == 2;
	}
}
