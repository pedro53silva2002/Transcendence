using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trippie.Common.Services.Authentication.Context;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Social.Dtos;
using Trippie.Modules.Social.Model;
using Trippie.Modules.Social.Service;

namespace Trippie.Modules.Social.Router;

[ApiController]
[Authorize]
[Route("api/friendships")]
public sealed class FriendshipRouter(FriendshipService service, IUserContext userContext) : ControllerBase
{
	[HttpPost]
	public async Task<ActionResult<FriendshipDto>> Create([FromBody] CreateFriendshipDto dto, CancellationToken ct = default)
	{
		var userId = userContext.Require().UserId;
		var friendship = await service.CreateFriendshipAsync(userId, dto, ct);
		return CreatedAtAction(nameof(Create), new { id = friendship.Id }, friendship);
	}

	[HttpGet]
	public async Task<ActionResult<List<FriendDto>>> GetAllAsync(CancellationToken ct = default)
	{
		var userId = userContext.Require().UserId;
		var friends = await service.GetAllAsync(userId, ct);
		return Ok(friends);
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
		var userId = userContext.Require().UserId;
		var friendship = await service.GetById(id, ct);
		if (friendship == null)
			return NotFound();
		return Ok(friendship);
	}

	[HttpDelete("{Id}")]
	public async Task<ActionResult> Delete(int id, CancellationToken ct = default)
	{
		var userId = userContext.Require().UserId;
		await service.DeleteFriendshipAsync(id, userId, ct);
		return NoContent();
	}
}
