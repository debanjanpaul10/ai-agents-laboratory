using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The Agent Skills API handler adapter.
/// </summary>
/// <param name="agentSkillsService">The agent skills service.</param>
/// <seealso cref="IAgentSkillsHandler" />
public class AgentSkillsHandler(IMapper mapper, IAgentSkillsService agentSkillsService) : IAgentSkillsHandler
{
	/// <summary>
	/// Creates the new skill asynchronous.
	/// </summary>
	/// <param name="agentSkillRequest">The agent skill request.</param>
	/// <returns>
	/// The boolean for success/failure.
	/// </returns>
	public async Task<bool> CreateNewSkillAsync(AgentSkillDTO agentSkillRequest)
	{
		var domainInput = mapper.Map<AgentSkillDomain>(agentSkillRequest);
		return await agentSkillsService.CreateNewSkillAsync(domainInput).ConfigureAwait(false);
	}

	/// <summary>
	/// Gets all skills asynchronous.
	/// </summary>
	/// <returns>
	/// The list of <see cref="AgentSkillDTO" />
	/// </returns>
	public async Task<IEnumerable<AgentSkillDTO>> GetAllSkillsAsync()
	{
		var domainResult = await agentSkillsService.GetAllSkillsAsync().ConfigureAwait(false);
		return mapper.Map<IEnumerable<AgentSkillDTO>>(domainResult);
	}

	/// <summary>
	/// Gets the skill by identifier asynchronous.
	/// </summary>
	/// <param name="skillId">The skill identifier.</param>
	/// <returns>
	/// The agent skill DTO.
	/// </returns>
	public async Task<AgentSkillDTO> GetSkillByIdAsync(string skillId)
	{
		var domainResult = await agentSkillsService.GetSkillByIdAsync(skillId).ConfigureAwait(false);
		return mapper.Map<AgentSkillDTO>(domainResult);
	}
}
