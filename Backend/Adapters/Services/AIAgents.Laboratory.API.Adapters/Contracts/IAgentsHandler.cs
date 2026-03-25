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
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> CreateNewAgentAsync(CreateAgentDTO agentData, string userEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all agents data asynchronous.
    /// </summary>
    /// <param name="userEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="AgentDataDTO"/></returns>
    Task<IEnumerable<AgentDataDTO>> GetAllAgentsDataAsync(string userEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the agent data by identifier asynchronous.
    /// </summary>
    /// <param name="agentId">The agent identifier.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The agent data dto.</returns>
    Task<AgentDataDTO> GetAgentDataByIdAsync(string agentId, string userEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the existing agent data.
    /// </summary>
    /// <param name="updateAgentData">The update agent data DTO model.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> UpdateExistingAgentDataAsync(AgentDataDTO updateAgentData, string currentUserEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing agent data.
    /// </summary>
    /// <param name="agentId">The agent id.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> DeleteExistingAgentDataAsync(string agentId, string currentUserEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads the knowledgebase file asynchronous.
    /// </summary>
    /// <param name="agentGuid">The agent guid id.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The downloaded file url</returns>
    Task<string> DownloadKnowledgebaseFileAsync(string agentGuid, string fileName, CancellationToken cancellationToken = default);
}
