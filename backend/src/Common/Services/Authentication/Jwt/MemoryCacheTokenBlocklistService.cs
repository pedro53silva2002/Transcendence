using Microsoft.Extensions.Caching.Memory;

namespace Trippie.Common.Services.Authentication.Jwt;

public sealed class MemoryCacheTokenBlocklistService(IMemoryCache cache) : ITokenBlocklistService
{
    public void Revoke(string jti, DateTimeOffset expiry)
    {
        var ttl = expiry - DateTimeOffset.UtcNow;
        if (ttl > TimeSpan.Zero)
            cache.Set(CacheKey(jti), true, ttl);
    }

    public bool IsRevoked(string jti) => cache.TryGetValue(CacheKey(jti), out _);

    private static string CacheKey(string jti) => $"blocklist:{jti}";
}
