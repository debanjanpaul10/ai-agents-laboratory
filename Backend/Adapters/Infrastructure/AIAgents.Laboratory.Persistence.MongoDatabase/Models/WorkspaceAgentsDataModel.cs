namespace AIAgents.Laboratory.Persistence.MongoDatabase.Models;

/// <summary>
/// The Workspace agents data model.
/// </summary>
public sealed record WorkspaceAgentsDataModel
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
