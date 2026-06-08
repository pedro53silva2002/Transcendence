using Trippie.Common.Services.Search.Model;
using Trippie.Modules.Auth.Dtos;
using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Auth.Service;

public sealed class CityService(CityModel cityModel)
{
    public async Task<CursorPage<CityDto>> SearchCitiesAsync(SearchPayload payload, CancellationToken ct = default)        
        => await cityModel.SearchCitiesAsync(payload, ct);
}
