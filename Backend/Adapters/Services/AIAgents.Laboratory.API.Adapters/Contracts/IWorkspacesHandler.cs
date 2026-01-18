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
    /// <returns>The list of <see cref="AgentsWorkspaceDTO"/></returns>
    Task<IEnumerable<AgentsWorkspaceDTO>> GetAllWorkspacesAsync(string userName);

    /// <summary>
    /// Gets the workspace by workspace id.
    /// </summary>
    /// <param name="workspaceId">The workspace id.</param>
    /// <param name="currentUserEmail">The current logged in user email</param>
    /// <returns>The agent workspace domain model.</returns>
    Task<AgentsWorkspaceDTO> GetWorkspaceByWorkspaceIdAsync(string workspaceId, string currentUserEmail);

    /// <summary>
    /// Creates a new workspace.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data.</param>
    /// <param name="currentUserEmail">The current user email address.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    Task<bool> CreateNewWorkspaceAsync(AgentsWorkspaceDTO agentsWorkspaceData, string currentUserEmail);

    /// <summary>
    /// Deletes the existing workspace by workspace guid id.
    /// </summary>
    /// <param name="workspaceGuidId">The workspace guid id.</param>
    /// <param name="currentUserEmail">The current logged in user email address.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    Task<bool> DeleteExistingWorkspaceAsync(string workspaceGuidId, string currentUserEmail);

    /// <summary>
    /// Updates the existing workspace data.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data domain model.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    Task<bool> UpdateExistingWorkspaceDataAsync(AgentsWorkspaceDTO agentsWorkspaceData, string currentUserEmail);
}
