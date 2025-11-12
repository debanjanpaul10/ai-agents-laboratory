using System.Globalization;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The Chat Service class.
/// </summary>
/// <param name="agentsService">The agents service.</param>
/// <param name="logger">The logger service.</param>
/// <param name="aiServices">The Ai services.</param>
/// <param name="conversationHistoryService">The conversation history service.</param>
/// <param name="knowledgeBaseProcessor">The knowledge base processor service.</param>
/// <seealso cref="AIAgents.Laboratory.Domain.DrivingPorts.IChatService" />
public class ChatService(ILogger<ChatService> logger, IAgentsService agentsService, IAiServices aiServices, IConversationHistoryService conversationHistoryService, IKnowledgeBaseProcessor knowledgeBaseProcessor) : IChatService
{
	/// <summary>
	/// Gets the agent chat response asynchronous.
	/// </summary>
	/// <param name="chatRequest">The chat request.</param>
	/// <returns>
	/// The AI response.
	/// </returns>
	/// <exception cref="System.NotImplementedException"></exception>
	public async Task<string> GetAgentChatResponseAsync(ChatRequestDomain chatRequest)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetAgentChatResponseAsync), DateTime.UtcNow, chatRequest.AgentId));

			var agentData = await agentsService.GetAgentDataByIdAsync(chatRequest.AgentId).ConfigureAwait(false);
			if (agentData is null || string.IsNullOrEmpty(agentData.AgentMetaPrompt))
			{
				var ex = new FileNotFoundException(ExceptionConstants.AgentNotFoundExceptionMessage);
				logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetAgentChatResponseAsync), DateTime.UtcNow, ex.Message));
				throw ex;
			}

			var chatMessage = new ChatMessageDomain()
			{
				AgentMetaPrompt = agentData.AgentMetaPrompt,
				AgentName = agentData.AgentName,
				UserMessage = chatRequest.UserMessage,
			};

			if (agentData.KnowledgeBaseDocument is not null || agentData.StoredKnowledgeBase is not null)
			{
				var relevantKnowledge = await knowledgeBaseProcessor.GetRelevantKnowledgeAsync(chatRequest.UserMessage, agentData.AgentId).ConfigureAwait(false);
				chatMessage.KnowledgeBase = relevantKnowledge;
			}

			return await aiServices.GetAiFunctionResponseAsync(chatMessage, ApplicationPluginsHelpers.PluginName, ApplicationPluginsHelpers.GetChatMessageResponseFunction.FunctionName).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetAgentChatResponseAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetAgentChatResponseAsync), DateTime.UtcNow, chatRequest.AgentId));
		}

	}

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
			var conversationHistoryData = await conversationHistoryService.GetConversationHistoryAsync(userEmail).ConfigureAwait(false);
			var chatHistoryList = conversationHistoryData.ChatHistory.ToList();
			chatHistoryList.Add(new ChatHistoryDomain
			{
				Role = ChatbotHelperConstants.UserRoleConstant,
				Content = userQuery
			});
			conversationHistoryData.ChatHistory = chatHistoryList;
			await conversationHistoryService.SaveMessageToConversationHistoryAsync(conversationHistoryData).ConfigureAwait(false);

			var aiResponse = await aiServices.GetChatbotResponseAsync(conversationHistoryData, userQuery).ConfigureAwait(false);
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
