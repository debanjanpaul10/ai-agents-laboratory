using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
using AIAgents.Laboratory.Domain.Ports.In;
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
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> CreateNewWorkspaceAsync(AgentsWorkspaceDTO agentsWorkspaceData, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        var domainModel = mapper.Map<AgentsWorkspaceDomain>(agentsWorkspaceData);
        return await workspacesService.CreateNewWorkspaceAsync(
            agentsWorkspaceData: domainModel,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes the existing workspace by workspace guid id.
    /// </summary>
    /// <param name="workspaceGuidId">The workspace guid id.</param>
    /// <param name="currentUserEmail">The current logged in user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> DeleteExistingWorkspaceAsync(string workspaceGuidId, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        return await workspacesService.DeleteExistingWorkspaceAsync(
            workspaceGuidId,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the collection of all available workspaces.
    /// </summary>
    /// <param name="userName">The current logged in user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="AgentsWorkspaceDTO"/></returns>
    public async Task<IEnumerable<AgentsWorkspaceDTO>> GetAllWorkspacesAsync(string userName, CancellationToken cancellationToken = default)
    {
        var domainResult = await workspacesService.GetAllWorkspacesAsync(
            currentUserEmail: userName,
            cancellationToken
        ).ConfigureAwait(false);
        return mapper.Map<IEnumerable<AgentsWorkspaceDTO>>(domainResult);
    }

    /// <summary>
    /// Gets the workspace group chat response.
    /// </summary>
    /// <param name="chatRequest">The workspace agent chat request dto model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The group chat response.</returns>
    public async Task<GroupChatResponseDTO> GetWorkspaceGroupChatResponseAsync(WorkspaceAgentChatRequestDTO chatRequest, CancellationToken cancellationToken = default)
    {
        var domainInput = mapper.Map<WorkspaceAgentChatRequestDomain>(chatRequest);
        var domainResult = await workspacesService.GetWorkspaceGroupChatResponseAsync(
            chatRequest: domainInput,
            cancellationToken
        ).ConfigureAwait(false);
        return mapper.Map<GroupChatResponseDTO>(domainResult);
    }

    /// <summary>
    /// Gets the workspace by workspace id.
    /// </summary>
    /// <param name="workspaceId">The workspace id.</param>
    /// <param name="currentUserEmail">The current logged in user email</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The agent workspace domain model.</returns>
    public async Task<AgentsWorkspaceDTO> GetWorkspaceByWorkspaceIdAsync(string workspaceId, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        var domainResult = await workspacesService.GetWorkspaceByWorkspaceIdAsync(
            workspaceId,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
        return mapper.Map<AgentsWorkspaceDTO>(domainResult);
    }

    /// <summary>
    /// Invoke the workspace agent with user message and get the response.
    /// </summary>
    /// <param name="chatRequestDTO">The chat request dto model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The string response from AI.</returns>
    public async Task<string> InvokeWorkspaceAgentAsync(WorkspaceAgentChatRequestDTO chatRequestDTO, CancellationToken cancellationToken = default)
    {
        var domainInput = mapper.Map<WorkspaceAgentChatRequestDomain>(chatRequestDTO);
        return await workspacesService.InvokeWorkspaceAgentAsync(
            chatRequest: domainInput,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates the existing workspace data.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data domain model.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> UpdateExistingWorkspaceDataAsync(AgentsWorkspaceDTO agentsWorkspaceData, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        var domainModel = mapper.Map<AgentsWorkspaceDomain>(agentsWorkspaceData);
        return await workspacesService.UpdateExistingWorkspaceDataAsync(
            agentsWorkspaceData: domainModel,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
