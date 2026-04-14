using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides common AI-related services for retrieving configuration data, model identifiers, and agent information within the application.
/// </summary>
/// <remarks>This class centralizes access to AI configuration and agent data, supporting caching and logging for
/// operational efficiency. It is intended to be used as a singleton within the application's service layer.</remarks>
/// <param name="configuration">The application configuration provider used to access settings and model identifiers.</param>
/// <param name="logger">The logger instance used for logging operational and error information.</param>
/// <param name="correlationContext">The correlation context used to track and propagate request correlation information.</param>
/// <param name="cacheService">The cache service used for storing and retrieving configuration data to improve performance.</param>
/// <param name="agentsService">The agents service used to access agent-related data and operations.</param>
/// <seealso cref="ICommonAiService"/>
public sealed class CommonAiService(
    IConfiguration configuration,
    ILogger<CommonAiService> logger,
    ICorrelationContext correlationContext,
    ICacheService cacheService,
    IAgentsService agentsService) : ICommonAiService
{
    /// <summary>
    /// Gets the current model identifier.
    /// </summary>
    /// <returns>The current model identifier.</returns>
    public string GetCurrentModelId()
    {
        var isProModelEnabled = bool.TryParse(configuration[AzureAppConfigurationConstants.IsProModelEnabledFlag], out bool parsedValue) && parsedValue;
        var geminiAiModel = isProModelEnabled ? AzureAppConfigurationConstants.GeminiProModel : AzureAppConfigurationConstants.GeminiFlashModel;
        return configuration[geminiAiModel] ?? throw new KeyNotFoundException(ExceptionConstants.ModelNameNotFoundExceptionConstant);
    }

    /// <summary>
    /// Gets the configurations data for application.
    /// </summary>
    /// <param name="userName">The current logged in user.</param>
    /// <returns>The dictionary containing the key-value pair.</returns>
    public Dictionary<string, string> GetConfigurationsData(string userName)
    {
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetConfigurationsData), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName })
            );

            var cachedValues = cacheService.GetCachedData<Dictionary<string, string>>(CacheKeys.AllAppSettingsKeyName);
            if (cachedValues is null || cachedValues.Count == 0)
            {
                var existingCacheData = new Dictionary<string, string>
                {
                    { AzureAppConfigurationConstants.IsKnowledgeBaseServiceEnabledConstant, configuration[AzureAppConfigurationConstants.IsKnowledgeBaseServiceEnabledConstant]! },
                    { AzureAppConfigurationConstants.CurrentAiServiceProvider, configuration[AzureAppConfigurationConstants.CurrentAiServiceProvider]! },
                    { AzureAppConfigurationConstants.IsFeedbackFeatureEnabled, configuration[AzureAppConfigurationConstants.IsFeedbackFeatureEnabled]! },
                    { AzureAppConfigurationConstants.IsEmailNotificationEnabled, configuration[AzureAppConfigurationConstants.IsEmailNotificationEnabled]! },
                    { AzureAppConfigurationConstants.IsCacheServiceEnabled, configuration[AzureAppConfigurationConstants.IsCacheServiceEnabled]! },
                    { AzureAppConfigurationConstants.HowToFileLinkConstant, configuration[AzureAppConfigurationConstants.HowToFileLinkConstant]! },
                    { AzureAppConfigurationConstants.IsAiVisionServiceEnabledConstant, configuration[AzureAppConfigurationConstants.IsAiVisionServiceEnabledConstant]! }
                };

                cacheService.SetCacheData(
                    CacheKeys.AllAppSettingsKeyName,
                    existingCacheData,
                    CacheKeys.CacheExpirationTimeout
                );
                return existingCacheData;
            }

            return cachedValues;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(GetConfigurationsData), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(GetConfigurationsData), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName })
            );
        }
    }

    /// <summary>
    /// Retrieves a collection of configuration settings associated with the specified key name.
    /// </summary>
    /// <param name="key">The key name used to identify the configuration group. Cannot be null or empty.</param>
    /// <returns>A dictionary containing configuration key-value pairs for the specified key name.</returns>
    public Dictionary<string, string> GetConfigurationByKeyName(string key)
    {
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetConfigurationByKeyName), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, key })
            );

            var cachedkeyValue = cacheService.GetCachedData<Dictionary<string, string>>(key);
            if (cachedkeyValue is not null && cachedkeyValue.Count > 0)
            {
                return cachedkeyValue;
            }
            else
            {
                cacheService.SetCacheData(
                    key,
                    value: new Dictionary<string, string> { { key, configuration[key]! } },
                    expirationTime: CacheKeys.CacheExpirationTimeout
                );
                return new() { { key, configuration[key]! } };
            }
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(GetConfigurationByKeyName), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(GetConfigurationByKeyName), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, key })
            );
        }
    }

    /// <summary>
    /// Gets the top active agents data list and the agents count asynchronously.
    /// </summary>
    /// <param name="userName">The current logged in user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tupple containing the list of agents and the top 3 active ai agents.</returns>
    public async Task<(int ActiveAgentsCount, IEnumerable<AgentDataDomain> TopActiveAgentsList)> GetTopActiveAgentsDataAsync(
        string userName,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetTopActiveAgentsDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName })
            );

            var activeAgentsList = await agentsService.GetAllAgentsDataAsync(
                userEmail: userName,
                cancellationToken
            ).ConfigureAwait(false);

            return (
                ActiveAgentsCount: activeAgentsList.Count(),
                TopActiveAgentsList: [.. activeAgentsList.OrderByDescending(x => x.DateModified).Take(3)]);
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(GetTopActiveAgentsDataAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(GetTopActiveAgentsDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName })
            );
        }
    }
}