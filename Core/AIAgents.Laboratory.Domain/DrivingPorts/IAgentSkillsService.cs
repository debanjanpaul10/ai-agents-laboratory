using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

namespace AIAgents.Laboratory.Domain.DrivingPorts;

/// <summary>
/// The Agent Skills Business service interface.
/// </summary>
public interface IAgentSkillsService
{
	/// <summary>
	/// Creates the new skill asynchronous.
	/// </summary>
	/// <param name="agentSkillRequest">The agent skill request.</param>
	/// <returns>The boolean for success/failure.</returns>
	Task<bool> CreateNewSkillAsync(AgentSkillDomain agentSkillRequest);

	/// <summary>
	/// Gets all skills asynchronous.
	/// </summary>
	/// <returns>The list of <see cref="AgentSkillDomain"/></returns>
	Task<IEnumerable<AgentSkillDomain>> GetAllSkillsAsync();

	/// <summary>
	/// Gets the skill by identifier asynchronous.
	/// </summary>
	/// <param name="skillId">The skill identifier.</param>
	/// <returns>The agent skill DTO.</returns>
	Task<AgentSkillDomain> GetSkillByIdAsync(string skillId);
}
