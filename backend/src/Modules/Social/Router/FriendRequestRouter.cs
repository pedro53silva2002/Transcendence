using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trippie.Modules.Social.Dtos;
using Trippie.Modules.Social.Service;
using Trippie.Common.Services.Authentication.Context;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Modules.Social.Model;

namespace Trippie.Modules.Social.Service;

[ApiController]
[Authorize]
[Route("api/friend-requests")]
public sealed class FriendRequestRouter(FriendRequestService friendRequestService, IUserContext userContext) : ControllerBase
{
	private static readonly JsonSerializerOptions SearchJsonOptions = new()
	{
		PropertyNameCaseInsensitive = true,
		Converters = { new JsonStringEnumConverter()}
	};

	[HttpPost]
	public async Task<ActionResult<FriendRequestDto>> CreateAsync(CreateFriendRequestDto dto, CancellationToken ct = default)
	{
		var callerId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");

		var friendRequest = await friendRequestService.CreateAsync(callerId, dto, ct);
		return Ok(friendRequest);
	}

	[HttpPost("{id}/accept")]
	public async Task<ActionResult<FriendshipDto>> AcceptAsync(int id, CancellationToken ct = default)
	{
		var callerId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");

		var friendRequest = await friendRequestService.AcceptAsync(callerId, id, ct);
		if (friendRequest == null)
			return NotFound();
		
		var friendshipDto = await new FriendshipService(new FriendshipModel(db)).CreateFriendshipAsync(callerId, friendRequest, ct);
		return Ok(friendshipDto);
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteAsync(int id, CancellationToken ct = default)
	{
		var callerId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");

		var success = await new FriendshipService(new FriendshipModel(db)).DeleteAsync(callerId, id, ct);
		if (!success)
			return NotFound();

		return NoContent();
	}
}
