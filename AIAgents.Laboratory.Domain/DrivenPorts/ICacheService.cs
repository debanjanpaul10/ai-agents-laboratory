namespace AIAgents.Laboratory.Domain.DrivenPorts;

/// <summary>
/// Cache service interface.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Gets async.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <typeparam name="T"></typeparam>
    T? GetCachedData<T>(string key);

    /// <summary>
    /// Sets async.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="expirationTime">The expiration time.</param>
    /// <typeparam name="T"></typeparam>
    bool SetCacheData<T>(string key, T value, TimeSpan expirationTime);

    /// <summary>
    /// Removes data.
    /// </summary>
    /// <param name="key">The key.</param>
    bool RemoveCachedData(string key);
}
