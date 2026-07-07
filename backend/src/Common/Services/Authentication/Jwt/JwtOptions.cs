using System.ComponentModel.DataAnnotations;

namespace Trippie.Common.Services.Authentication.Jwt;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required, MinLength(1)]
    public string Issuer { get; init; } = string.Empty;

    [Required, MinLength(1)]
    public string Audience { get; init; } = string.Empty;

    [Required, MinLength(32)]
    public string SecretKey { get; init; } = string.Empty;

    [Range(1, 1440)]
    public int AccessTokenLifetimeMinutes { get; init; } = 1;

    [Range(0, 300)]
    public int ClockSkewSeconds { get; init; } = 0;

    [Range(1, 365)]
    public int RefreshTokenLifetimeDays { get; set; } = 7;
}
