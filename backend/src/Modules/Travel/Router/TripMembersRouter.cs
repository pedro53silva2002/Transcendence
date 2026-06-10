using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trippie.Common.Services.Authentication.Context;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Travel.Dtos;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Modules.Travel.Service;

namespace Trippie.Modules.Travel.Router;

[ApiController]
[Authorize]
[Route("api/trips/{tripId}/members")]
public sealed class TripMembersRouter(TripMembersService tripMembersService, IUserContext userContext) : ControllerBase
{
	private static readonly JsonSerializerOptions SearchJsonOptions = new()
	{
		PropertyNameCaseInsensitive = true,
		Converters = { new JsonStringEnumConverter()}
	};

	[HttpPost]
	public async Task<ActionResult<TripMembersDto>> CreateAsync([FromBody] CreateTripMembersDto dto, CancellationToken ct = default)
	{
		var callerId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");

		var createdMembers = await tripMembersService.CreateAsync(callerId, dto, ct);
		return CreatedAtAction(nameof(CreateAsync), new { Id = createdMembers.Select(m => m.Id) }, createdMembers);
	}

	[HttpGet("search")]
	public async Task<ActionResult<CursorPage<TripMembersDto>>> SearchAsync([FromQuery] string q, CancellationToken ct = default)
	{
		var json = Encoding.UTF8.GetString(Convert.FromBase64String(q));
		var payload = JsonSerializer.Deserialize<SearchPayload>(json, SearchJsonOptions) ?? new SearchPayload();
		var page = await tripMembersService.SearchAsync(payload, ct);
		return Ok(page);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<TripMembersDto>> GetByIdAsync(int id, CancellationToken ct = default)
	{
		var member = await tripMembersService.GetById(id, ct);
		return member == null ? NotFound() : Ok(member);
	}

	[HttpPut("{id}")]
	public async Task<ActionResult<TripMembersDto>> UpdateAsync(int id, [FromBody] UpdateTripMembersDto dto, CancellationToken ct = default)
	{
		var AdminId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");
		var updatedMember = await tripMembersService.UpdateAsync(AdminId, id, dto, ct);
		return updatedMember == null ? NotFound() : Ok(updatedMember);
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteAsync(int id, CancellationToken ct = default)
	{
		var AdminId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");
		await tripMembersService.DeleteAsync(AdminId, id, ct);
		return NoContent();
	}
}
