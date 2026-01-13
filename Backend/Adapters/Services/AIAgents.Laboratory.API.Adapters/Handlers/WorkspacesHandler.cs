using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The agent workspaces api adapter handler implementation.
/// </summary>
/// <param name="mapper">The auto mapper service.</param>
/// <param name="workspacesService">The workspace service.</param>
/// <seealso cref="IWorkspacesHandler"/>
public class WorkspacesHandler(IMapper mapper, IWorkspacesService workspacesService) : IWorkspacesHandler
{
    /// <summary>
    /// Gets the collection of all available workspaces.
    /// </summary>
    /// <param name="userName">The current logged in user name.</param>
    /// <returns>The list of <see cref="AgentsWorkspaceDTO"/></returns>
    public async Task<IEnumerable<AgentsWorkspaceDTO>> GetAllWorkspacesAsync(string userName)
    {
        var domainResult = await workspacesService.GetAllWorkspacesAsync(userName).ConfigureAwait(false);
        return mapper.Map<IEnumerable<AgentsWorkspaceDTO>>(domainResult);
    }
}
