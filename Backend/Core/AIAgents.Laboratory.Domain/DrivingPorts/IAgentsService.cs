using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

namespace AIAgents.Laboratory.Domain.DrivingPorts;

/// <summary>
/// The Agents Business Service Interface.
/// </summary>
public interface IAgentsService
{
    /// <summary>
    /// Creates the new agent asynchronous.
    /// </summary>
    /// <param name="agentData">The agent data.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> CreateNewAgentAsync(AgentDataDomain agentData, string userEmail);

    /// <summary>
    /// Gets all agents data asynchronous.
    /// </summary>
    /// <param name="userEmail">The current logged in user email.</param>
    /// <returns>The list of <see cref="AgentDataDomain"/></returns>
    Task<IEnumerable<AgentDataDomain>> GetAllAgentsDataAsync(string userEmail);

    /// <summary>
    /// Gets the agent data by identifier asynchronous.
    /// </summary>
    /// <param name="agentId">The agent identifier.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <returns>The agent data dto.</returns>
    Task<AgentDataDomain> GetAgentDataByIdAsync(string agentId, string userEmail);

    /// <summary>
    /// Updates the existing agent data.
    /// </summary>
    /// <param name="updateDataDomain">The update agent data domain model.</param>
    /// <param name="userEmail">The current logged in user email address.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> UpdateExistingAgentDataAsync(AgentDataDomain updateDataDomain, string userEmail);

    /// <summary>
    /// Deletes an existing agent data.
    /// </summary>
    /// <param name="agentId">The agent id.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> DeleteExistingAgentDataAsync(string agentId, string currentUserEmail);
}
