
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FieldShield.Infrastructure.Caching;

public static class FieldShieldCacheExtensions
{

    public static async Task<T> GetOrCreateAsync<T>(
        this IDistributedCache cache,
        string key,
        Func<Task<T>> factory,
        DistributedCacheEntryOptions? options = null)
    {
        var cacheValue = await cache.GetStringAsync(key);

        if (cacheValue is not null)
        {
            return JsonSerializer.Deserialize<T>(cacheValue);
        }

        var data = await factory();

        await cache.SetStringAsync(
            key,
            JsonSerializer.Serialize(data),
            options ?? new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            });

        return data;
    }
}
