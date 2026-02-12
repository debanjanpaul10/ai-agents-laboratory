using ModelContextProtocol.Client;

namespace AIAgents.Laboratory.Domain.Contracts;

/// <summary>
/// Defines a service for retrieving MCP client tools and functions from a specified MCP server.
/// </summary>
public interface IMcpClientServices
{
    /// <summary>
    /// Asynchronously retrieves all available MCP client tools from the specified MCP server endpoint.
    /// </summary>
    /// <param name="mcpServerUrl">The URL of the MCP server endpoint from which to retrieve the list of client tools. Must be a valid, absolute URI.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
    /// cref="McpClientTool"/> objects representing the available client tools. The collection will be empty if no tools are found.</returns>
    Task<IEnumerable<McpClientTool>> GetAllMcpToolsAsync(string mcpServerUrl);

    /// <summary>
    /// Gets the MCP tool response asynchronous.
    /// </summary>
    /// <param name="mcpServerUrl">The MCP server URL.</param>
    /// <param name="toolName">Name of the tool.</param>
    /// <param name="toolArguments">The tool arguments.</param>
    /// <returns>The MCP Tool Response.</returns>
    Task<string> GetMcpToolResponseAsync(string mcpServerUrl, string toolName, Dictionary<string, object?> toolArguments);
}
