using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.ApplicationPluginsHelpers;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The orchestrator service implementation.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="agentsService">The agents service.</param>
/// <param name="agentChatService">The agents chat service.</param>
/// <param name="aiServices">The ai services.</param>
/// <seealso cref="IOrchestratorService"/>
public sealed class OrchestratorService(ILogger<OrchestratorService> logger, IAgentsService agentsService, IAgentChatService agentChatService, IAiServices aiServices) : IOrchestratorService
{
    /// <summary>
    /// Get the orchestrator agent response for each agent asynchronously.
    /// </summary>
    /// <param name="chatRequest">The chat request domain model.</param>
    /// <returns>The final consolidated orchestrator response domain model.</returns>
    public async Task<OrchestratorFinalResponseDomain> GetOrchestratorAgentResponseAsync(WorkspaceAgentChatRequestDomain chatRequest, AgentsWorkspaceDomain workspaceDetails)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetOrchestratorAgentResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(chatRequest));

            var agentsData = await this.GetActiveAgentsDataAsync(workspaceDetails).ConfigureAwait(false);
            var orchestratorSystemPrompt = OrchestratorHelpers.GetOrchestratorSystemPrompt(agentsData);
            ConversationHistoryDomain conversationHistory = new();
            IList<string> agentsInvoked = [];
            List<GroupChatAgentsResponseDomain> groupChatAgentsResponses = [];

            var loopCount = 0;
            while (loopCount < SystemOrchestratorFunction.MAX_ORCHESTRATOR_LOOPS)
            {
                loopCount++;

                // Call Orchestrator
                var orchestratorResponse = await this.InvokeOrchestratorIterationAsync(conversationHistory, chatRequest.UserMessage, orchestratorSystemPrompt).ConfigureAwait(false);

                // Parse Orchestrator Response
                var parsedResponse = OrchestratorHelpers.ParseOrchestratorResponse(orchestratorResponse);
                if (parsedResponse?.Type == SystemOrchestratorFunction.OrchestratorResponseTypeFinalResponse)
                {
                    return OrchestratorHelpers.PrepareOrchestratorFinalResponse(
                        finalResponse: parsedResponse.Content,
                        groupChatResponses: groupChatAgentsResponses
                    );
                }
                else if (parsedResponse?.Type == SystemOrchestratorFunction.OrchestratorResponseTypeDelegate)
                {
                    var agentResponse = await this.DelegateToAgentAsync(agentsData, chatRequest, conversationHistory, parsedResponse, agentsInvoked).ConfigureAwait(false);
                    groupChatAgentsResponses.Add(new GroupChatAgentsResponseDomain
                    {
                        AgentName = parsedResponse.AgentName,
                        AgentResponse = agentResponse
                    });
                }
                else
                {
                    return OrchestratorHelpers.PrepareOrchestratorFinalResponse(
                        finalResponse: ExceptionConstants.OrchestratorResponseFormatInvalidExceptionMessage,
                        groupChatResponses: groupChatAgentsResponses);
                }
            }

            return OrchestratorHelpers.PrepareOrchestratorFinalResponse(
                finalResponse: ExceptionConstants.OrchestratorLoopLimitReachedExceptionMessage,
                groupChatResponses: groupChatAgentsResponses
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetOrchestratorAgentResponseAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetOrchestratorAgentResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(chatRequest));
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Invokes the orchestrator iteration.
    /// </summary>
    /// <param name="conversationHistory">The conversation history.</param>
    /// <param name="userMessage">The user message.</param>
    /// <param name="orchestratorSystemPrompt">The orchestrator system prompt.</param>
    /// <returns>The orchestrator response content.</returns>
    private async Task<string> InvokeOrchestratorIterationAsync(ConversationHistoryDomain conversationHistory, string userMessage, string orchestratorSystemPrompt)
    {
        var orchestratorResponse = await aiServices.GetChatbotResponseAsync(
            conversationDataDomain: conversationHistory,
            userMessage,
            agentMetaPrompt: orchestratorSystemPrompt).ConfigureAwait(false);

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
    /// <returns>The list of <see cref="AgentDataDomain"/></returns>
    private async Task<IList<AgentDataDomain>> GetActiveAgentsDataAsync(AgentsWorkspaceDomain workspaceDetails)
    {
        IList<AgentDataDomain> agentsData = [];
        await Parallel.ForEachAsync(workspaceDetails.ActiveAgentsListInWorkspace, async (agent, cancellationToken) =>
        {
            var agentData = await agentsService.GetAgentDataByIdAsync(agent.AgentGuid, string.Empty).ConfigureAwait(false);
            if (agentData is not null)
                lock (agentsData)
                    agentsData.Add(agentData);
        }).ConfigureAwait(false);

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
    private async Task<string> DelegateToAgentAsync(IList<AgentDataDomain> agentsData, WorkspaceAgentChatRequestDomain chatRequest, ConversationHistoryDomain conversationHistory,
        OrchestratorAgentResponseDomain parsedResponse, IList<string> agentsInvoked)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(DelegateToAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(chatRequest));

            var targetAgentName = parsedResponse.AgentName;
            var targetAgent = agentsData.FirstOrDefault(a => a.AgentName.Equals(targetAgentName, StringComparison.OrdinalIgnoreCase));
            if (targetAgent is null)
            {
                // Agent not found, let orchestrator know
                var errorMessage = string.Format(ExceptionConstants.OrchestratorAgentNotAvailableExceptionMessage, targetAgentName);
                conversationHistory.ChatHistory.Add(new ChatHistoryDomain
                {
                    Role = ChatbotHelperConstants.UserRoleConstant,
                    Content = errorMessage
                });
                return errorMessage;
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
            var agentResponse = await agentChatService.GetAgentChatResponseAsync(agentChatRequestModel).ConfigureAwait(false);

            // Add Agent's response to history so Orchestrator can see it
            conversationHistory.ChatHistory.Add(new ChatHistoryDomain
            {
                Role = ChatbotHelperConstants.UserRoleConstant,
                Content = $"[{targetAgentName}]: {agentResponse}"
            });

            return agentResponse;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DelegateToAgentAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(DelegateToAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(chatRequest));
        }
    }

    #endregion
}
