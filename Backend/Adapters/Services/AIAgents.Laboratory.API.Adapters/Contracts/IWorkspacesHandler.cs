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
}
