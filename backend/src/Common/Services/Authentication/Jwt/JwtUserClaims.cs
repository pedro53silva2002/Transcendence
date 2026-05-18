namespace Trippie.Common.Services.Authentication.Jwt;

public sealed class JwtUserClaims
{
    public required int UserId { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string DisplayName { get; init; }
    public required IReadOnlyList<JwtTripClaim> Trips { get; init; }
}
