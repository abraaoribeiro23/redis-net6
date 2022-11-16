using Microsoft.Extensions.Caching.Distributed;

namespace Redis.Cache;

public class RedisService : IRedisService
{
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _options;

    public RedisService(IDistributedCache cache)
    {
        _cache = cache;
        _options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(1)
        };
    }

    public async Task SetAsync(string key, string value)
    {
        await _cache.SetStringAsync(key, value, _options);
    }

    public async Task<string?> GetAsync(string key)
    {
        return await _cache.GetStringAsync(key);
    }
}