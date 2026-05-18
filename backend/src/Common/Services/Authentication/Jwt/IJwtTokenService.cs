using System.Security.Claims;

namespace Trippie.Common.Services.Authentication.Jwt;

public interface IJwtTokenService
{
    (string Token, DateTimeOffset ExpiresAtUtc) GenerateToken(JwtUserClaims claims);
    ClaimsPrincipal ValidateToken(string token);
    JwtUserClaims? ExtractClaims(ClaimsPrincipal principal);
}
