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
	/// <returns>The boolean for success/failure.</returns>
	Task<bool> CreateNewAgentAsync(AgentDataDomain agentData);

	/// <summary>
	/// Gets all agents data asynchronous.
	/// </summary>
	/// <returns>The list of <see cref="AgentDataDomain"/></returns>
	Task<IEnumerable<AgentDataDomain>> GetAllAgentsDataAsync();

	/// <summary>
	/// Gets the agent data by identifier asynchronous.
	/// </summary>
	/// <param name="agentId">The agent identifier.</param>
	/// <returns>The agent data dto.</returns>
	Task<AgentDataDomain> GetAgentDataByIdAsync(string agentId);
}
