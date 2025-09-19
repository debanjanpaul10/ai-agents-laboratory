using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// The Agents API adapter handler interface.
/// </summary>
public interface IAgentsHandler
{
    /// <summary>
    /// Creates the new agent asynchronous.
    /// </summary>
    /// <param name="agentData">The agent data.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> CreateNewAgentAsync(CreateAgentDTO agentData, string userEmail);

    /// <summary>
    /// Gets all agents data asynchronous.
    /// </summary>
    /// <returns>The list of <see cref="AgentDataDTO"/></returns>
    Task<IEnumerable<AgentDataDTO>> GetAllAgentsDataAsync();

    /// <summary>
    /// Gets the agent data by identifier asynchronous.
    /// </summary>
    /// <param name="agentId">The agent identifier.</param>
    /// <returns>The agent data dto.</returns>
    Task<AgentDataDTO> GetAgentDataByIdAsync(string agentId);

    /// <summary>
    /// Updates the existing agent data.
    /// </summary>
    /// <param name="updateAgentData">The update agent data DTO model.</param>
    /// <returns>The updated agent data dto.</returns>
    Task<AgentDataDTO> UpdateExistingAgentDataAsync(AgentDataDTO updateAgentData);

    /// <summary>
    /// Deletes an existing agent data.
    /// </summary>
    /// <param name="agentId">The agent id.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> DeleteExistingAgentDataAsync(string agentId);
}
