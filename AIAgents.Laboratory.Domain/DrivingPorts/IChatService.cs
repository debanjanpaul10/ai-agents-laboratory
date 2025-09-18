using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

namespace AIAgents.Laboratory.Domain.DrivingPorts;

/// <summary>
/// The Chat Service interface.
/// </summary>
public interface IChatService
{
	/// <summary>
	/// Gets the agent chat response asynchronous.
	/// </summary>
	/// <param name="chatRequest">The chat request.</param>
	/// <returns>The AI response.</returns>
	Task<string> GetAgentChatResponseAsync(ChatRequestDomain chatRequest);
}
