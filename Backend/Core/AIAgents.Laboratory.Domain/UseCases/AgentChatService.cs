using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Processor.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides services for managing agent chat interactions.
/// </summary>
/// <remarks>AgentChatService coordinates between agent data retrieval, knowledge base enrichment, and AI response
/// generation to deliver contextual chat replies. This service is typically used in conversational applications where agent-specific logic and knowledge integration are required.</remarks>
/// <param name="configuration">The configuration service.</param>
/// <param name="logger">The logger used to record diagnostic and operational information for the service.</param>
/// <param name="agentsService">The service used to retrieve agent data and metadata required for chat interactions.</param>
/// <param name="knowledgeBaseProcessor">The processor used to extract relevant knowledge base information to enhance agent responses.</param>
/// <param name="aiServices">The AI services used to generate responses based on chat context and agent configuration.</param>
/// <param name="toolSkillsService">The tool skills service used to manage and invoke external tools or skills associated with agents.</param>
/// <seealso cref="IAgentChatService"/>
public sealed class AgentChatService(IConfiguration configuration, ILogger<AgentChatService> logger, IAgentsService agentsService,
    IKnowledgeBaseProcessor knowledgeBaseProcessor, IAiServices aiServices, IToolSkillsService toolSkillsService) : IAgentChatService
{
    /// <summary>
    /// The feature flag for knowledge base service.
    /// </summary>
    private readonly bool IsKnowledgeBaseServiceAllowed = bool.TryParse(configuration[AzureAppConfigurationConstants.IsKnowledgeBaseServiceEnabledConstant], out var kbAllowed) && kbAllowed;

    /// <summary>
    /// The feature flag for AI vision service.
    /// </summary>
    private readonly bool IsAiVisionServiceAllowed = bool.TryParse(configuration[AzureAppConfigurationConstants.IsAiVisionServiceEnabledConstant], out var aivisionAllowed) && aivisionAllowed;

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
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAgentChatResponseAsync), DateTime.UtcNow, chatRequest.AgentId);

            var agentData = await agentsService.GetAgentDataByIdAsync(chatRequest.AgentId, string.Empty).ConfigureAwait(false);
            if (agentData is null || string.IsNullOrEmpty(agentData.AgentMetaPrompt))
            {
                var ex = new FileNotFoundException(ExceptionConstants.AgentNotFoundExceptionMessage);
                logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAgentChatResponseAsync), DateTime.UtcNow, ex.Message);
                throw ex;
            }

            var chatMessage = new ChatMessageDomain()
            {
                AgentMetaPrompt = agentData.AgentMetaPrompt,
                AgentName = agentData.AgentName,
                UserMessage = chatRequest.UserMessage,
            };

            // Get data from Knowledge Base if configured
            if (IsKnowledgeBaseServiceAllowed && agentData.HasKnowledgeBaseContent())
                chatMessage.KnowledgeBase = await knowledgeBaseProcessor.GetRelevantKnowledgeAsync(chatRequest.UserMessage, agentData.AgentId).ConfigureAwait(false);

            // Use AI Vision services if configured
            if (IsAiVisionServiceAllowed && agentData.AiVisionImagesData is not null && agentData.AiVisionImagesData.Any())
                chatMessage.ImageKeyWords = agentData.AiVisionImagesData;

            // Handle integrated skills if configured
            if (agentData.AssociatedSkillGuids is not null && agentData.AssociatedSkillGuids.Count > 0)
                return await this.GetResponseWithIntegratedSkillAsync(
                    chatMessage: chatMessage,
                    associatedSkillGuids: agentData.AssociatedSkillGuids).ConfigureAwait(false);
            else
                return await aiServices.GetAiFunctionResponseAsync(
                    input: chatMessage,
                    pluginName: ApplicationPluginsHelpers.PluginName,
                    functionName: ApplicationPluginsHelpers.GetChatMessageResponseFunction.FunctionName).ConfigureAwait(false);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAgentChatResponseAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAgentChatResponseAsync), DateTime.UtcNow, chatRequest.AgentId);
        }
    }

    /// <summary>
    /// Gets the response with integrated skill asynchronous.
    /// </summary>
    /// <param name="chatMessage">The chat message request domain model.</param>
    /// <param name="associatedSkillGuids">The associated skill guids list</param>
    /// <returns>The AI agent response string.</returns>
    private async Task<string> GetResponseWithIntegratedSkillAsync(ChatMessageDomain chatMessage, IList<string> associatedSkillGuids)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetResponseWithIntegratedSkillAsync), DateTime.UtcNow, chatMessage.AgentName);

            var associatedSkill = await toolSkillsService.GetToolSkillBySkillIdAsync(associatedSkillGuids[0], string.Empty).ConfigureAwait(false);
            ArgumentNullException.ThrowIfNull(associatedSkill);
            ArgumentException.ThrowIfNullOrWhiteSpace(associatedSkill.ToolSkillMcpServerUrl);

            return await aiServices.GetAiFunctionResponseAsync(
                input: chatMessage,
                mcpServerUrl: associatedSkill.ToolSkillMcpServerUrl,
                pluginName: ApplicationPluginsHelpers.PluginName,
                functionName: ApplicationPluginsHelpers.GetChatMessageResponseFunction.FunctionName).ConfigureAwait(false);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetResponseWithIntegratedSkillAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetResponseWithIntegratedSkillAsync), DateTime.UtcNow, chatMessage.AgentName);
        }
    }
}
