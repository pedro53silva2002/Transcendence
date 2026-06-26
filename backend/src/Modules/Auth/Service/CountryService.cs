using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Auth.Service;

public sealed class CountryService(CountryModel countryModel)
{
    public async Task<CursorPage<CountryDto>> SearchCountriesAsync(SearchPayload payload, CancellationToken ct = default)        
        => await countryModel.SearchCountriesAsync(payload, ct);
}
