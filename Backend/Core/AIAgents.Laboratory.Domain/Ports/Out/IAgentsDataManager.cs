using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

namespace AIAgents.Laboratory.Domain.Ports.Out;

/// <summary>
/// Provides definition for the data service operations related to agent data, including creating, retrieving, updating, and deleting agent data in the underlying data store.
/// </summary>
/// <remarks>This interface abstracts the data access layer for agent data, allowing for different implementations (e.g., MongoDB, SQL Server) without affecting the domain logic.</remarks>
public interface IAgentsDataManager
{
    /// <summary>
    /// Creates the new agent asynchronous.
    /// </summary>
    /// <param name="agentData">The agent data.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> CreateNewAgentAsync(AgentDataDomain agentData, string userEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all agents data asynchronous.
    /// </summary>
    /// <param name="userEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="AgentDataDomain"/></returns>
    Task<IEnumerable<AgentDataDomain>> GetAllAgentsDataAsync(string userEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the agent data by identifier asynchronous.
    /// </summary>
    /// <param name="agentId">The agent identifier.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The agent data dto.</returns>
    Task<AgentDataDomain> GetAgentDataByIdAsync(string agentId, string userEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the existing agent data.
    /// </summary>
    /// <param name="updateDataDomain">The update agent data domain model.</param>
    /// <param name="userEmail">The current logged in user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> UpdateExistingAgentDataAsync(AgentDataDomain updateDataDomain, string userEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing agent data.
    /// </summary>
    /// <param name="agentId">The agent id.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> DeleteExistingAgentDataAsync(string agentId, string currentUserEmail, CancellationToken cancellationToken = default);
}
