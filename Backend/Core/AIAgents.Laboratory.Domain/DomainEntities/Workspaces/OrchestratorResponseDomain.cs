namespace AIAgents.Laboratory.Domain.DomainEntities.Workspaces;

/// <summary>
/// The Orchestrator Response Domain Model.
/// </summary>
public sealed record OrchestratorResponseDomain
{
    /// <summary>
    /// The type of response from orchestrator.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// The name of the agent.
    /// </summary>
    public string AgentName { get; set; } = string.Empty;

    /// <summary>
    /// The instruction from orchestrator.
    /// </summary>
    public string Instruction { get; set; } = string.Empty;

    /// <summary>
    /// The content from orchestrator.
    /// </summary>
    public string Content { get; set; } = string.Empty;
}
