namespace AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

/// <summary>
/// Represents the result of a tool selection process within an AI agent.
/// </summary>
public class ToolSelectionResultDomain
{
    /// <summary>
    /// Gets or sets the name of the tool.
    /// </summary>
    /// <value>
    /// The name of the tool.
    /// </value>
    public string ToolName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tool arguments.
    /// </summary>
    /// <value>
    /// The tool arguments.
    /// </value>
    public Dictionary<string, object?> ToolArguments { get; set; } = [];
}
