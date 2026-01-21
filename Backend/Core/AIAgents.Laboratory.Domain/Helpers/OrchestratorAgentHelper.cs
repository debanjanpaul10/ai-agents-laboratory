using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.ApplicationPluginsHelpers;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The orchestrator agent helper class.
/// </summary>
internal static class OrchestratorAgentHelper
{
    /// <summary>
    /// Generates the orchestrator system prompt.
    /// </summary>
    /// <param name="agentsData">The agents data.</param>
    /// <returns>The system prompt string.</returns>
    public static string GenerateOrchestratorSystemPrompt(IList<AgentDataDomain> agentsData)
    {
        var agentsList = agentsData.Select(agent => $"{agent.AgentName}: {agent.AgentDescription}");
        return SystemOrchestratorFunction.GetFunctionInstructions(string.Join("\n", agentsList));
    }

    /// <summary>
    /// Initializes the orchestrator conversation.
    /// </summary>
    /// <param name="chatRequest">The chat request.</param>
    /// <returns>The conversation history domain.</returns>
    public static ConversationHistoryDomain InitializeOrchestratorConversation(WorkspaceAgentChatRequestDomain chatRequest)
    {
        var conversationHistory = new ConversationHistoryDomain
        {
            ConversationId = chatRequest.ConversationId,
            IsActive = true,
            UserName = chatRequest.ApplicationName,
            ChatHistory = []
        };

        // Add the initial User message to history
        conversationHistory.ChatHistory.Add(new ChatHistoryDomain
        {
            Role = ChatbotHelperConstants.UserRoleConstant,
            Content = chatRequest.UserMessage
        });

        return conversationHistory;
    }

    /// <summary>
    /// Parses the orchestrator response.
    /// </summary>
    /// <param name="orchestratorResponse">The orchestrator response.</param>
    /// <returns>The orchestrator response domain.</returns>
    public static OrchestratorResponseDomain? ParseOrchestratorResponse(string orchestratorResponse) => JsonConvert.DeserializeObject<OrchestratorResponseDomain>(orchestratorResponse);
}
