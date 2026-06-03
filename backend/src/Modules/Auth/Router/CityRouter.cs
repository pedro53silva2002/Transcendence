using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Service;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Trippie.Common.Services.Search.Model;

namespace Trippie.Modules.Auth.Router;

[ApiController]
[Authorize]
[Route("api/cities")]
public sealed class CityRouter(CityService service) : ControllerBase
{
    private static readonly JsonSerializerOptions SearchJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    [HttpGet("search")]
    public async Task<ActionResult<List<CityDto>>> SearchCities([FromQuery] string query, CancellationToken ct)
    {
        try
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(query));
            var payload = JsonSerializer.Deserialize<SearchPayload>(json, SearchJsonOptions) ?? new SearchPayload();

            if (payload is null)
                return BadRequest("Invalid payload");

            if (payload.Filters != null && 
                payload.Filters.Any(f => f.Column.ToLowerInvariant() == "name") && 
                !payload.Filters.Any(f => f.Column.ToLowerInvariant() == "country_id"))
            {
                return BadRequest("Filtering by city name requires a country filter.");
            }

            var cities = await service.SearchCitiesAsync(payload, ct);
            return Ok(cities);
        }
        catch (FormatException)
        {
            return BadRequest("Invalid Base64 query");
        }
        catch (JsonException)
        {
            return BadRequest("Invalid JSON payload");
        }
    }
}
