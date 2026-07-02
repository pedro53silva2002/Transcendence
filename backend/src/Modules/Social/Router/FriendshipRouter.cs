using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trippie.Common.Services.Authentication.Context;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Modules.Social.Dtos;
using Trippie.Modules.Social.Service;
using Trippie.Modules.Auth.Service;

namespace Trippie.Modules.Social.Router;

[ApiController]
[Authorize]
[Route("api/friendships")]
public sealed class FriendshipRouter(FriendshipService service, IUserContext userContext, UserService userService) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<List<FriendDto>>> GetAllAsync(CancellationToken ct = default)
	{
		var userId = userContext.Require().UserId;

		
		return Ok(await service.GetAllAsync(userId, ct));
	}

	[HttpGet("count")]
	public async Task<ActionResult<int>> FriendshipCountAsync(CancellationToken ct = default)
	{
		var userId = userContext.Require().UserId;

		var count = await service.FriendshipCountAsync(userId, ct);
		return Ok(count);
	}

	[HttpGet("{Id}")]
	public async Task<ActionResult<FriendshipDto?>> GetById(int id, CancellationToken ct = default)
	{
		var userId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated");
	
		var friendship = await service.GetById(id, userId, ct);

		if (friendship == null)
			return NotFound();

		return Ok(friendship);
	}

	[HttpDelete("{Id}")]
	public async Task<ActionResult> UnfriendAsync(int id, CancellationToken ct = default)
	{
		var userId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated");
	
		await service.UnfriendAsync(id, userId, ct);
		return NoContent();
	}

	[HttpGet("exists/{username}")]
	public async Task<ActionResult<bool>> FriendshipExistsAsync(string username, CancellationToken ct = default)
	{
		var myId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated");

		var otherUser = await userService.GetByUsername(username, ct);

		if (otherUser is null)
		    return NotFound();

		var exists = await service.FriendshipExistsAsync(myId, otherUser.Id, ct);
	
		return Ok(exists);
	}
}
