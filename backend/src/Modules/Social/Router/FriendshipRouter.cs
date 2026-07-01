using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trippie.Common.Services.Authentication.Context;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Modules.Social.Dtos;
using Trippie.Modules.Social.Service;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Social.Model;

namespace Trippie.Modules.Social.Router;

[ApiController]
[Authorize]
[Route("api/friendships")]
public sealed class FriendshipRouter(FriendshipService service, IUserContext userContext) : ControllerBase
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
}
