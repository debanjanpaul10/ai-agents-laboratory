using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides services for managing direct chat interactions.
/// </summary>
/// <param name="logger">The logger used to record diagnostic and operational information for the chat service.</param>
/// <param name="configuration">The application configuration provider used to access settings required by the chat service.</param>
/// <param name="correlationContext">The correlation context used for tracing and logging purposes across service calls.</param>
/// <param name="agentsService">The service used to retrieve agent data and metadata for chatbot interactions.</param>
/// <param name="aiServices">The AI services provider used to generate chatbot responses based on conversation history and user input.</param>
/// <param name="conversationHistoryService">The service responsible for managing and persisting user conversation history.</param>
/// <seealso cref="IDirectChatService"/>
public sealed class DirectChatService(
    ILogger<DirectChatService> logger,
    IConfiguration configuration,
    IAgentsService agentsService,
    ICorrelationContext correlationContext,
    IAiServices aiServices,
    IConversationHistoryService conversationHistoryService) : IDirectChatService
{
    /// <summary>
    /// Gets the chatbot response.
    /// </summary>
    /// <param name="userQuery">The user query.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The AI response.</returns>
    public async Task<string> GetDirectChatResponseAsync(string userQuery, string userEmail, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userQuery);
        ArgumentException.ThrowIfNullOrWhiteSpace(userEmail);

        string aiResponse = string.Empty;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetDirectChatResponseAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userEmail, userQuery }));

            var chatbotAgentGuid = configuration[AzureAppConfigurationConstants.AIChatbotAgentId] ?? throw new KeyNotFoundException(ExceptionConstants.AgentNotFoundExceptionMessage);
            var agentDataTask = agentsService.GetAgentDataByIdAsync(
                agentId: chatbotAgentGuid,
                userEmail,
                cancellationToken);
            var conversationHistoryTask = conversationHistoryService.GetConversationHistoryAsync(
                userName: userEmail,
                cancellationToken);
            await Task.WhenAll(agentDataTask, conversationHistoryTask)
                .WaitAsync(cancellationToken).ConfigureAwait(false);

            var conversationHistoryData = conversationHistoryTask.Result;
            var agentMetaprompt = agentDataTask.Result.AgentMetaPrompt;
            var chatHistoryList = conversationHistoryData.ChatHistory.ToList();

            aiResponse = await aiServices.GetChatbotResponseAsync(
                conversationDataDomain: conversationHistoryData,
                userMessage: userQuery,
                agentMetaPrompt: agentMetaprompt,
                cancellationToken
            ).ConfigureAwait(false);

            chatHistoryList.Add(new ChatHistoryDomain
            {
                Role = ChatbotHelperConstants.AssistantRoleConstant,
                Content = aiResponse
            });
            conversationHistoryData.ChatHistory = chatHistoryList;
            await conversationHistoryService.SaveMessageToConversationHistoryAsync(
                conversationHistory: conversationHistoryData,
                cancellationToken
            ).ConfigureAwait(false);

            return aiResponse;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetDirectChatResponseAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetDirectChatResponseAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userEmail, aiResponse }));
        }
    }

    /// <summary>
    /// Clears the conversation history data for the user.
    /// </summary>
    /// <param name="userName">The user name for user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> ClearConversationHistoryForUserAsync(string userName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName }));

            return await conversationHistoryService.ClearConversationHistoryForUserAsync(
                userName,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName }));
        }
    }

    /// <summary>
    /// Gets the conversation history data for user.
    /// </summary>
    /// <param name="userName">The current user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The conversation history data domain model.</returns>
    public async Task<ConversationHistoryDomain> GetConversationHistoryDataAsync(string userName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        ConversationHistoryDomain? result = null;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetConversationHistoryDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName }));

            result = await conversationHistoryService.GetConversationHistoryAsync(
                userName,
                cancellationToken
            ).ConfigureAwait(false) ?? new();
            return result;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetConversationHistoryDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetConversationHistoryDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userName, result }));
        }
    }
}
