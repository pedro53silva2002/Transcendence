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
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(query));
        var payload = JsonSerializer.Deserialize<SearchPayload>(json, SearchJsonOptions) ?? new SearchPayload();
        var cities = await service.SearchCitiesAsync(payload, ct);
        return Ok(cities);
    }
}
