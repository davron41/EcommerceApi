namespace Twewew.Services.Interfaces;

public interface ICacheService
{
    /// <summary>
    /// Retrieves data from the cache by the specified key.
    /// </summary>
    /// <typeparam name="T">The type of data to retrieve.</typeparam>
    /// <param name="key">The key associated with the cached data.</param>
    /// <returns>The cached data, or the default value of T if the key does not exist.</returns>
    T GetData<T>(string key);

    /// <summary>
    /// Stores data in the cache with the specified key and expiration time.
    /// </summary>
    /// <typeparam name="T">The type of data to store.</typeparam>
    /// <param name="key">The key to associate with the data.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="expirationTime">The expiration time for the cached data.</param>
    /// <returns>True if the data was successfully stored; otherwise, false.</returns>
    bool SetData<T>(string key, T value, DateTimeOffset expirationTime);

    /// <summary>
    /// Removes data from the cache by the specified key.
    /// </summary>
    /// <param name="key">The key of the data to remove.</param>
    /// <returns>True if the key was successfully removed; otherwise, false.</returns>
    bool RemoveData(string key);
}
