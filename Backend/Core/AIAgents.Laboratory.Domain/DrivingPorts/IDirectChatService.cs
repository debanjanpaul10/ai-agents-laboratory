using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.DrivingPorts;

/// <summary>
/// Defines methods for managing direct chat interaction services.
/// </summary>
public interface IDirectChatService
{
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
    Task<ConversationHistoryDomain> GetConversationHistoryDataAsync(string userName);
}
