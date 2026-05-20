using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Mapper;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.Ports.In;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The agent workspaces api adapter handler implementation.
/// </summary>
/// <param name="workspacesService">The workspace service.</param>
/// <seealso cref="IWorkspacesHandler"/>
public sealed class WorkspacesHandler(
    IWorkspacesService workspacesService) : IWorkspacesHandler
{
    /// <inheritdoc/>
    public async Task<bool> CreateNewWorkspaceAsync(
        AgentsWorkspaceDTO agentsWorkspaceData,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        var domainModel = DomainMapperProfile.MapToDomain(dto: agentsWorkspaceData);
        return await workspacesService.CreateNewWorkspaceAsync(
            agentsWorkspaceData: domainModel,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteExistingWorkspaceAsync(
        string workspaceGuidId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        return await workspacesService.DeleteExistingWorkspaceAsync(
            workspaceGuidId,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AgentsWorkspaceDTO>> GetAllWorkspacesAsync(
        string userName,
        CancellationToken cancellationToken = default
    )
    {
        var domainResult = await workspacesService.GetAllWorkspacesAsync(
            currentUserEmail: userName,
            cancellationToken
        ).ConfigureAwait(false);
        return [.. domainResult.Select(DomainMapperProfile.MapToDto)];
    }

    /// <inheritdoc/>
    public async Task<GroupChatResponseDto> GetWorkspaceGroupChatResponseAsync(
        WorkspaceAgentChatRequestDto chatRequest,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        var domainInput = DomainMapperProfile.MapToDomain(dto: chatRequest);
        domainInput.CurrentUserEmail = currentUserEmail;
        var domainResult = await workspacesService.GetWorkspaceGroupChatResponseAsync(
            chatRequest: domainInput,
            cancellationToken
        ).ConfigureAwait(false);
        return DomainMapperProfile.MapToDto(domainResult);
    }

    /// <inheritdoc/>
    public async Task<AgentsWorkspaceDTO> GetWorkspaceByWorkspaceIdAsync(
        string workspaceId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        var domainResult = await workspacesService.GetWorkspaceByWorkspaceIdAsync(
            workspaceId,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
        return DomainMapperProfile.MapToDto(domainResult);
    }

    /// <inheritdoc/>
    public async Task<string> InvokeWorkspaceAgentAsync(
        WorkspaceAgentChatRequestDto chatRequestDTO,
        CancellationToken cancellationToken = default
    )
    {
        var domainInput = DomainMapperProfile.MapToDomain(dto: chatRequestDTO);
        return await workspacesService.InvokeWorkspaceAgentAsync(
            chatRequest: domainInput,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateExistingWorkspaceDataAsync(
        AgentsWorkspaceDTO agentsWorkspaceData,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        var domainModel = DomainMapperProfile.MapToDomain(dto: agentsWorkspaceData);
        return await workspacesService.UpdateExistingWorkspaceDataAsync(
            agentsWorkspaceData: domainModel,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ClearWorkspaceConversationHistoryAsync(
        string workspaceId,
        string currentUserEmail,
        string conversationId,
        CancellationToken cancellationToken = default
    )
    {
        return await workspacesService.ClearWorkspaceConversationHistoryAsync(
            workspaceId,
            currentUserEmail,
            conversationId,
            cancellationToken
        ).ConfigureAwait(false);
    }
}

