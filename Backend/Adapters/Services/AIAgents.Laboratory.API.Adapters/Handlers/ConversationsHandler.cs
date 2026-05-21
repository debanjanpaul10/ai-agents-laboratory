using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Mapper;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.Contracts;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The conversations handler, which is responsible for handling the conversation history related operations, such as getting conversation history data and clearing conversation history.
/// </summary>
/// <param name="conversationHistoryService">The conversation history service.</param>
/// <seealso cref="IConversationsHandler"/>
public sealed class ConversationsHandler(
    IConversationHistoryService conversationHistoryService) : IConversationsHandler
{
    /// <inheritdoc />
    public async Task<ConversationHistoryDTO> GetConversationHistoryDataAsync(
        string userName,
        CancellationToken cancellationToken = default
    )
    {
        var domainResult = await conversationHistoryService.GetConversationHistoryAsync(
            userName,
            cancellationToken
        ).ConfigureAwait(false);
        return DomainMapperProfile.MapToDto(domain: domainResult);
    }

    /// <inheritdoc/>
    public async Task<ConversationHistoryDTO> GetWorkspaceConversationHistoryAsync(
        string workspaceId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        var domainResult = await conversationHistoryService.GetConversationHistoryByWorkspaceAsync(
            workspaceId,
            conversationId: string.Empty,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
        return DomainMapperProfile.MapToDto(domain: domainResult);
    }

    /// <inheritdoc/>
    public async Task<bool> ClearWorkspaceConversationHistoryAsync(
        string workspaceId,
        string currentUserEmail,
        string conversationId,
        CancellationToken cancellationToken = default
    )
    {
        return await conversationHistoryService.ClearConversationHistoryByWorkspaceAsync(
            workspaceId,
            conversationId,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> ClearConversationHistoryForUserAsync(
        string userName,
        CancellationToken cancellationToken = default
    )
    {
        return await conversationHistoryService.ClearConversationHistoryForUserAsync(
            userName,
            cancellationToken
        ).ConfigureAwait(false);
    }


}