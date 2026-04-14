using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides services for managing agent chat interactions.
/// </summary>
/// <remarks>AgentChatService coordinates between agent data retrieval, knowledge base enrichment, and AI response
/// generation to deliver contextual chat replies. This service is typically used in conversational applications where agent-specific logic and knowledge integration are required.</remarks>
/// <param name="configuration">The configuration service.</param>
/// <param name="logger">The logger used to record diagnostic and operational information for the service.</param>
/// <param name="correlationContext">The correlation context used to track and correlate logs and operations across different components and services during a chat interaction.</param>
/// <param name="agentsService">The service used to retrieve agent data and metadata required for chat interactions.</param>
/// <param name="knowledgeBaseProcessor">The processor used to extract relevant knowledge base information to enhance agent responses.</param>
/// <param name="aiServices">The AI services used to generate responses based on chat context and agent configuration.</param>
/// <param name="toolSkillsService">The tool skills service used to manage and invoke external tools or skills associated with agents.</param>
/// <seealso cref="IAgentChatService"/>
public sealed class AgentChatService(
    IConfiguration configuration,
    ILogger<AgentChatService> logger,
    ICorrelationContext correlationContext,
    IAgentsService agentsService,
    IKnowledgeBaseProcessor knowledgeBaseProcessor,
    IAiServices aiServices,
    IToolSkillsService toolSkillsService) : IAgentChatService
{
    /// <summary>
    /// The feature flag for knowledge base service.
    /// </summary>
    private readonly string IsKnowledgeBaseServiceAllowed = configuration[AzureAppConfigurationConstants.IsKnowledgeBaseServiceEnabledConstant]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The feature flag for AI vision service.
    /// </summary>
    private readonly string IsAiVisionServiceAllowed = configuration[AzureAppConfigurationConstants.IsAiVisionServiceEnabledConstant]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// Gets the agent chat response asynchronous.
    /// </summary>
    /// <param name="chatRequest">The chat request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The AI response.</returns>
    public async Task<string> GetAgentChatResponseAsync(
        ChatRequestDomain chatRequest,
        CancellationToken cancellationToken = default
    )
    {
        string response = string.Empty;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetAgentChatResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, chatRequest })
            );

            var agentData = await agentsService.GetAgentDataByIdAsync(
                agentId: chatRequest.AgentId,
                userEmail: string.Empty,
                cancellationToken
            ).ConfigureAwait(false);
            if (agentData is null || string.IsNullOrEmpty(agentData.AgentMetaPrompt))
            {
                var ex = new FileNotFoundException(ExceptionConstants.AgentNotFoundExceptionMessage);
                logger.LogAppError(
                    ex,
                    LoggingConstants.LogHelperMethodFailed,
                    nameof(GetAgentChatResponseAsync), DateTime.UtcNow, ex.Message
                );
                throw ex;
            }

            var chatMessage = new ChatMessageDomain()
            {
                AgentMetaPrompt = agentData.AgentMetaPrompt,
                AgentName = agentData.AgentName,
                UserMessage = chatRequest.UserMessage,
            };

            // Get data from Knowledge Base if configured
            var isKnowledgeBaseServiceAllowedFlag = bool.TryParse(IsKnowledgeBaseServiceAllowed, out var isKbAllowed) && isKbAllowed;
            if (isKnowledgeBaseServiceAllowedFlag && agentData.HasKnowledgeBaseContent())
                chatMessage.KnowledgeBase = await knowledgeBaseProcessor.GetRelevantKnowledgeAsync(
                    query: chatRequest.UserMessage,
                    agentId: agentData.AgentId,
                    cancellationToken
                ).ConfigureAwait(false);

            // Use AI Vision services if configured
            bool isAiVisionServiceAllowedFlag = bool.TryParse(IsAiVisionServiceAllowed, out var isVisionAllowed) && isVisionAllowed;
            if (isAiVisionServiceAllowedFlag && agentData.AiVisionImagesData is not null && agentData.AiVisionImagesData.Any())
                chatMessage.ImageKeyWords = agentData.AiVisionImagesData;

            // Handle integrated skills if configured
            if (agentData.AssociatedSkillGuids is not null && agentData.AssociatedSkillGuids.Count > 0)
                response = await this.GetResponseWithIntegratedSkillAsync(
                    chatMessage: chatMessage,
                    associatedSkillGuids: agentData.AssociatedSkillGuids,
                    cancellationToken
                ).ConfigureAwait(false);
            else
                response = await aiServices.GetAiFunctionResponseAsync(
                    input: chatMessage,
                    pluginName: ApplicationPluginsHelpers.PluginName,
                    functionName: ApplicationPluginsHelpers.GetChatMessageResponseFunction.FunctionName,
                    cancellationToken
                ).ConfigureAwait(false);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(GetAgentChatResponseAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(GetAgentChatResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, response })
            );
        }
    }

    /// <summary>
    /// Gets the response with integrated skill asynchronous.
    /// </summary>
    /// <param name="chatMessage">The chat message request domain model.</param>
    /// <param name="associatedSkillGuids">The associated skill guids list</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The AI agent response string.</returns>
    private async Task<string> GetResponseWithIntegratedSkillAsync(
        ChatMessageDomain chatMessage,
        IList<string> associatedSkillGuids,
        CancellationToken cancellationToken = default
    )
    {
        string response = string.Empty;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetResponseWithIntegratedSkillAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, chatMessage })
            );

            var associatedSkill = await toolSkillsService.GetToolSkillBySkillIdAsync(
                toolSkillId: associatedSkillGuids[0],
                currentUserEmail: string.Empty,
                cancellationToken
            ).ConfigureAwait(false);

            ArgumentNullException.ThrowIfNull(associatedSkill);
            ArgumentException.ThrowIfNullOrWhiteSpace(associatedSkill.ToolSkillMcpServerUrl);

            response = await aiServices.GetAiFunctionResponseAsync(
                input: chatMessage,
                mcpServerUrl: associatedSkill.ToolSkillMcpServerUrl,
                pluginName: ApplicationPluginsHelpers.PluginName,
                functionName: ApplicationPluginsHelpers.GetChatMessageResponseFunction.FunctionName,
                cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(GetResponseWithIntegratedSkillAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(GetResponseWithIntegratedSkillAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, response })
            );
        }
    }
}
