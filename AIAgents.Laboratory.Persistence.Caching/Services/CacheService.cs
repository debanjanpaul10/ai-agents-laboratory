using AIAgents.Laboratory.Domain.DrivenPorts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Persistence.Caching.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.Caching.Services;

/// <summary>
/// Cache service class.
/// </summary>
/// <param name="memoryCache">The memory cache service.</param>
/// <param name="logger">The logger.</param>
/// <seealso cref="ICacheService"/>
public class CacheService(IMemoryCache memoryCache, ILogger<CacheService> logger) : ICacheService
{
    /// <summary>
    /// Gets async.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <typeparam name="T"></typeparam>
    public T? GetCachedData<T>(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            var ex = new ArgumentNullException(key);
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(GetCachedData), DateTime.UtcNow, ex.Message);
            throw ex;
        }
        try
        {
            logger.LogInformation(LoggingConstants.MethodStartedMessageConstant, nameof(GetCachedData), DateTime.UtcNow, key);
            if (memoryCache.TryGetValue(key, out T? value))
            {
                logger.LogInformation(LoggingConstants.CacheKeyFoundMessageConstant, key);
                return value;
            }

            logger.LogInformation(LoggingConstants.CacheKeyNotFoundMessageConstant, key);
            return value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(GetCachedData), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.MethodEndedMessageConstant, nameof(GetCachedData), DateTime.UtcNow, key);
        }
    }

    /// <summary>
    /// Removes data.
    /// </summary>
    /// <param name="key">The key.</param>
    public bool RemoveCachedData(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            var ex = new ArgumentNullException(nameof(key), ExceptionConstants.KeyNameIsNullMessageConstant);
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(RemoveCachedData), DateTime.UtcNow, ex.Message);
            throw ex;
        }

        try
        {
            logger.LogInformation(LoggingConstants.MethodStartedMessageConstant, nameof(RemoveCachedData), DateTime.UtcNow, key);
            if (memoryCache.TryGetValue(key, out _)) // Check if key exists
            {
                memoryCache.Remove(key);
                logger.LogInformation("Successfully removed cache for key: {CacheKey}", key);
                return true;
            }

            logger.LogInformation(LoggingConstants.CacheKeyNotFoundMessageConstant, key);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(RemoveCachedData), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.MethodEndedMessageConstant, nameof(RemoveCachedData), DateTime.UtcNow, key);
        }
    }

    /// <summary>
    /// Sets async.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="expirationTime">The expiration time.</param>
    /// <typeparam name="T"></typeparam>
    public bool SetCacheData<T>(string key, T value, TimeSpan expirationTime)
    {
        if (string.IsNullOrEmpty(key))
        {
            var ex = new ArgumentNullException(nameof(key), ExceptionConstants.KeyNameIsNullMessageConstant);
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(SetCacheData), DateTime.UtcNow, ex.Message);
            throw ex;
        }

        try
        {
            logger.LogInformation(LoggingConstants.MethodStartedMessageConstant, nameof(SetCacheData), DateTime.UtcNow, key);
            memoryCache.Set(key, value, expirationTime);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(SetCacheData), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.MethodEndedMessageConstant, nameof(SetCacheData), DateTime.UtcNow, key);
        }
    }
}
