namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The MCP Server Tools DTO.
/// </summary>
public sealed record McpServerToolsDTO
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
