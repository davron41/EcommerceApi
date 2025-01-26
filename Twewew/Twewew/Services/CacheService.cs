using Newtonsoft.Json;
using StackExchange.Redis;
using Twewew.Services.Interfaces;


namespace Twewew.Services;

public class CacheService : ICacheService, IDisposable
{
    private readonly IDatabase _db;
    private readonly IConnectionMultiplexer _redisConnection;

    public CacheService(IConnectionMultiplexer redisConnection)
    {
        _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
        _db = _redisConnection.GetDatabase();
    }

    public T GetData<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        try
        {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error fetching data from cache: {ex.Message}");
        }

        return default;
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        try
        {
            var expiryTime = expirationTime - DateTimeOffset.Now;
            return _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
        }
        catch (Exception ex)
        {

            Console.Error.WriteLine($"Error setting data to cache: {ex.Message}");
        }

        return false;
    }

    public bool RemoveData(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or empty.", nameof(key));

        try
        {
            return _db.KeyDelete(key);
        }
        catch (Exception ex)
        {
            // Log the exception (use an appropriate logging mechanism)
            Console.Error.WriteLine($"Error removing data from cache: {ex.Message}");
        }

        return false;
    }

    public void Dispose()
    {
        if (_redisConnection != null)
        {
            _redisConnection.Dispose();
        }
    }
}