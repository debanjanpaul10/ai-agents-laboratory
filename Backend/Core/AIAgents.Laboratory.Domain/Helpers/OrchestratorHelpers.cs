using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The orchestrator helpers class.
/// </summary>
internal static class OrchestratorHelpers
{
    /// <summary>
    /// Prepares the group chat response domain.
    /// </summary>
    /// <param name="agentResponse">The agent response</param>
    /// <param name="listOfAgentsInvoked">The list of agents invoked for this operation.</param>
    /// <returns>The group chat response domain model.</returns>
    internal static GroupChatResponseDomain PrepareGroupChatResponseDomain(string agentResponse, IEnumerable<string> listOfAgentsInvoked) =>
        new()
        {
            AgentResponse = agentResponse,
            AgentsInvoked = [.. listOfAgentsInvoked]
        };

}
