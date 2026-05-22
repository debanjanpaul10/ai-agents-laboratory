using AIAgents.Laboratory.API.Adapters.Models.Response;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// The conversations handler interface, which is responsible for handling the conversation history related operations, such as getting conversation history data and clearing conversation history.
/// </summary>
public interface IConversationsHandler
{
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
    /// Gets the conversation history data for user.
    /// </summary>
    /// <param name="userName">The current user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The conversation history data domain model.</returns>
    Task<ConversationHistoryDTO> GetConversationHistoryDataAsync(
        string userName,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Clears the workspace conversation history.
    /// </summary>
    /// <param name="workspaceId">The workspace id.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="conversationId">The conversation id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean indicating success or failure.</returns>
    Task<bool> ClearWorkspaceConversationHistoryAsync(
        string workspaceId,
        string currentUserEmail,
        string conversationId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets the workspace conversation history asynchronous.
    /// </summary>
    /// <param name="workspaceId">The workspace identifier.</param>
    /// <param name="currentUserEmail">The current user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The conversation history.</returns>
    Task<ConversationHistoryDTO> GetWorkspaceConversationHistoryAsync(
        string workspaceId,
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