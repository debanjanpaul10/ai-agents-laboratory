using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.ApplicationPluginsHelpers;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The orchestrator service implementation.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="correlationContext">The correlation context for logging.</param>
/// <param name="agentsService">The agents service.</param>
/// <param name="agentChatService">The agents chat service.</param>
/// <param name="aiServices">The ai services.</param>
/// <seealso cref="IOrchestratorService"/>
public sealed class OrchestratorService(
    ILogger<OrchestratorService> logger,
    ICorrelationContext correlationContext,
    IAgentsService agentsService,
    IAgentChatService agentChatService,
    IAiServices aiServices) : IOrchestratorService
{
    /// <summary>
    /// Get the orchestrator agent response for each agent asynchronously.
    /// </summary>
    /// <param name="chatRequest">The chat request domain model.</param>
    /// <param name="workspaceDetails">The workspace details domain model.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>The final consolidated orchestrator response domain model.</returns>
    public async Task<OrchestratorFinalResponseDomain> GetOrchestratorAgentResponseAsync(
        WorkspaceAgentChatRequestDomain chatRequest,
        AgentsWorkspaceDomain workspaceDetails,
        CancellationToken cancellationToken = default
    )
    {
        OrchestratorFinalResponseDomain? response = null;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetOrchestratorAgentResponseAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, chatRequest })
            );

            var agentsData = await this.GetActiveAgentsDataAsync(
                workspaceDetails,
                cancellationToken
            ).ConfigureAwait(false);

            var orchestratorSystemPrompt = OrchestratorHelpers.GetOrchestratorSystemPrompt(agentsData);
            ConversationHistoryDomain conversationHistory = new();
            IList<string> agentsInvoked = [];
            List<GroupChatAgentsResponseDomain> groupChatAgentsResponses = [];

            var loopCount = 0;
            while (loopCount < SystemOrchestratorFunction.MAX_ORCHESTRATOR_LOOPS)
            {
                loopCount++;

                // Call Orchestrator
                var orchestratorResponse = await this.InvokeOrchestratorIterationAsync(
                    conversationHistory,
                    userMessage: chatRequest.UserMessage,
                    orchestratorSystemPrompt,
                    cancellationToken
                ).ConfigureAwait(false);

                // Parse Orchestrator Response
                var parsedResponse = OrchestratorHelpers.ParseOrchestratorResponse(orchestratorResponse);
                if (parsedResponse?.Type == SystemOrchestratorFunction.OrchestratorResponseTypeFinalResponse)
                {
                    response = OrchestratorHelpers.PrepareOrchestratorFinalResponse(
                        finalResponse: parsedResponse.Content,
                        groupChatResponses: groupChatAgentsResponses);
                    return response;
                }
                else if (parsedResponse?.Type == SystemOrchestratorFunction.OrchestratorResponseTypeDelegate)
                {
                    var agentResponse = await this.DelegateToAgentAsync(
                        agentsData,
                        chatRequest,
                        conversationHistory,
                        parsedResponse,
                        agentsInvoked,
                        cancellationToken
                    ).ConfigureAwait(false);

                    groupChatAgentsResponses.Add(new GroupChatAgentsResponseDomain
                    {
                        AgentName = parsedResponse.AgentName,
                        AgentResponse = agentResponse
                    });
                }
                else
                {
                    response = OrchestratorHelpers.PrepareOrchestratorFinalResponse(
                        finalResponse: ExceptionConstants.OrchestratorResponseFormatInvalidExceptionMessage,
                        groupChatResponses: groupChatAgentsResponses
                    );
                    return response;
                }
            }

            response = OrchestratorHelpers.PrepareOrchestratorFinalResponse(
                finalResponse: ExceptionConstants.OrchestratorLoopLimitReachedExceptionMessage,
                groupChatResponses: groupChatAgentsResponses
            );
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(GetOrchestratorAgentResponseAsync), DateTime.UtcNow, ex.Message
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
                nameof(GetOrchestratorAgentResponseAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, chatRequest })
            );
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Invokes the orchestrator iteration.
    /// </summary>
    /// <param name="conversationHistory">The conversation history.</param>
    /// <param name="userMessage">The user message.</param>
    /// <param name="orchestratorSystemPrompt">The orchestrator system prompt.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>The orchestrator response content.</returns>
    private async Task<string> InvokeOrchestratorIterationAsync(
        ConversationHistoryDomain conversationHistory,
        string userMessage,
        string orchestratorSystemPrompt,
        CancellationToken cancellationToken = default
    )
    {
        var orchestratorResponse = await aiServices.GetChatbotResponseAsync(
            conversationDataDomain: conversationHistory,
            userMessage,
            agentMetaPrompt: orchestratorSystemPrompt,
            cancellationToken
        ).ConfigureAwait(false);

        // Add Orchestrator's own response to history to maintain context
        conversationHistory.ChatHistory.Add(new ChatHistoryDomain
        {
            Role = ChatbotHelperConstants.AssistantRoleConstant,
            Content = orchestratorResponse
        });

        return orchestratorResponse;
    }

    /// <summary>
    /// Gets the active agents data in the workspace.
    /// </summary>
    /// <param name="workspaceDetails">The workspace details.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>The list of <see cref="AgentDataDomain"/></returns>
    private async Task<IList<AgentDataDomain>> GetActiveAgentsDataAsync(
        AgentsWorkspaceDomain workspaceDetails,
        CancellationToken cancellationToken = default
    )
    {
        IList<AgentDataDomain> agentsData = [];
        await Parallel.ForEachAsync(
            workspaceDetails.ActiveAgentsListInWorkspace, cancellationToken,
            async (agent, ct) =>
            {
                var agentData = await agentsService.GetAgentDataByIdAsync(
                    agentId: agent.AgentGuid,
                    userEmail: string.Empty,
                    cancellationToken
                ).ConfigureAwait(false);
                if (agentData is not null)
                {
                    lock (agentsData)
                    {
                        agentsData.Add(agentData);
                    }
                }
            }
        ).ConfigureAwait(false);

        return agentsData;
    }

    /// <summary>
    /// Delegates the task to an agent.
    /// </summary>
    /// <param name="agentsData">The agents data.</param>
    /// <param name="chatRequest">The chat request.</param>
    /// <param name="conversationHistory">The conversation history.</param>
    /// <param name="parsedResponse">The parsed response.</param>
    /// <param name="agentsInvoked">The list of agents invoked.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    private async Task<string> DelegateToAgentAsync(
        IList<AgentDataDomain> agentsData,
        WorkspaceAgentChatRequestDomain chatRequest,
        ConversationHistoryDomain conversationHistory,
        OrchestratorAgentResponseDomain parsedResponse,
        IList<string> agentsInvoked,
        CancellationToken cancellationToken = default
    )
    {
        string response = string.Empty;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(DelegateToAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, chatRequest })
            );

            var targetAgentName = parsedResponse.AgentName;
            var targetAgent = agentsData
                .FirstOrDefault(a => a.AgentName.Equals(targetAgentName, StringComparison.OrdinalIgnoreCase));
            if (targetAgent is null)
            {
                // Agent not found, let orchestrator know
                var errorMessage = string.Format(ExceptionConstants.OrchestratorAgentNotAvailableExceptionMessage, targetAgentName);
                conversationHistory.ChatHistory.Add(new ChatHistoryDomain
                {
                    Role = ChatbotHelperConstants.UserRoleConstant,
                    Content = errorMessage
                });
                response = errorMessage;
                return response;
            }

            // Track the invoked agent
            agentsInvoked.Add(targetAgentName);

            // Invoke the Agent
            var agentChatRequestModel = new ChatRequestDomain()
            {
                AgentId = targetAgent.AgentId,
                AgentName = targetAgentName,
                ConversationId = chatRequest.ConversationId,
                UserMessage = parsedResponse.Instruction,
            };
            var agentResponse = await agentChatService.GetAgentChatResponseAsync(
                chatRequest: agentChatRequestModel,
                cancellationToken
            ).ConfigureAwait(false);

            // Add Agent's response to history so Orchestrator can see it
            conversationHistory.ChatHistory.Add(new ChatHistoryDomain
            {
                Role = ChatbotHelperConstants.UserRoleConstant,
                Content = $"[{targetAgentName}]: {agentResponse}"
            });

            response = agentResponse;
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(DelegateToAgentAsync), DateTime.UtcNow, ex.Message
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
                nameof(DelegateToAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, response })
            );
        }
    }

    #endregion
}
