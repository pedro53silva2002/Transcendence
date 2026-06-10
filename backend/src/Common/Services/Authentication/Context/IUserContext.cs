using Trippie.Common.Services.Authentication.Jwt;

namespace Trippie.Common.Services.Authentication.Context;

public interface IUserContext
{
    bool IsAuthenticated { get; }

    int? UserId { get; }
    string? Email { get; }
    string? Username { get; }
    string? DisplayName { get; }
    IReadOnlyList<JwtTripClaim> Trips { get; }

    AuthenticatedUser Require();
}
