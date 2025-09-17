using AIAgents.Laboratory.API.Adapters.Models.Request;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// The Agent Skills API adapter handler interface.
/// </summary>
public interface IAgentSkillsHandler
{
	/// <summary>
	/// Creates the new skill asynchronous.
	/// </summary>
	/// <param name="agentSkillRequest">The agent skill request.</param>
	/// <returns>The boolean for success/failure.</returns>
	Task<bool> CreateNewSkillAsync(AgentSkillDTO agentSkillRequest);

	/// <summary>
	/// Gets all skills asynchronous.
	/// </summary>
	/// <returns>The list of <see cref="AgentSkillDTO"/></returns>
	Task<IEnumerable<AgentSkillDTO>> GetAllSkillsAsync();

	/// <summary>
	/// Gets the skill by identifier asynchronous.
	/// </summary>
	/// <param name="skillId">The skill identifier.</param>
	/// <returns>The agent skill DTO.</returns>
	Task<AgentSkillDTO> GetSkillByIdAsync(string skillId);
}
