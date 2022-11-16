namespace Redis.Cache;

public interface IRedisService
{
    Task SetAsync(string key, string value);
    Task<string?> GetAsync(string key);
}