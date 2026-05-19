using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Trippie.Common.Services.Authentication.Jwt;

namespace Trippie.Common.Services.Authentication.Extensions;

public static class ClaimsPrincipalExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static bool IsAuthenticatedUser(this ClaimsPrincipal? principal) => principal?.Identity?.IsAuthenticated == true;
    public static int? GetUserId(this ClaimsPrincipal principal)
    {
        var raw = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        return int.TryParse(raw, out var id) ? id : null;
    }
    public static string? GetEmail(this ClaimsPrincipal principal) => principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

    public static string? GetUsername(this ClaimsPrincipal principal) => principal.FindFirst(JwtClaimTypes.Username)?.Value;

    public static string? GetDisplayName(this ClaimsPrincipal principal) => principal.FindFirst(JwtClaimTypes.DisplayName)?.Value;

    public static IReadOnlyList<JwtTripClaim> GetTrips(this ClaimsPrincipal principal)
    {
        var raw = principal.FindFirst(JwtClaimTypes.Trips)?.Value;
        if (string.IsNullOrEmpty(raw)) return [];

        try
        {
            return JsonSerializer.Deserialize<List<JwtTripClaim>>(raw, JsonOptions) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }

    public static bool HasTripRole(this ClaimsPrincipal principal, int tripId, string role)
    {
        foreach (var t in principal.GetTrips())
        {
            if (t.TripId == tripId && string.Equals(t.Role, role, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    public static bool IsMemberOfTrip(this ClaimsPrincipal principal, int tripId)
    {
        foreach (var t in principal.GetTrips())
            if (t.TripId == tripId) return true;
        return false;
    }

}