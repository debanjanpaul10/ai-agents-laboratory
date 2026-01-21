namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The Workspace agents data domain model.
/// </summary>
public sealed record WorkspaceAgentsDataDomain
{
    /// <summary>
    /// Gets or sets the agent name.
    /// </summary>
    /// <value>The agent name.</value>
    public string AgentName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the agent guid.
    /// </summary>
    /// <value>The agent guid.</value>
    public string AgentGuid { get; set; } = string.Empty;
}
