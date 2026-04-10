using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;

namespace AIAgents.Laboratory.Domain.Ports.Out;

/// <summary>
/// Provides an interface for managing workspace data, including creating, retrieving, updating, and deleting workspaces.
/// This interface abstracts the underlying data storage mechanism, allowing for flexibility in how workspace data is persisted
/// </summary>
public interface IWorkspacesDataManager
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
}
