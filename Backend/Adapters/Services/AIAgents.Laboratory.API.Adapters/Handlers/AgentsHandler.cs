using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DrivingPorts;
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
    /// <returns>
    /// The boolean for success/failure.
    /// </returns>
    public async Task<bool> CreateNewAgentAsync(CreateAgentDTO agentData, string userEmail)
    {
        var domainInput = mapper.Map<AgentDataDomain>(agentData);
        return await agentsService.CreateNewAgentAsync(domainInput, userEmail).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the agent data by identifier asynchronous.
    /// </summary>
    /// <param name="agentId">The agent identifier.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <returns>
    /// The agent data dto.
    /// </returns>
    public async Task<AgentDataDTO> GetAgentDataByIdAsync(string agentId, string userEmail)
    {
        var domainResult = await agentsService.GetAgentDataByIdAsync(agentId, userEmail).ConfigureAwait(false);
        return mapper.Map<AgentDataDTO>(domainResult);
    }

    /// <summary>
    /// Gets all agents data asynchronous.
    /// </summary>
    /// <param name="userEmail">The current logged in user email.</param>
    /// <returns>The list of <see cref="AgentDataDTO"/></returns>
    public async Task<IEnumerable<AgentDataDTO>> GetAllAgentsDataAsync(string userEmail)
    {
        var domainResult = await agentsService.GetAllAgentsDataAsync(userEmail).ConfigureAwait(false);
        return mapper.Map<IEnumerable<AgentDataDTO>>(domainResult);
    }

    /// <summary>
    /// Updates the existing agent data.
    /// </summary>
    /// <param name="updateAgentData">The update agent data DTO model.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> UpdateExistingAgentDataAsync(AgentDataDTO updateAgentData, string currentUserEmail)
    {
        var domainRequest = mapper.Map<AgentDataDomain>(updateAgentData);
        return await agentsService.UpdateExistingAgentDataAsync(domainRequest, currentUserEmail).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes an existing agent data.
    /// </summary>
    /// <param name="agentId">The agent id.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> DeleteExistingAgentDataAsync(string agentId)
    {
        return await agentsService.DeleteExistingAgentDataAsync(agentId).ConfigureAwait(false);
    }
}
