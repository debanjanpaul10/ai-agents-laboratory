using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;

namespace AIAgents.Laboratory.Domain.Contracts;

/// <summary>
/// The orchestrator service interface.
/// </summary>
public interface IOrchestratorService
{
    /// <summary>
    /// Get the orchestrator agent response asynchronously.
    /// </summary>
    /// <param name="chatRequest">The chat request domain model.</param>
    /// <returns>The orchestrator agent response.</returns>
    Task<OrchestratorFinalResponseDomain> GetOrchestratorAgentResponseAsync(WorkspaceAgentChatRequestDomain chatRequest, AgentsWorkspaceDomain workspaceDetails);
}
