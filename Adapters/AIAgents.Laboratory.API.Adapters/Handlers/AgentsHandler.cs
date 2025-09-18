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
public class AgentsHandler(IMapper mapper, IAgentsService agentsService) : IAgentsHandler
{
	/// <summary>
	/// Creates the new agent asynchronous.
	/// </summary>
	/// <param name="agentData">The agent data.</param>
	/// <returns>
	/// The boolean for success/failure.
	/// </returns>
	public async Task<bool> CreateNewAgentAsync(CreateAgentDTO agentData)
	{
		var domainInput = mapper.Map<AgentDataDomain>(agentData);
		return await agentsService.CreateNewAgentAsync(domainInput).ConfigureAwait(false);
	}

	/// <summary>
	/// Gets the agent data by identifier asynchronous.
	/// </summary>
	/// <param name="agentId">The agent identifier.</param>
	/// <returns>
	/// The agent data dto.
	/// </returns>
	public async Task<AgentDataDTO> GetAgentDataByIdAsync(string agentId)
	{
		var domainResult = await agentsService.GetAgentDataByIdAsync(agentId).ConfigureAwait(false);
		return mapper.Map<AgentDataDTO>(domainResult);
	}

	/// <summary>
	/// Gets all agents data asynchronous.
	/// </summary>
	/// <returns>
	/// The list of <see cref="AgentDataDTO" />
	/// </returns>
	public async Task<IEnumerable<AgentDataDTO>> GetAllAgentsDataAsync()
	{
		var domainResult = await agentsService.GetAllAgentsDataAsync().ConfigureAwait(false);
		return mapper.Map<IEnumerable<AgentDataDTO>>(domainResult);
	}
}
