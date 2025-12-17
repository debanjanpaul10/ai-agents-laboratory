using Microsoft.AspNetCore.Http;

namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The Create Agent Data DTO model.
/// </summary>
public record CreateAgentDTO
{
    /// <summary>
    /// Gets or sets the name of the agent.
    /// </summary>
    /// <value>
    /// The name of the agent.
    /// </value>
    public string AgentName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the agent meta prompt.
    /// </summary>
    /// <value>
    /// The agent meta prompt.
    /// </value>
    public string AgentMetaPrompt { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the application.
    /// </summary>
    /// <value>
    /// The name of the application.
    /// </value>
    public string ApplicationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the knowledge base document.
    /// </summary>
    /// <value>
    /// The knowledge base document.
    /// </value>
    public IEnumerable<IFormFile>? KnowledgeBaseDocument { get; set; } = [];

    /// <summary>
    /// Gets or sets the is private boolean flag.
    /// </summary>
    /// <value>
    /// The boolean flag for private agent.
    /// </value>
    public bool IsPrivate { get; set; } = false;

    /// <summary>
    /// Gets or sets the URL of the MCP server used for network communication.
    /// </summary>
    /// <value>
    /// The MCP server URL.
    /// </value>
    public string? McpServerUrl { get; set; }
}
