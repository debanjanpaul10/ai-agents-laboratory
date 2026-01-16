namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The MCP Server Tool Request DTO.
/// </summary>
public sealed record McpServerToolRequestDTO
{
    /// <summary>
    /// The server URL.
    /// </summary>
    public string ServerUrl { get; set; } = string.Empty;
}
