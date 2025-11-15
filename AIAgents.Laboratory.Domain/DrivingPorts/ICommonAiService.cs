using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.DrivingPorts;

/// <summary>
/// Common AI Service interface.
/// </summary>
public interface ICommonAiService
{
    /// <summary>
    /// Gets the current model identifier.
    /// </summary>
    /// <returns>The current model identifier.</returns>
    string GetCurrentModelId();

    /// <summary>
    /// Gets the agent current status.
    /// </summary>
    /// <returns>The agent status data.</returns>
    AgentStatus GetAgentCurrentStatus();

    /// <summary>
    /// Gets the configurations data for application.
    /// </summary>
    /// <param name="userName">The current logged in user.</param>
    /// <returns>The dictionary containing the key-value pair.</returns>
    Dictionary<string, string> GetConfigurationsData(string userName);

    /// <summary>
    /// Retrieves a collection of configuration settings associated with the specified key name.
    /// </summary>
    /// <param name="key">The key name used to identify the configuration group. Cannot be null or empty.</param>
    /// <returns>A dictionary containing configuration key-value pairs for the specified key name.</returns>
    Dictionary<string, string> GetConfigurationByKeyName(string key);
}