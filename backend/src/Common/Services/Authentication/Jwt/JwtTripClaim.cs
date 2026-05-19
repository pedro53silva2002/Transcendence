using System.Text.Json.Serialization;

namespace Trippie.Common.Services.Authentication.Jwt;

public sealed class JwtTripClaim
{
    [JsonPropertyName("tripId")]
    public int TripId { get; init; }

    [JsonPropertyName("role")]
    public string Role { get; init; } = string.Empty;
}
