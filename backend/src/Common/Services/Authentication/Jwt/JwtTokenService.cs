using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Trippie.Common.Services.Authentication.Jwt;

public sealed class JwtTokenService : IJwtTokenService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IMemoryCache _cache;
    private readonly JwtOptions _options;
    private readonly SigningCredentials _signingCredentials;
    private readonly TokenValidationParameters _validationParameters;
    private readonly JwtSecurityTokenHandler _handler = new() { MapInboundClaims = false };

    public JwtTokenService(IOptions<JwtOptions> options, IMemoryCache cache)
    {
        _cache = cache;
        _options = options.Value;
        var keyBytes = Encoding.UTF8.GetBytes(_options.SecretKey);
        if (keyBytes.Length < 32)
            throw new InvalidOperationException("Jwt:SecretKey must be at least 32 bytes (256 bits) for HS256.");

        var key = new SymmetricSecurityKey(keyBytes);
        _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        _validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _options.Issuer,
            ValidateAudience = true,
            ValidAudience = _options.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ClockSkew = TimeSpan.FromSeconds(_options.ClockSkewSeconds),
            NameClaimType = JwtClaimTypes.Username,
            RoleClaimType = ClaimTypes.Role
        };
    }

    public (string Token, DateTimeOffset ExpiresAtUtc) GenerateToken(JwtUserClaims claims)
    {
        ArgumentNullException.ThrowIfNull(claims);

        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.AddMinutes(_options.AccessTokenLifetimeMinutes);
        var tripsJson = JsonSerializer.Serialize(claims.Trips, JsonOptions);

        var claimList = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, claims.UserId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new(JwtRegisteredClaimNames.Email, claims.Email),
            new(JwtClaimTypes.Username, claims.Username),
            new(JwtClaimTypes.DisplayName, claims.DisplayName),
            new(JwtClaimTypes.Trips, tripsJson, JsonClaimValueTypes.JsonArray)
        };

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claimList,
            notBefore: now.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: _signingCredentials
        );

        return (_handler.WriteToken(token), expiresAt);
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new SecurityTokenException("Token is empty");

        var principal = _handler.ValidateToken(token, _validationParameters, out _);
        return principal;
    }

    public void InvalidateToken(string rawToken)
    {
        var parsed = _handler.ReadJwtToken(rawToken);
        var jti = parsed.Id;
        var expiry = parsed.ValidTo;
        var ttl = expiry - DateTime.UtcNow;
        if (!string.IsNullOrEmpty(jti) && ttl > TimeSpan.Zero)
            _cache.Set(jti, true, ttl);
    }

    public bool IsRevoked(string jti) => _cache.TryGetValue(jti, out _);

    public JwtUserClaims? ExtractClaims(ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        var sub = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        var email = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
        var username = principal.FindFirst(JwtClaimTypes.Username)?.Value;
        var displayName = principal.FindFirst(JwtClaimTypes.DisplayName)?.Value;
        var tripsRaw = principal.FindFirst(JwtClaimTypes.Trips)?.Value;

        if (sub is null || email is null || username is null || displayName is null || tripsRaw is null)
            return null;

        if (!int.TryParse(sub, out var userId))
            return null;

        IReadOnlyList<JwtTripClaim> trips;
        try
        {
            trips = JsonSerializer.Deserialize<List<JwtTripClaim>>(tripsRaw, JsonOptions)
                    ?? [];
        }
        catch (JsonException)
        {
            return null;
        }

        return new JwtUserClaims
        {
            UserId = userId,
            Email = email,
            Username = username,
            DisplayName = displayName,
            Trips = trips
        };

    }
}
