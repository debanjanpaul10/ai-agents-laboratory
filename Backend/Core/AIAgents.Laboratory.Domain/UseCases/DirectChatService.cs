using System.Globalization;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides services for managing direct chat interactions.
/// </summary>
/// <param name="logger">The logger used to record diagnostic and operational information for the chat service.</param>
/// <param name="configuration">The application configuration provider used to access settings required by the chat service.</param>
/// <param name="agentsService">The service used to retrieve agent data and metadata for chatbot interactions.</param>
/// <param name="aiServices">The AI services provider used to generate chatbot responses based on conversation history and user input.</param>
/// <param name="conversationHistoryService">The service responsible for managing and persisting user conversation history.</param>
/// <seealso cref="IDirectChatService"/>
public sealed class DirectChatService(ILogger<AgentChatService> logger, IConfiguration configuration, IAgentsService agentsService,
    IAiServices aiServices, IConversationHistoryService conversationHistoryService) : IDirectChatService
{
    /// <summary>
    /// Gets the chatbot response.
    /// </summary>
    /// <param name="userQuery">The user query.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <returns>The AI response.</returns>
    public async Task<string> GetDirectChatResponseAsync(string userQuery, string userEmail)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetDirectChatResponseAsync), DateTime.UtcNow, userQuery));

            ArgumentException.ThrowIfNullOrEmpty(userQuery);

            var chatbotAgentGuid = configuration[AzureAppConfigurationConstants.AIChatbotAgentId] ?? throw new Exception(ExceptionConstants.AgentNotFoundExceptionMessage);
            var agentDataTask = agentsService.GetAgentDataByIdAsync(chatbotAgentGuid, userEmail);
            var conversationHistoryTask = conversationHistoryService.GetConversationHistoryAsync(userEmail);
            await Task.WhenAll(agentDataTask, conversationHistoryTask).ConfigureAwait(false);

            var conversationHistoryData = conversationHistoryTask.Result;
            var agentMetaprompt = agentDataTask.Result.AgentMetaPrompt;
            var chatHistoryList = conversationHistoryData.ChatHistory.ToList();
            chatHistoryList.Add(new ChatHistoryDomain
            {
                Role = ChatbotHelperConstants.UserRoleConstant,
                Content = userQuery
            });
            conversationHistoryData.ChatHistory = chatHistoryList;
            await conversationHistoryService.SaveMessageToConversationHistoryAsync(conversationHistoryData).ConfigureAwait(false);

            var aiResponse = await aiServices.GetChatbotResponseAsync(conversationHistoryData, userQuery, agentMetaprompt).ConfigureAwait(false);
            chatHistoryList.Add(new ChatHistoryDomain
            {
                Role = ChatbotHelperConstants.AssistantRoleConstant,
                Content = aiResponse
            });
            conversationHistoryData.ChatHistory = chatHistoryList;
            await conversationHistoryService.SaveMessageToConversationHistoryAsync(conversationHistoryData).ConfigureAwait(false);
            return aiResponse;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetDirectChatResponseAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetDirectChatResponseAsync), DateTime.UtcNow, userQuery));
        }
    }

    /// <summary>
    /// Clears the conversation history data for the user.
    /// </summary>
    /// <param name="userName">The user name for user.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> ClearConversationHistoryForUserAsync(string userName)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, userName));
            return await conversationHistoryService.ClearConversationHistoryForUserAsync(userName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(ClearConversationHistoryForUserAsync), DateTime.UtcNow, userName));
        }
    }

    /// <summary>
    /// Gets the conversation history data for user.
    /// </summary>
    /// <param name="userName">The current user name.</param>
    /// <returns>The conversation history data domain model.</returns>
    public async Task<ConversationHistoryDomain> GetConversationHistoryDataAsync(string userName)
    {

        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetConversationHistoryDataAsync), DateTime.UtcNow, userName));
            return await conversationHistoryService.GetConversationHistoryAsync(userName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetConversationHistoryDataAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetConversationHistoryDataAsync), DateTime.UtcNow, userName));
        }
    }
}
