using System.Globalization;
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
/// <seealso cref="AIAgents.Laboratory.Domain.DrivingPorts.IChatService" />
public class ChatService(ILogger<ChatService> logger, IAgentsService agentsService, IAiServices aiServices) : IChatService
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
		return await aiServices.GetAiFunctionResponseAsync(chatMessage, ApplicationPluginsHelpers.PluginName, ApplicationPluginsHelpers.GetChatMessageResponseFunction.FunctionName).ConfigureAwait(false);
	}

	/// <summary>
	/// Gets the chatbot response.
	/// </summary>
	/// <param name="userQuery">The user query.</param>
	/// <returns>The AI response.</returns>
	public async Task<string> GetDirectChatResponseAsync(string userQuery)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetDirectChatResponseAsync), DateTime.UtcNow, userQuery));

			ArgumentException.ThrowIfNullOrEmpty(userQuery);
			return await aiServices.GetAiFunctionResponseAsync(userQuery, ChatbotPluginHelpers.PluginName, ChatbotPluginHelpers.ConversationAgentFunction.FunctionName).ConfigureAwait(false);
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
}
