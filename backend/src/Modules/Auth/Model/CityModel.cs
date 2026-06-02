using Trippie.Modules.Auth.Dtos;
using Trippie.Common.Database;
using Trippie.Common.Services.Search.Model;
using Trippie.Common.Services.Search.Linq;
using Trippie.Common.Services.Search.Exception;

namespace Trippie.Modules.Auth.Model;

public sealed class City
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int CountryId { get; set; }
    public Country? Country { get; set; }

    public static CityDto ToDto(City c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        CountryId = c.CountryId
    };
}

public sealed class CityModel(AppDbContext db)
{
    public async Task<CursorPage<CityDto>> SearchCitiesAsync(SearchPayload payload, CancellationToken ct = default)
    {
        var res = await new SearchQueryBuilder<City>(db.Cities)
            .WithKey("id", x => x.Id)
            .AddFilters(payload.Filters, field => field.ToLowerInvariant() switch
            {
				"id" => x => x.Id,
                "name" => x => x.Name,
                "country_id" => x => x.CountryId,
                _ => throw new SearchValidationException($"Unknown filter field '{field}'."),
            })
            .SetOrderBy(payload.Sort, field => field.ToLowerInvariant() switch
            {
				"id" => x => x.Id,
                "name" => x => x.Name,
				"country_id" => x => x.CountryId,
                _ => throw new SearchValidationException($"Unsortable field '{field}'."),
            })
            .SetCursorPagination(payload.Page)
            .RunAsync(x => City.ToDto(x), ct);

        return res;
    }

    
}
