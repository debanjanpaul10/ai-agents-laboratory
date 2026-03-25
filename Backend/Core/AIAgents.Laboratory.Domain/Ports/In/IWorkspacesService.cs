using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;

namespace AIAgents.Laboratory.Domain.Ports.In;

/// <summary>
/// The Workspace Service interface definition.
/// </summary>
public interface IWorkspacesService
{
    /// <summary>
    /// Gets the collection of all available workspaces.
    /// </summary>
    /// <param name="currentUserEmail">The current logged in user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="AgentsWorkspaceDomain"/></returns>
    Task<IEnumerable<AgentsWorkspaceDomain>> GetAllWorkspacesAsync(string currentUserEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the workspace by workspace id.
    /// </summary>
    /// <param name="workspaceId">The workspace id.</param>
    /// <param name="currentUserEmail">The current logged in user email</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The agent workspace domain model.</returns>
    Task<AgentsWorkspaceDomain> GetWorkspaceByWorkspaceIdAsync(string workspaceId, string currentUserEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new workspace.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data.</param>
    /// <param name="currentUserEmail">The current user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    Task<bool> CreateNewWorkspaceAsync(AgentsWorkspaceDomain agentsWorkspaceData, string currentUserEmail, CancellationToken cancellationToken = default);

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
    Task<bool> UpdateExistingWorkspaceDataAsync(AgentsWorkspaceDomain agentsWorkspaceData, string currentUserEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Invoke the workspace agent with user message and get the response.
    /// </summary>
    /// <param name="chatRequest">The chat request domain model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The string response from AI.</returns>
    Task<string> InvokeWorkspaceAgentAsync(WorkspaceAgentChatRequestDomain chatRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the workspace group chat response.
    /// </summary>
    /// <param name="chatRequest">The workspace agent chat request dto model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The group chat response.</returns>
    Task<GroupChatResponseDomain> GetWorkspaceGroupChatResponseAsync(WorkspaceAgentChatRequestDomain chatRequest, CancellationToken cancellationToken = default);
}
