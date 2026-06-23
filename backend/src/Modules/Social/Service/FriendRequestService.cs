using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Social.Dtos;
using Trippie.Modules.Social.Model;
using Trippie.Modules.Social.Config;

namespace Trippie.Modules.Social.Service;

public sealed class FriendRequestService(FriendRequestModel friendRequestModel)
{

	public async Task<FriendRequestDto> CreateAsync(int callerId, CreateFriendRequestDto dto, CancellationToken ct = default)
	{
		if (callerId != dto.SenderId)
			throw new UnauthorizedException("Caller ID does not match sender ID.");
		if (dto.SenderId == dto.ReceiverId)
			throw new ValidationException("SenderID = ReceiverID", "Sender ID and Receiver ID cannot be the same.");
		if (dto.SenderId <= 0 || dto.ReceiverId <= 0)
			throw new ValidationException("SenderID, ReceiverID", "Sender ID and Receiver ID must be greater than zero.");

		var friendRequestDto = await friendRequestModel.CreateAsync(dto, ct);

		return friendRequestDto;
	}

	public async Task<FriendRequestDto> AcceptAsync(int callerId, int id, CancellationToken ct = default)
	{
		var friendRequest = await friendRequestModel.GetByReceiverIdAsync(id, ct);
		if (friendRequest == null || friendRequest.Id != id)
			throw new NotFoundException($"Friend request {id} not found for receiver {callerId}.", id);

		if (friendRequest.Status != FriendRequestStatus.Pending)
			throw new ValidationException("Status", "Only pending friend requests can be accepted.");

		friendRequest.Status = FriendRequestStatus.Accepted;
		await friendRequestModel.UpdateAsync(friendRequest, ct);

		return friendRequest;
	}
}
