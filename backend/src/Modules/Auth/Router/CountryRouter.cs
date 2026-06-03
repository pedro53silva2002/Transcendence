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
[Route("api/countries")]
public sealed class CountryRouter(CountryService service) : ControllerBase
{
    private static readonly JsonSerializerOptions SearchJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    [HttpGet("search")]
    public async Task<ActionResult<List<CountryDto>>> SearchCountries([FromQuery] string query, CancellationToken ct)
    {
        try
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(query));
            var payload = JsonSerializer.Deserialize<SearchPayload>(json,SearchJsonOptions);

            if (payload is null)
                return BadRequest("Invalid payload");

            var countries = await service.SearchCountriesAsync(payload, ct);
            return Ok(countries);
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
