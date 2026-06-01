using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Trippie.Common.Services.Authentication.Context;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Travel.Dtos;
using Trippie.Modules.Travel.Service;

namespace Trippie.Modules.Travel.Router;

[ApiController]
[Route("api/trips/itinerary")]
public sealed class ItineraryRouter(ItineraryService service, IUserContext userContext) : ControllerBase
{
    private static readonly JsonSerializerOptions SearchJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    [HttpPost]
    public async Task<ActionResult<ItineraryDto>> Create([FromBody] CreateItineraryDto dto, CancellationToken ct)
    {
        var userId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");
        var itinerary = await service.CreateAsync(userId, dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = itinerary.Id }, itinerary);
    }

    [HttpGet("search")]
    public async Task<ActionResult<CursorPage<ItineraryDto>>> Search([FromQuery] string q, CancellationToken ct)
    {
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(q));
        var payload = JsonSerializer.Deserialize<SearchPayload>(json, SearchJsonOptions) ?? new SearchPayload();
        var page = await service.SearchAsync(payload, ct);
        return Ok(page);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItineraryDto>> GetById(int id, CancellationToken ct)
    {
        var itinerary = await service.GetById(id, ct);
        return itinerary is null ? NotFound() : Ok(itinerary);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ItineraryDto>> Update(int userid, int id, [FromBody] UpdateItineraryDto dto, CancellationToken ct)
    {
        var userId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");

        var itinerary = await service.UpdateAsync(id, dto, ct);
        return itinerary is null ? NotFound() : Ok(itinerary);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var userId = userContext.UserId ?? throw new UnauthorizedException("User not authenticated.");
        await service.DeleteAsync(userId, id, ct);
        return NoContent();
    }
}
