using Microsoft.EntityFrameworkCore;
using Trippie.Common.Database;
using Trippie.Modules.Social.Config;
using Trippie.Modules.Social.Dtos;

namespace Trippie.Modules.Social.Model;

public sealed class Friendship()
{
	public required int Id { get; set; }
	public required int UserId { get; set; }
	public required int FriendId { get; set; }
	public DateTime CreatedAt { get; set; }

	public static FriendshipDto ToDto(Friendship f) => new()
	{
		Id = f.Id,
		UserId = f.UserId,
		FriendId = f.FriendId,
		CreatedAt = f.CreatedAt,
	};
}

public sealed class FriendshipModel(AppDbContext db)
{
	//Are all this verifications necessary?
	public async Task<FriendshipDto> CreateAsync(int senderId, int receiverId, CancellationToken ct = default)
	{
		var now = DateTime.UtcNow;

		var existingFriendship = await db.Friendships.AnyAsync(
		    f => (f.UserId == senderId && f.FriendId == receiverId) ||
		         (f.UserId == receiverId && f.FriendId == senderId),
		    ct);

		if (existingFriendship)
		    throw new InvalidOperationException("Friendship already exists between these users.");

		var request = await db.FriendRequests.FirstOrDefaultAsync(
			fr => (fr.SenderId == senderId && fr.ReceiverId == receiverId) ||
				  (fr.SenderId == receiverId && fr.ReceiverId == senderId), ct);

		if (request == null)
			throw new InvalidOperationException("No friend request exists between these users.");

		var f = new Friendship()
		{
			Id = 0,
			UserId = senderId,
			FriendId = receiverId,
			CreatedAt = now,
		};
		var f2 = new Friendship()
		{
			Id = 0,
			UserId = receiverId,
			FriendId = senderId,
			CreatedAt = now,
		};

		db.Friendships.AddRange(f, f2);

		await db.SaveChangesAsync(ct);

		// we only return one of the friendships, as they are essentially duplicates 
		// and this friendship derives from the receiver's prespective 
		// since they are the one accepting the request
		return Friendship.ToDto(f2);
	}

	public async Task<List<FriendDto>> GetAllAsync(int userId, CancellationToken ct = default)
	{
		return await (
    		from f in db.Friendships.AsNoTracking()
    		where f.UserId == userId
    		join u in db.Users.AsNoTracking() on f.FriendId equals u.Id
    		select new FriendDto
    		{
    		    Id = f.Id,
    		    FriendId = u.Id,
    		    Username = u.Username,
    		    ProfilePhotoUrl = u.ProfilePhotoUrl
    		})
    		.ToListAsync(ct);
	}

	public async Task<int> FriendshipCountAsync(int userId, CancellationToken ct = default)
	{
		return await db.Friendships
			.AsNoTracking()
			.CountAsync(f => f.UserId == userId, ct);
	}

	public async Task<FriendshipDto?> GetById(int id, int userId, CancellationToken ct = default)
	{
		var friendship = await db.Friendships
		.AsNoTracking()
		.FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId, ct);

		return friendship is null ? null : Friendship.ToDto(friendship);
	}

	public async Task<bool> DeleteAsync(int id, int userId, CancellationToken ct = default)
	{
		var friendship = await db.Friendships
			.AsNoTracking()
			.FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId, ct);

		if (friendship is null)
			return false;

		var deleted = await db.Friendships
        	.Where(f =>
        	    (f.UserId == userId && f.FriendId == friendship.FriendId) ||
        	    (f.UserId == friendship.FriendId && f.FriendId == userId))
        	.ExecuteDeleteAsync(ct);

    	return deleted == 2;
	}
}
