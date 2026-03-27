using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.Ports.In;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The Chat API adapter handler class.
/// </summary>
/// <param name="mapper">The auto mapper service.</param>
/// <param name="agentChatService">The agent chat service.</param>
/// <param name="directChatService">The direct chat service.</param>
/// <seealso cref="IChatHandler" />
public sealed class ChatHandler(IMapper mapper, IAgentChatService agentChatService, IDirectChatService directChatService) : IChatHandler
{
    #region DIRECT CHAT SERVICE

    /// <summary>
    /// Gets the chatbot response.
    /// </summary>
    /// <param name="userQuery">The user query.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <returns>The AI response.</returns>
    public async Task<string> GetDirectChatResponseAsync(string userQuery, string userEmail, CancellationToken cancellationToken = default)
    {
        return await directChatService.GetDirectChatResponseAsync(
            userQuery,
            userEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Clears the conversation history data for the user.
    /// </summary>
    /// <param name="userName">The user name for user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> ClearConversationHistoryForUserAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await directChatService.ClearConversationHistoryForUserAsync(
            userName,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the conversation history data for user.
    /// </summary>
    /// <param name="userName">The current user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The conversation history data domain model.</returns>
    public async Task<ConversationHistoryDTO> GetConversationHistoryDataAsync(string userName, CancellationToken cancellationToken = default)
    {
        var domainResult = await directChatService.GetConversationHistoryDataAsync(
            userName,
            cancellationToken
        ).ConfigureAwait(false);
        return mapper.Map<ConversationHistoryDTO>(domainResult);
    }

    #endregion

    #region AGENT CHAT SERVICE

    /// <summary>
    /// Invokes the chat agent asynchronous.
    /// </summary>
    /// <param name="chatRequestDTO">The chat request dto.</param>
    /// <returns>
    /// The chatbot response.
    /// </returns>
    public async Task<string> InvokeChatAgentAsync(ChatRequestDTO chatRequestDTO, CancellationToken cancellationToken = default)
    {
        var domainInput = mapper.Map<ChatRequestDomain>(chatRequestDTO);
        return await agentChatService.GetAgentChatResponseAsync(
            chatRequest: domainInput,
            cancellationToken
        ).ConfigureAwait(false);
    }

    #endregion
}
