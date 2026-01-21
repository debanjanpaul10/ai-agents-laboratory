using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The agent workspaces api adapter handler implementation.
/// </summary>
/// <param name="mapper">The auto mapper service.</param>
/// <param name="workspacesService">The workspace service.</param>
/// <seealso cref="IWorkspacesHandler"/>
public sealed class WorkspacesHandler(IMapper mapper, IWorkspacesService workspacesService) : IWorkspacesHandler
{
    /// <summary>
    /// Creates a new workspace.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data.</param>
    /// <param name="currentUserEmail">The current user email address.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> CreateNewWorkspaceAsync(AgentsWorkspaceDTO agentsWorkspaceData, string currentUserEmail)
    {
        var domainModel = mapper.Map<AgentsWorkspaceDomain>(agentsWorkspaceData);
        return await workspacesService.CreateNewWorkspaceAsync(domainModel, currentUserEmail).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes the existing workspace by workspace guid id.
    /// </summary>
    /// <param name="workspaceGuidId">The workspace guid id.</param>
    /// <param name="currentUserEmail">The current logged in user email address.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> DeleteExistingWorkspaceAsync(string workspaceGuidId, string currentUserEmail)
    {
        return await workspacesService.DeleteExistingWorkspaceAsync(workspaceGuidId, currentUserEmail).ConfigureAwait(false);
    }

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

    /// <summary>
    /// Gets the workspace by workspace id.
    /// </summary>
    /// <param name="workspaceId">The workspace id.</param>
    /// <param name="currentUserEmail">The current logged in user email</param>
    /// <returns>The agent workspace domain model.</returns>
    public async Task<AgentsWorkspaceDTO> GetWorkspaceByWorkspaceIdAsync(string workspaceId, string currentUserEmail)
    {
        var domainResult = await workspacesService.GetWorkspaceByWorkspaceIdAsync(workspaceId, currentUserEmail).ConfigureAwait(false);
        return mapper.Map<AgentsWorkspaceDTO>(domainResult);
    }

    /// <summary>
    /// Invoke the workspace agent with user message and get the response.
    /// </summary>
    /// <param name="chatRequestDTO">The chat request dto model.</param>
    /// <returns>The string response from AI.</returns>
    public async Task<string> InvokeWorkspaceAgentAsync(WorkspaceAgentChatRequestDTO chatRequestDTO)
    {
        var domainInput = mapper.Map<WorkspaceAgentChatRequestDomain>(chatRequestDTO);
        return await workspacesService.InvokeWorkspaceAgentAsync(domainInput).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates the existing workspace data.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data domain model.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> UpdateExistingWorkspaceDataAsync(AgentsWorkspaceDTO agentsWorkspaceData, string currentUserEmail)
    {
        var domainModel = mapper.Map<AgentsWorkspaceDomain>(agentsWorkspaceData);
        return await workspacesService.UpdateExistingWorkspaceDataAsync(domainModel, currentUserEmail).ConfigureAwait(false);
    }
}
