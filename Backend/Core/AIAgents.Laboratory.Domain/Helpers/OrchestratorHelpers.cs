using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.ApplicationPluginsHelpers;

namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The orchestrator helpers class.
/// </summary>
internal static class OrchestratorHelpers
{
    /// <summary>
    /// Prepares the orchestrator final consolidated response.
    /// </summary>
    /// <param name="finalResponse">The final response.</param>
    /// <param name="groupChatResponses">The group chat response.</param>
    /// <returns>The group chat response domain model.</returns>
    internal static OrchestratorFinalResponseDomain PrepareOrchestratorFinalResponse(string finalResponse, List<GroupChatAgentsResponseDomain> groupChatResponses) =>
        new()
        {
            FinalResponse = finalResponse,
            GroupChatAgentsResponses = groupChatResponses
        };

    /// <summary>
    /// Parses the orchestrator response.
    /// </summary>
    /// <param name="orchestratorResponse">The orchestrator response string.</param>
    /// <returns>The <see cref="OrchestratorAgentResponseDomain"/></returns>
    internal static OrchestratorAgentResponseDomain? ParseOrchestratorResponse(string orchestratorResponse) => JsonConvert.DeserializeObject<OrchestratorAgentResponseDomain>(orchestratorResponse);

    /// <summary>
    /// Gets the orchestrator system prompt.
    /// </summary>
    /// <param name="agentsData">The agents data.</param>
    /// <returns>The orchestrator system prompt.</returns>
    internal static string GetOrchestratorSystemPrompt(IList<AgentDataDomain> agentsData)
    {
        var agentsList = agentsData.Select(agent => $"{agent.AgentName}: {agent.AgentDescription}");
        return SystemOrchestratorFunction.GetFunctionInstructions(string.Join("\n", agentsList));
    }

}
