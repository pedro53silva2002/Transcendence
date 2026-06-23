using Trippie.Modules.Social.Model;
using Trippie.Modules.Social.Dtos;


namespace Trippie.Modules.Social.Service;

public sealed class FriendshipService(FriendshipModel friendshipModel)
{
	public async Task<FriendshipDto> CreateFriendshipAsync(int myId, CreateFriendshipDto dto, CancellationToken ct = default)
	{
		if (dto.FriendId == myId)
			throw new ArgumentException("Cannot create friendship with the same user.");
		if (myId <= 0 || dto.FriendId <= 0)
			throw new ArgumentException("User IDs must be greater than zero.");
		
		var friendship = await friendshipModel.CreateAsync(myId, dto, ct);
		return friendship;
	}

	public async Task<List<FriendDto>> GetAllAsync(int myId, CancellationToken ct = default)
	{
		if (myId <= 0)
			throw new ArgumentException("User ID must be greater than zero.");

		return await friendshipModel.GetAllAsync(myId, ct);
	}

	public async Task<int> FriendshipCountAsync(int myId, CancellationToken ct = default)
	{
		if (myId <= 0)
			throw new ArgumentException("User ID must be greater than zero.");

		return await friendshipModel.FriendshipCountAsync(myId, ct);
	}

	public async Task<FriendshipDto?> GetById(int id, CancellationToken ct = default)
	{
		if (id <= 0)
			throw new ArgumentException("Friendship ID must be greater than zero.");

		return await friendshipModel.GetById(id, ct);
	}

	public async Task DeleteAsync(int id, int userId, CancellationToken ct = default)
	{
		if (id <= 0)
			throw new ArgumentException("Friendship ID must be greater than zero.");

		await friendshipModel.DeleteAsync(id, userId, ct);
	}
}
