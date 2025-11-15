using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The Common AI Handler.
/// </summary>
/// <param name="commonAiService">The common ai service.</param>
/// <param name="mapper">The mapper.</param>
/// <seealso cref="AIAgents.Laboratory.API.Adapters.Contracts.ICommonAiHandler" />
public class CommonAiHandler(ICommonAiService commonAiService, IMapper mapper) : ICommonAiHandler
{
    /// <summary>
    /// Gets the agent current status.
    /// </summary>
    /// <returns>
    /// The agent status data.
    /// </returns>
    public AgentStatusDTO GetAgentCurrentStatus()
    {
        var domainStatusResponse = commonAiService.GetAgentCurrentStatus();
        return mapper.Map<AgentStatusDTO>(domainStatusResponse);
    }

    /// <summary>
    /// Retrieves a collection of configuration settings associated with the specified key name.
    /// </summary>
    /// <param name="key">The key name used to identify the configuration group. Cannot be null or empty.</param>
    /// <returns>A dictionary containing configuration key-value pairs for the specified key name.</returns>
    public Dictionary<string, string> GetConfigurationByKeyName(string key)
    {
        return commonAiService.GetConfigurationByKeyName(key);
    }

    /// <summary>
    /// Gets the configurations data for application.
    /// </summary>
    /// <param name="userName">The current logged in user.</param>
    /// <returns>The dictionary containing the key-value pair.</returns>
    public Dictionary<string, string> GetConfigurationsData(string userName)
    {
        return commonAiService.GetConfigurationsData(userName);
    }
}
