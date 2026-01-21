namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The workspace agents data DTO.
/// </summary>
public sealed record WorkspaceAgentsDataDTO
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
