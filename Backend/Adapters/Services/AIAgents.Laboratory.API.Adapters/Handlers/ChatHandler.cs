using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Mapper;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.Domain.Ports.In;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The Chat API adapter handler class.
/// </summary>
/// <param name="agentChatService">The agent chat service.</param>
/// <param name="directChatService">The direct chat service.</param>
/// <seealso cref="IChatHandler" />
public sealed class ChatHandler(
    IAgentChatService agentChatService,
    IDirectChatService directChatService) : IChatHandler
{
    #region DIRECT CHAT SERVICE

    /// <inheritdoc />
    public async Task<string> GetDirectChatResponseAsync(
        string userQuery,
        string userEmail,
        CancellationToken cancellationToken = default
    )
    {
        return await directChatService.GetDirectChatResponseAsync(
            userQuery,
            userEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    #endregion

    #region AGENT CHAT SERVICE

    /// <inheritdoc />
    public async Task<string> InvokeChatAgentAsync(
        ChatRequestDTO chatRequestDTO,
        CancellationToken cancellationToken = default
    )
    {
        var domainInput = DomainMapperProfile.MapToDomain(dto: chatRequestDTO);
        return await agentChatService.GetAgentChatResponseAsync(
            chatRequest: domainInput,
            cancellationToken
        ).ConfigureAwait(false);
    }

    #endregion
}
