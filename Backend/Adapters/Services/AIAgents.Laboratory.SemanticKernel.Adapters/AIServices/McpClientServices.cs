using System.Text.Json;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.SemanticKernel.Adapters.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.AIServices;

/// <summary>
/// A service for retrieving MCP client tools and functions from a specified MCP server.
/// </summary>
/// <param name="configuration">The configuration.</param>
/// <param name="logger">The logger used to record diagnostic and operational information for the service.</param>
/// <seealso cref="IMcpClientServices"/>
public class McpClientServices(IConfiguration configuration, ILogger<McpClientServices> logger) : IMcpClientServices
{
    /// <summary>
    /// Asynchronously retrieves all available MCP client tools from the specified MCP server endpoint.
    /// </summary>
    /// <param name="mcpServerUrl">The URL of the MCP server endpoint from which to retrieve the list of client tools. Must be a valid, absolute URI.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
    /// cref="McpServerToolsDomain"/> objects representing the available client tools. The collection will be empty if no tools are found.</returns>
    public async Task<IEnumerable<McpClientTool>> GetAllMcpToolsAsync(string mcpServerUrl)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllMcpToolsAsync), DateTime.UtcNow);

            var mcpClient = await this.CreateMcpClientAsync(mcpServerUrl).ConfigureAwait(false);
            return await mcpClient.ListToolsAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(LoggingConstants.LogHelperMethodFailed, nameof(GetAllMcpToolsAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllMcpToolsAsync), DateTime.UtcNow);
        }
    }

    /// <summary>
    /// Gets the MCP tool response asynchronous.
    /// </summary>
    /// <param name="mcpServerUrl">The MCP server URL.</param>
    /// <param name="toolName">Name of the tool.</param>
    /// <param name="toolArguments">The tool arguments.</param>
    /// <returns>
    /// The MCP Tool Response.
    /// </returns>
    public async Task<string> GetMcpToolResponseAsync(string mcpServerUrl, string toolName, Dictionary<string, object?> toolArguments)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetMcpToolResponseAsync), DateTime.UtcNow);

            var arguments = toolArguments ?? [];
            var mcpClient = await this.CreateMcpClientAsync(mcpServerUrl).ConfigureAwait(false);
            var callToolResult = await mcpClient.CallToolAsync(toolName, arguments).ConfigureAwait(false);
            return JsonSerializer.Serialize(callToolResult);
        }
        catch (Exception ex)
        {
            logger.LogError(LoggingConstants.LogHelperMethodFailed, nameof(GetMcpToolResponseAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetMcpToolResponseAsync), DateTime.UtcNow);
        }
    }

    /// <summary>
    /// Creates the MCP client asynchronous.
    /// </summary>
    /// <param name="mcpServerUrl">The MCP server URL.</param>
    /// <returns>The MCP client.</returns>
    private async Task<McpClient> CreateMcpClientAsync(string mcpServerUrl)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateMcpClientAsync), DateTime.UtcNow);

            var aiAgentsToken = await TokenHelper.GetAiAgentsLabTokenAsync(configuration, logger).ConfigureAwait(false);
            var transportOptions = new HttpClientTransportOptions
            {
                Endpoint = new Uri(mcpServerUrl),
                AdditionalHeaders = new Dictionary<string, string>
                {
                    { ConfigurationConstants.AuthorizationConstant, string.Format(ConfigurationConstants.BearerTokenConstant, aiAgentsToken) }
                }
            };

            var httpClientTransport = new HttpClientTransport(transportOptions);
            return await McpClient.CreateAsync(httpClientTransport).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(LoggingConstants.LogHelperMethodFailed, nameof(CreateMcpClientAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(CreateMcpClientAsync), DateTime.UtcNow);
        }
    }
}
