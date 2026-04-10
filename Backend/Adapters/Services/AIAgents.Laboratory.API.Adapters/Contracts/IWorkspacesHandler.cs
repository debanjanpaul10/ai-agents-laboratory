using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// The workspaces api adapter handler interface.
/// </summary>
public interface IWorkspacesHandler
{
    /// <summary>
    /// Gets the collection of all available workspaces.
    /// </summary>
    /// <param name="userName">The current logged in user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="AgentsWorkspaceDTO"/></returns>
    Task<IEnumerable<AgentsWorkspaceDTO>> GetAllWorkspacesAsync(string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the workspace by workspace id.
    /// </summary>
    /// <param name="workspaceId">The workspace id.</param>
    /// <param name="currentUserEmail">The current logged in user email</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The agent workspace domain model.</returns>
    Task<AgentsWorkspaceDTO> GetWorkspaceByWorkspaceIdAsync(string workspaceId, string currentUserEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new workspace.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data.</param>
    /// <param name="currentUserEmail">The current user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    Task<bool> CreateNewWorkspaceAsync(AgentsWorkspaceDTO agentsWorkspaceData, string currentUserEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the existing workspace by workspace guid id.
    /// </summary>
    /// <param name="workspaceGuidId">The workspace guid id.</param>
    /// <param name="currentUserEmail">The current logged in user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    Task<bool> DeleteExistingWorkspaceAsync(string workspaceGuidId, string currentUserEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the existing workspace data.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data domain model.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    Task<bool> UpdateExistingWorkspaceDataAsync(AgentsWorkspaceDTO agentsWorkspaceData, string currentUserEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Invoke the workspace agent with user message and get the response.
    /// </summary>
    /// <param name="chatRequestDTO">The chat request dto model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The string response from AI.</returns>
    Task<string> InvokeWorkspaceAgentAsync(WorkspaceAgentChatRequestDTO chatRequestDTO, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the workspace group chat response.
    /// </summary>
    /// <param name="chatRequest">The workspace agent chat request dto model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The group chat response.</returns>
    Task<GroupChatResponseDTO> GetWorkspaceGroupChatResponseAsync(WorkspaceAgentChatRequestDTO chatRequest, CancellationToken cancellationToken = default);
}
