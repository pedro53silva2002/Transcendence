using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Trippie.Common.Services.Caching;

public sealed class CacheService(IDistributedCache cache, ILogger<CacheService> logger) : ICachingService
{
	private static readonly JsonSerializerOptions SerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = false,
	};

	public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
	{
		try
		{
			var cachedData = await cache.GetStringAsync(key, ct);
			if (string.IsNullOrEmpty(cachedData))
				return default;
			
			return JsonSerializer.Deserialize<T>(cachedData, SerializerOptions);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Redis error while getting cache for key: {Key}", key);
			return default;
		}
	}

	public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken ct = default)
	{
		try
		{
			var jsonData = JsonSerializer.Serialize(value, SerializerOptions);
			var options = new DistributedCacheEntryOptions
			{
				// Default to 1 hour if no expiration is provided
				AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(1)
			};

			await cache.SetStringAsync(key, jsonData, options, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Redis error while setting cache for key: {Key}", key);
		}
	}

	public async Task RemoveAsync(string key, CancellationToken ct = default)
	{
		try
		{
			await cache.RemoveAsync(key, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Redis error while removing cache for key: {Key}", key);
		}
	}

	public async Task<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? expiration = null, CancellationToken ct = default)
	{
		var cachedValue = await GetAsync<T>(key, ct);
		if (cachedValue is not null)
			return cachedValue;
		
		var freshData = await factory(ct);
		await SetAsync(key, freshData, expiration, ct);
		return freshData;
	}
}
