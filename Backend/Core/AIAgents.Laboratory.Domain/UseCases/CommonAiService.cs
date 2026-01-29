using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Common AI Service.
/// </summary>
/// <param name="configuration">The configuration.</param>
/// <param name="agentStatusStore">THe agent status store.</param>
/// <param name="logger">The logger service.</param>
/// <param name="cacheService">The cache service.</param>
/// <seealso cref="ICommonAiService"/>
public sealed class CommonAiService(IConfiguration configuration, ILogger<CommonAiService> logger, IAgentStatusStore agentStatusStore, ICacheService cacheService, IAgentsService agentsService) : ICommonAiService
{
    /// <summary>
    /// Gets the current model identifier.
    /// </summary>
    /// <returns>The current model identifier.</returns>
    public string GetCurrentModelId()
    {
        var isProModelEnabled = bool.TryParse(configuration[AzureAppConfigurationConstants.IsProModelEnabledFlag], out bool parsedValue) && parsedValue;
        var geminiAiModel = isProModelEnabled ? AzureAppConfigurationConstants.GeminiProModel : AzureAppConfigurationConstants.GeminiFlashModel;
        return configuration[geminiAiModel] ?? ExceptionConstants.ModelNameNotFoundExceptionConstant;
    }

    /// <summary>
    /// Gets the agent current status.
    /// </summary> 
    /// <returns>
    /// The agent status data.
    /// </returns>
    public AgentStatus GetAgentCurrentStatus()
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAgentCurrentStatus), DateTime.UtcNow, string.Empty);
            return agentStatusStore.Current;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAgentCurrentStatus), DateTime.UtcNow, ex.Message);
            return new() { IsAvailable = false };
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAgentCurrentStatus), DateTime.UtcNow, string.Empty);
        }
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
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetConfigurationsData), DateTime.UtcNow, userName);

            var cachedValues = cacheService.GetCachedData<Dictionary<string, string>>(CacheKeys.AllAppSettingsKeyName);
            if (cachedValues is not null && cachedValues.Count > 0)
            {
                return cachedValues;
            }
            else
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

                cacheService.SetCacheData(CacheKeys.AllAppSettingsKeyName, existingCacheData, CacheKeys.CacheExpirationTimeout);
                return existingCacheData;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetConfigurationsData), DateTime.UtcNow, ex.Message);
            return [];
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetConfigurationsData), DateTime.UtcNow, userName);
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
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetConfigurationByKeyName), DateTime.UtcNow, key);

            var cachedkeyValue = cacheService.GetCachedData<Dictionary<string, string>>(key);
            if (cachedkeyValue is not null && cachedkeyValue.Count > 0)
            {
                return cachedkeyValue;
            }
            else
            {
                cacheService.SetCacheData(key, new Dictionary<string, string> { { key, configuration[key]! } }, CacheKeys.CacheExpirationTimeout);
                return new Dictionary<string, string> { { key, configuration[key]! } };
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetConfigurationByKeyName), DateTime.UtcNow, ex.Message);
            return [];
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetConfigurationByKeyName), DateTime.UtcNow, key);
        }
    }

    /// <summary>
    /// Gets the top active agents data list and the agents count asynchronously.
    /// </summary>
    /// <param name="userName">The current logged in user.</param>
    /// <returns>A tupple containing the list of agents and the top 3 active ai agents.</returns>
    public async Task<(int ActiveAgentsCount, IEnumerable<AgentDataDomain> TopActiveAgentsList)> GetTopActiveAgentsDataAsync(string userName)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetTopActiveAgentsDataAsync), DateTime.UtcNow, userName);

            var activeAgentsList = await agentsService.GetAllAgentsDataAsync(userName).ConfigureAwait(false);
            return (activeAgentsList.Count(), [.. activeAgentsList.OrderByDescending(x => x.DateModified).Take(3)]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetTopActiveAgentsDataAsync), DateTime.UtcNow, ex.Message);
            return (0, []);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetTopActiveAgentsDataAsync), DateTime.UtcNow, userName);
        }
    }
}