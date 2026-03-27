using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.Ports.In;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The Agents API adapter handler.
/// </summary>
/// <param name="agentsService">The agents service.</param>
/// <param name="mapper">The mapper service.</param>
/// <seealso cref="AIAgents.Laboratory.API.Adapters.Contracts.IAgentsHandler" />
public sealed class AgentsHandler(IMapper mapper, IAgentsService agentsService) : IAgentsHandler
{
    /// <summary>
    /// Creates the new agent asynchronous.
    /// </summary>
    /// <param name="agentData">The agent data.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The boolean for success/failure.
    /// </returns>
    public async Task<bool> CreateNewAgentAsync(CreateAgentDTO agentData, string userEmail, CancellationToken cancellationToken = default)
    {
        var domainInput = mapper.Map<AgentDataDomain>(agentData);
        return await agentsService.CreateNewAgentAsync(
            agentData: domainInput,
            userEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the agent data by identifier asynchronous.
    /// </summary>
    /// <param name="agentId">The agent identifier.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The agent data dto.
    /// </returns>
    public async Task<AgentDataDTO> GetAgentDataByIdAsync(string agentId, string userEmail, CancellationToken cancellationToken = default)
    {
        var domainResult = await agentsService.GetAgentDataByIdAsync(
            agentId,
            userEmail,
            cancellationToken
        ).ConfigureAwait(false);
        return mapper.Map<AgentDataDTO>(domainResult);
    }

    /// <summary>
    /// Gets all agents data asynchronous.
    /// </summary>
    /// <param name="userEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="AgentDataDTO"/></returns>
    public async Task<IEnumerable<AgentDataDTO>> GetAllAgentsDataAsync(string userEmail, CancellationToken cancellationToken = default)
    {
        var domainResult = await agentsService.GetAllAgentsDataAsync(
            userEmail,
            cancellationToken
        ).ConfigureAwait(false);
        return mapper.Map<IEnumerable<AgentDataDTO>>(domainResult);
    }

    /// <summary>
    /// Updates the existing agent data.
    /// </summary>
    /// <param name="updateAgentData">The update agent data DTO model.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> UpdateExistingAgentDataAsync(AgentDataDTO updateAgentData, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        var domainRequest = mapper.Map<AgentDataDomain>(updateAgentData);
        return await agentsService.UpdateExistingAgentDataAsync(
            updateDataDomain: domainRequest,
            userEmail: currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes an existing agent data.
    /// </summary>
    /// <param name="agentId">The agent id.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> DeleteExistingAgentDataAsync(string agentId, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        return await agentsService.DeleteExistingAgentDataAsync(
            agentId,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Downloads the knowledgebase file asynchronous.
    /// </summary>
    /// <param name="agentGuid">The agent guid id.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The downloaded file url</returns>
    public async Task<string> DownloadKnowledgebaseFileAsync(string agentGuid, string fileName, CancellationToken cancellationToken = default)
    {
        return await agentsService.DownloadKnowledgebaseFileAsync(
            agentGuid,
            fileName,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
