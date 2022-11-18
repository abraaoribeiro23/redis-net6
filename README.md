# redis-net6

Simple example about how to connect in Redis, set and get a value with .net 6

```c#
public interface IRedisService
{
    Task SetAsync(string key, string value);
    Task<string?> GetAsync(string key);
}
```
