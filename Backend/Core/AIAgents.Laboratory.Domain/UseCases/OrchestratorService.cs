using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
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
    /// Get the orchestrator agent response asynchronously.
    /// </summary>
    /// <param name="chatRequest">The chat request domain model.</param>
    /// <param name="workspaceDetails">The workspace details.</param>
    /// <returns>The orchestrator agent response.</returns>
    public async Task<string> GetOrchestratorAgentResponseAsync(WorkspaceAgentChatRequestDomain chatRequest, AgentsWorkspaceDomain workspaceDetails)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetOrchestratorAgentResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(chatRequest));

            IList<AgentDataDomain> agentsData = [];
            await Parallel.ForEachAsync(workspaceDetails.ActiveAgentsListInWorkspace, async (agent, cancellationToken) =>
            {
                var agentData = await agentsService.GetAgentDataByIdAsync(agent.AgentGuid, string.Empty).ConfigureAwait(false);
                if (agentData is not null)
                    lock (agentsData)
                        agentsData.Add(agentData);
            }).ConfigureAwait(false);

            var agentsList = agentsData.Select(agent => $"{agent.AgentName}: {agent.AgentDescription}");
            var orchestratorSystemPrompt = SystemOrchestratorFunction.GetFunctionInstructions(string.Join("\n", agentsList));
            ConversationHistoryDomain conversationHistory = new();

            var loopCount = 0;
            while (loopCount < SystemOrchestratorFunction.MAX_ORCHESTRATOR_LOOPS)
            {
                loopCount++;

                // Call Orchestrator
                var orchestratorResponse = await aiServices.GetChatbotResponseAsync(
                    conversationDataDomain: conversationHistory,
                    userMessage: chatRequest.UserMessage,
                    agentMetaPrompt: orchestratorSystemPrompt).ConfigureAwait(false);

                // Add Orchestrator's own response to history to maintain context
                conversationHistory.ChatHistory.Add(new ChatHistoryDomain
                {
                    Role = ChatbotHelperConstants.AssistantRoleConstant,
                    Content = orchestratorResponse
                });

                // Parse Orchestrator Response
                var parsedResponse = JsonConvert.DeserializeObject<OrchestratorResponseDomain>(orchestratorResponse);
                if (parsedResponse?.Type == SystemOrchestratorFunction.OrchestratorResponseTypeFinalResponse)
                    return parsedResponse.Content;

                else if (parsedResponse?.Type == SystemOrchestratorFunction.OrchestratorResponseTypeDelegate)
                    await this.DelegateToAgentAsync(agentsData, chatRequest, conversationHistory, parsedResponse).ConfigureAwait(false);

                else
                    return ExceptionConstants.OrchestratorResponseFormatInvalidExceptionMessage;
            }

            return ExceptionConstants.OrchestratorLoopLimitReachedExceptionMessage;
        }
        catch (Exception ex)
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(GetOrchestratorAgentResponseAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetOrchestratorAgentResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(chatRequest));
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Delegates the task to an agent.
    /// </summary>
    /// <param name="agentsData">The agents data.</param>
    /// <param name="chatRequest">The chat request.</param>
    /// <param name="conversationHistory">The conversation history.</param>
    /// <param name="parsedResponse">The parsed response.</param>
    private async Task DelegateToAgentAsync(IList<AgentDataDomain> agentsData, WorkspaceAgentChatRequestDomain chatRequest,
        ConversationHistoryDomain conversationHistory, OrchestratorResponseDomain parsedResponse)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(DelegateToAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(chatRequest));

            var targetAgentName = parsedResponse.AgentName;
            var targetAgent = agentsData.FirstOrDefault(a => a.AgentName.Equals(targetAgentName, StringComparison.OrdinalIgnoreCase));

            if (targetAgent is null)
            {
                // Agent not found, let orchestrator know
                conversationHistory.ChatHistory.Add(new ChatHistoryDomain
                {
                    Role = ChatbotHelperConstants.UserRoleConstant,
                    Content = string.Format(ExceptionConstants.OrchestratorAgentNotAvailableExceptionMessage, targetAgentName)
                });
                return;
            }

            // Invoke the Agent
            var agentChatRequestModel = new ChatRequestDomain()
            {
                AgentId = targetAgent.AgentId,
                AgentName = targetAgent.AgentName,
                ConversationId = chatRequest.ConversationId,
                UserMessage = parsedResponse.Instruction,
            };
            var agentResponse = await agentChatService.GetAgentChatResponseAsync(agentChatRequestModel).ConfigureAwait(false);

            // Add Agent's response to history so Orchestrator can see it
            conversationHistory.ChatHistory.Add(new ChatHistoryDomain
            {
                Role = ChatbotHelperConstants.UserRoleConstant,
                Content = $"[{targetAgent.AgentName}]: {agentResponse}"
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DelegateToAgentAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(DelegateToAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(chatRequest));
        }
    }

    #endregion
}
