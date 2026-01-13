using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.DrivingPorts;

/// <summary>
/// The Workspace Service interface definition.
/// </summary>
public interface IWorkspacesService
{
    /// <summary>
    /// Gets the collection of all available workspaces.
    /// </summary>
    /// <param name="userName">The current logged in user name.</param>
    /// <returns>The list of <see cref="AgentsWorkspaceDomain"/></returns>
    Task<IEnumerable<AgentsWorkspaceDomain>> GetAllWorkspacesAsync(string userName);
}
