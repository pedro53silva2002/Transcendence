using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trippie.Common.Services.Authentication.Context;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Travel.Dtos;
using Trippie.Modules.Travel.Service;

namespace Trippie.Modules.Travel.Router;

[ApiController]
[Authorize]
[Route("api/trips")]
public sealed class TripRouter(TripService service, IUserContext userContext) : ControllerBase
{
	private static readonly JsonSerializerOptions SearchJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

	[HttpPost]
	public async Task<ActionResult<TripDto>> Create([FromBody] CreateTripDto dto, CancellationToken ct)
	{
		var trip = await service.CreateAsync(dto, ct);
		return CreatedAtAction(nameof(Create), trip);
	}

	[HttpGet("search")]
	public async Task<ActionResult<CursorPage<TripDto>>> Search([FromQuery] string q, CancellationToken ct)
	{
		var json = Encoding.UTF8.GetString(Convert.FromBase64String(q));
		var payload = JsonSerializer.Deserialize<SearchPayload>(json, SearchJsonOptions) ?? new SearchPayload();
		var page = await service.SearchAsync(payload, ct);
		return Ok(page);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<TripDto>> Get(int id, CancellationToken ct)
	{
		var trip = await service.GetById(id, ct);
		if (trip is null) return NotFound();
		return Ok(trip);
	}

	[HttpPut("{id}")]
	public async Task<ActionResult<TripDto>> Update(int id, [FromBody]UpdateTripDto dto, CancellationToken ct)
	{
		var userId = userContext.Require().UserId;
		var trip = await service.UpdateAsync(userId, id, dto, ct);
		return Ok(trip);
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult<TripDto>> Delete(int id, CancellationToken ct)
	{
		var userId = userContext.Require().UserId;
		await service.DeleteAsync(userId, id, ct);
		return NoContent();
	}
}
