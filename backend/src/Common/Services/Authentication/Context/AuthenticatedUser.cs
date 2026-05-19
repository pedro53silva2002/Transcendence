using Trippie.Common.Services.Authentication.Jwt;

namespace Trippie.Common.Services.Authentication.Context;

public sealed class AuthenticatedUser
{
    public required int UserId { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string DisplayName { get; init; }
    public required IReadOnlyList<JwtTripClaim> Trips { get; init; }
}
