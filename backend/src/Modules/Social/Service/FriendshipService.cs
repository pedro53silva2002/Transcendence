using Trippie.Modules.Social.Model;
using Trippie.Modules.Social.Dtos;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;

namespace Trippie.Modules.Social.Service;

public sealed class FriendshipService(FriendshipModel friendshipModel)
{
	public Task<List<FriendDto>> GetAllAsync(int userId, CancellationToken ct = default)
	{
		if (userId <= 0)
			throw new ArgumentException("User ID must be greater than zero.");

		return friendshipModel.GetAllAsync(userId, ct);
	}

	public Task<int> FriendshipCountAsync(int userId, CancellationToken ct = default)
	{
		if (userId <= 0)
			throw new ArgumentException("User ID must be greater than zero.");

		return friendshipModel.FriendshipCountAsync(userId, ct);
	}

	public async Task<FriendshipDto?> GetById(int id, int userId, CancellationToken ct = default)
	{
		if (id <= 0)
			throw new ArgumentException("Friendship ID must be greater than zero.");

		if (userId <= 0)
			throw new ArgumentException("User ID must be greater than zero.");

		var friendship = await friendshipModel.GetById(id, userId, ct);

		return friendship is null ? null : friendship;
	}

	public async Task UnfriendAsync(int id, int userId, CancellationToken ct = default)
	{
		if (id <= 0)
			throw new ArgumentException("Friendship ID must be greater than zero.");
		
		if (userId <= 0)
			throw new ArgumentException("User ID must be greater than zero.");

		var deleted = await friendshipModel.DeleteAsync(id, userId, ct);

		if (!deleted)
			throw new NotFoundException("Friendship.NotFound",
				$"Friendship with ID {id} not found for user with ID {userId}.");
	}

	public async Task<bool> FriendshipExistsAsync(int myId, int otherId, CancellationToken ct = default)
	{
		if (myId <= 0 || otherId <= 0)
			throw new ValidationException("InvalidUserId", "User IDs must be greater than zero.");
		if (myId == otherId)
			throw new ValidationException("Self.Friendship", "User IDs cannot be the same.");
		if (await friendshipModel.FriendshipExistsAsync(myId, otherId, ct))
			return true;
		return false;
	}
}
