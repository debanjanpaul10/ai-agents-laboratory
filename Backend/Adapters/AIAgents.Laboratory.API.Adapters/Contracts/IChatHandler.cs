using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// The Chat API adapter handler interface.
/// </summary>
public interface IChatHandler
{
	/// <summary>
	/// Invokes the chat agent asynchronous.
	/// </summary>
	/// <param name="chatRequestDTO">The chat request dto.</param>
	/// <returns>The chatbot response.</returns>
	Task<string> InvokeChatAgentAsync(ChatRequestDTO chatRequestDTO);

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

	/// <summary>
	/// Gets the conversation history data for user.
	/// </summary>
	/// <param name="userName">The current user name.</param>
	/// <returns>The conversation history data domain model.</returns>
	Task<ConversationHistoryDTO> GetConversationHistoryDataAsync(string userName);
}
