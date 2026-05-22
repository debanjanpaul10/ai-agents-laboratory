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
    Task<ConversationHistoryDomain> GetConversationHistoryAsync(
        string userName,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets the conversation history data for current chat by workspace id.
    /// </summary>
    /// <remarks>This method is designed for multi-agent collaboration scenario, where agents are working in the same workspace and need to access the same conversation history.</remarks>
    /// <param name="workspaceId">The workspace id.</param>
    /// <param name="conversationId">The conversation id for current chat session, which is used to identify the unique conversation history for different chat sessions in the same workspace.</param>
    /// <param name="currentUserEmail">The current logged in user email, which is used to identify the conversation history for different users.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The conversation history domain.</returns>
    Task<ConversationHistoryDomain> GetConversationHistoryByWorkspaceAsync(
        string workspaceId,
        string conversationId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Saves the current message data to conversation history.
    /// </summary>
    /// <param name="conversationHistory">The conversation history.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> SaveMessageToConversationHistoryAsync(
        ConversationHistoryDomain conversationHistory,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Clears the conversation history data for the user.
    /// </summary>
    /// <param name="userName">The user name for user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> ClearConversationHistoryForUserAsync(
        string userName,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Clears the conversation history data for current chat by workspace id.
    /// </summary>
    /// <param name="workspaceId">The workspace id.</param>
    /// <param name="conversationId">The conversation id.</param>
    /// <param name="currentUserEmail">The current user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> ClearConversationHistoryByWorkspaceAsync(
        string workspaceId,
        string conversationId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Initializes the workspace conversation asynchronous, which will create a new conversation for the workspace and return the conversation id.
    /// </summary>
    /// <param name="workspaceGuid">The workspace GUID.</param>
    /// <param name="userOrApplicationName">The user or application name string.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The conversation ID.</returns>
    Task<string> InitializeWorkspaceConversationAsync(
        string workspaceGuid,
        string userOrApplicationName,
        CancellationToken cancellationToken = default
    );
}
