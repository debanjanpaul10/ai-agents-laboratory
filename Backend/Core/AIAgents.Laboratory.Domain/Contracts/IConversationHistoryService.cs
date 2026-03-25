using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.Contracts;

/// <summary>
/// The conversation history service interface.
/// </summary>
public interface IConversationHistoryService
{
    /// <summary>
    /// Gets the conversation history data for current chat.
    /// </summary>
    /// <param name="userName">The user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The conversation history domain.</returns>
    Task<ConversationHistoryDomain> GetConversationHistoryAsync(string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves the current message data to conversation history.
    /// </summary>
    /// <param name="conversationHistory">The conversation history.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> SaveMessageToConversationHistoryAsync(ConversationHistoryDomain conversationHistory, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears the conversation history data for the user.
    /// </summary>
    /// <param name="userName">The user name for user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> ClearConversationHistoryForUserAsync(string userName, CancellationToken cancellationToken = default);
}
