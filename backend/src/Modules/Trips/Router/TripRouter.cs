using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Trips.Dtos;
using Trippie.Modules.Trips.Service;

namespace Trippie.Modules.Trips.Router;

[ApiController]
[Route("api/trips")]
public sealed class TripRouter(TripService service) : ControllerBase
{
	private static readonly JsonSerializerOptions SearchJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

	[HttpPost]
	public async Task<ActionResult<TripDto>> Create([FromBody] CreatedTripDto dto, CancellationToken ct)
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

	[HttpPut("{id}")]
	public async Task<ActionResult<TripDto>> Update(int id, [FromBody]UpdateTripDto dto, CancellationToken ct)
	{
		var trip = await service.UpdateAsync(id, dto, ct);
		return Ok(trip);
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult<TripDto>> Delete(int id, CancellationToken ct)
	{
		await service.DeleteAsync(id, ct);
		return NoContent();
	}
}