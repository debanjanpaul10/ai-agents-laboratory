using AIAgents.Laboratory.Domain.DomainEntities;
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

	/// <summary>
	/// Gets the direct chat response.
	/// </summary>
	/// <param name="userQuery">The user query.</param>
	/// <param name="userEmail">The user email address.</param>
	/// <returns>The AI response.</returns>
	Task<string> GetDirectChatResponseAsync(string userQuery, string userEmail);

	/// <summary>
	/// Clears the conversation history data for the user.
	/// </summary>
	/// <param name="userName">The user name for user.</param>
	/// <returns>The boolean for success/failure.</returns>
	Task<bool> ClearConversationHistoryForUserAsync(string userName);
}
