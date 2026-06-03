using Trippie.Modules.Auth.Dtos;
using Trippie.Common.Database;
using Trippie.Common.Services.Search.Model;
using Trippie.Common.Services.Search.Linq;
using Trippie.Common.Services.Search.Exception;

namespace Trippie.Modules.Auth.Model;

public sealed class Country
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }

    public static CountryDto ToDto(Country c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Code = c.Code
    };
}

public sealed class CountryModel(AppDbContext db)
{
    public async Task<CursorPage<CountryDto>> SearchCountriesAsync(SearchPayload payload, CancellationToken ct = default)
    {
        var res = await new SearchQueryBuilder<Country>(db.Countries)
            .WithKey("id", x => x.Id)
            .AddFilters(payload.Filters, field => field.ToLowerInvariant() switch
            {
                "name" => x => x.Name,
                "code" => x => x.Code,
                _ => throw new SearchValidationException($"Unknown filter field '{field}'."),
            })
            .SetOrderBy(payload.Sort, field => field.ToLowerInvariant() switch
            {
                "name" => x => x.Name,
                _ => throw new SearchValidationException($"Unsortable field '{field}'."),
            })
            .SetCursorPagination(payload.Page)
            .RunAsync(x => Country.ToDto(x), ct);

        return res;
    }

    
}
