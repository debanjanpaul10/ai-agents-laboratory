namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The MCP Server Tools Domain.
/// </summary>
public sealed record McpServerToolsDomain
{
    /// <summary>
    /// The tool name.
    /// </summary>
    public string ToolName { get; set; } = string.Empty;

    /// <summary>
    /// The tool description.
    /// </summary>
    public string ToolDescription { get; set; } = string.Empty;
}
