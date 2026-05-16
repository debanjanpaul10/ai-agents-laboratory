using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Persistence.Caching.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.Caching.Services;

/// <summary>
/// Cache service class.
/// </summary>
/// <param name="memoryCache">The memory cache service.</param>
/// <param name="logger">The logger.</param>
/// <param name="correlationContext">The correlation context for logging.</param>
/// <param name="configuration">The configuration.</param>
/// <seealso cref="ICacheService"/>
public sealed class CacheService(IMemoryCache memoryCache, ILogger<CacheService> logger, IConfiguration configuration, ICorrelationContext correlationContext) : ICacheService
{
    /// <summary>
    /// The is cache service enabled.
    /// </summary>
    private readonly bool IsCacheServiceEnabled = bool.TryParse(configuration[AzureAppConfigurationConstants.IsCacheServiceEnabled], out var flagValue) && flagValue;

    /// <inheritdoc />
    public T? GetCachedData<T>(
        string key
    )
    {
        if (IsCacheServiceEnabled)
            return default;

        if (string.IsNullOrEmpty(key))
        {
            var ex = new ArgumentNullException(key);
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(GetCachedData), DateTime.UtcNow, ex.Message
            );
            throw ex;
        }
        try
        {
            logger.LogAppInformation(LoggingConstants.MethodStartedMessageConstant, nameof(GetCachedData), DateTime.UtcNow, key);
            if (memoryCache.TryGetValue(key, out T? value))
            {
                logger.LogAppInformation(LoggingConstants.CacheKeyFoundMessageConstant, key);
                return value;
            }

            logger.LogAppInformation(
                LoggingConstants.CacheKeyNotFoundMessageConstant,
                key
            );
            return value;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(GetCachedData), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                ex.Message,
                correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(GetCachedData), DateTime.UtcNow, key
            );
        }
    }

    /// <inheritdoc />
    public bool RemoveCachedData(
        string key
    )
    {
        if (IsCacheServiceEnabled)
            return default;

        if (string.IsNullOrEmpty(key))
        {
            var ex = new ArgumentNullException(nameof(key), ExceptionConstants.KeyNameIsNullMessageConstant);
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(RemoveCachedData), DateTime.UtcNow, ex.Message
            );
            throw ex;
        }

        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(RemoveCachedData), DateTime.UtcNow, key
            );
            if (memoryCache.TryGetValue(key, out _)) // Check if key exists
            {
                memoryCache.Remove(key);
                logger.LogAppInformation("Successfully removed cache for key: {CacheKey}", key);
                return true;
            }

            logger.LogAppInformation(
                LoggingConstants.CacheKeyNotFoundMessageConstant,
                key
            );
            return false;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(RemoveCachedData), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(RemoveCachedData), DateTime.UtcNow, key
            );
        }
    }

    /// <inheritdoc />
    public bool SetCacheData<T>(
        string key,
        T value,
        TimeSpan expirationTime
    )
    {
        if (!this.IsCacheServiceEnabled)
            return false;

        if (string.IsNullOrEmpty(key))
        {
            var ex = new ArgumentNullException(nameof(key), ExceptionConstants.KeyNameIsNullMessageConstant);
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(SetCacheData), DateTime.UtcNow, ex.Message
            );
            throw ex;
        }

        try
        {
            logger.LogAppInformation(LoggingConstants.MethodStartedMessageConstant, nameof(SetCacheData), DateTime.UtcNow, key);
            memoryCache.Set(
                key,
                value,
                absoluteExpirationRelativeToNow: expirationTime
            );
            return true;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(SetCacheData), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(SetCacheData), DateTime.UtcNow, key
            );
        }
    }
}
