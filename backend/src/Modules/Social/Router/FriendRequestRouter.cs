using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trippie.Common.Services.Authentication.Context;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Modules.Social.Dtos;
using System.Text.Json;
using System.Text.Json.Serialization;
using Trippie.Modules.Auth.Service;

namespace Trippie.Modules.Social.Service;

[ApiController]
[Authorize]
[Route("api/friend-requests")]
public sealed class FriendRequestRouter(
	FriendRequestService friendRequestService,
	IUserContext userContext, UserService userService) : ControllerBase
{
	private static readonly JsonSerializerOptions SearchJsonOptions = new()
	{
		PropertyNameCaseInsensitive = true,
		Converters = { new JsonStringEnumConverter()}
	};

	[HttpPost]
	public async Task<IActionResult> CreateAsync([FromBody] CreateFriendRequestDto dto, CancellationToken ct = default)
	{
		var callerId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");

		var friendRequest = await friendRequestService.CreateAsync(callerId, dto, ct);

		//mutual request
		if (friendRequest.WasAutoAccepted)
			return Ok(friendRequest.Friendship);

		//pending request created
		return Created($"/api/friend-requests/{friendRequest.FriendRequest!.Id}", friendRequest.FriendRequest);
	}

	[HttpGet("sent")]
	public async Task<ActionResult<List<FriendRequestDto>>> GetSentAsync(CancellationToken ct = default)
	{
		var callerId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");

		return Ok(await friendRequestService.GetSentAsync(callerId, ct));
	}

	[HttpGet("received")]
	public async Task<ActionResult<List<FriendRequestDto>>> GetReceivedAsync(CancellationToken ct = default)
	{
		var callerId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");

		return Ok(await friendRequestService.GetReceivedAsync(callerId, ct));
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<FriendRequestDto>> GetByIdAsync(int id, CancellationToken ct = default)
	{
		var callerId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");

		return Ok(await friendRequestService.GetByIdAsync(callerId, id, ct));
	}

	[HttpGet("exists/{username}")]
	public async Task<ActionResult<FriendRequestExistsDto?>> FriendRequestExistsAsync(string username, CancellationToken ct = default)
	{
		var myId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated");

		var otherUser = await userService.GetByUsername(username, ct);

		if (otherUser is null)
		    return NotFound();

		var exists = await friendRequestService.FriendRequestExistsAsync(myId, otherUser.Id, ct);

		return Ok(exists);
	}

	[HttpPost("{otherId}/accept")]
	public async Task<ActionResult<FriendshipDto>> AcceptAsync(int otherId, CancellationToken ct = default)
	{
		var myId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");

		var friendship = await friendRequestService.AcceptAsync(myId, otherId, ct);

		return Ok(friendship);
	}

	[HttpDelete("{otherId}")]
	public async Task<ActionResult> CancelOrRejectAsync(int otherId, CancellationToken ct = default)
	{
		var myId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");

		await friendRequestService.CancelOrRejectAsync(myId, otherId, ct);

		return NoContent();
	}
}
