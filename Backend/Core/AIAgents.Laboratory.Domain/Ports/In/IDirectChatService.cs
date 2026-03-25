using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.Ports.In;

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
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The AI response.</returns>
    Task<string> GetDirectChatResponseAsync(string userQuery, string userEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears the conversation history data for the user.
    /// </summary>
    /// <param name="userName">The user name for user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> ClearConversationHistoryForUserAsync(string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the conversation history data for user.
    /// </summary>
    /// <param name="userName">The current user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The conversation history data domain model.</returns>
    Task<ConversationHistoryDomain> GetConversationHistoryDataAsync(string userName, CancellationToken cancellationToken = default);
}
