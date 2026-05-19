using Trippie.Common.Services.Authentication.Extensions;
using Trippie.Common.Services.Authentication.Jwt;

namespace Trippie.Common.Services.Authentication.Context;

public sealed class UserContext(IHttpContextAccessor acessor) : IUserContext
{
    private readonly IHttpContextAccessor _acessor = acessor;
private AuthenticatedUser? _cached;

private AuthenticatedUser? Resolve()
{
    if (_cached is not null) return _cached;

    var principal = _acessor.HttpContext?.User;
    if (principal?.Identity?.IsAuthenticated != true) return null;

    var userId = principal.GetUserId();
    var email = principal.GetEmail();
    var username = principal.GetUsername();
    var displayName = principal.GetDisplayName();

    if (userId is null || email is null || username is null || displayName is null)
        return null;

    _cached = new AuthenticatedUser
    {
        UserId = userId.Value,
        Email = email,
        Username = username,
        DisplayName = displayName,
        Trips = principal.GetTrips()
    };

    return _cached;
}

public bool IsAuthenticated => Resolve() is not null;
public int? UserId => Resolve()?.UserId;
public string? Email => Resolve()?.Email;
public string? Username => Resolve()?.Username;
public string? DisplayName => Resolve()?.DisplayName;
public IReadOnlyList<JwtTripClaim> Trips => Resolve()?.Trips ?? [];

public AuthenticatedUser Require() => Resolve() ?? throw new InvalidOperationException(
    "No authenticated user is present on the current request.");
}
