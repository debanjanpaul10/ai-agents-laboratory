using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers.Constants;

namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.AgentServices;

/// <summary>
/// A service for retrieving MCP client tools and functions from a specified MCP server using Agents Framework.
/// </summary>
/// <param name="configuration">The configuration.</param>
/// <param name="logger">The logger used to record diagnostic and operational information for the service.</param>
/// <seealso cref="IMcpClientServices"/>
public sealed class McpAgentServices(IConfiguration configuration, ILogger<McpAgentServices> logger) : IMcpClientServices
{
    /// <summary>
    /// Asynchronously retrieves all available MCP client tools from the specified MCP server endpoint.
    /// </summary>
    /// <param name="mcpServerUrl">The URL of the MCP server endpoint from which to retrieve the list of client tools. Must be a valid, absolute URI.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
    /// cref="McpClientTool"/> objects representing the available client tools. The collection will be empty if no tools are found.</returns>
    public async Task<IEnumerable<McpClientTool>> GetAllMcpToolsAsync(string mcpServerUrl)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllMcpToolsAsync), DateTime.UtcNow, SanitizeForLogging(mcpServerUrl));

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
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllMcpToolsAsync), DateTime.UtcNow, SanitizeForLogging(mcpServerUrl));
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
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetMcpToolResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { mcpServerUrl, toolName, toolArguments }));

            var arguments = toolArguments ?? [];
            var mcpClient = await this.CreateMcpClientAsync(mcpServerUrl).ConfigureAwait(false);
            var callToolResult = await mcpClient.CallToolAsync(toolName, arguments).ConfigureAwait(false);
            return JsonConvert.SerializeObject(callToolResult);
        }
        catch (Exception ex)
        {
            logger.LogError(LoggingConstants.LogHelperMethodFailed, nameof(GetMcpToolResponseAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetMcpToolResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { mcpServerUrl, toolName, toolArguments }));
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Creates the MCP client asynchronous.
    /// </summary>
    /// <param name="mcpServerUrl">The MCP server URL.</param>
    /// <returns>The MCP client.</returns>
    private async Task<McpClient> CreateMcpClientAsync(string mcpServerUrl)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateMcpClientAsync), DateTime.UtcNow, SanitizeForLogging(mcpServerUrl));

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
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(CreateMcpClientAsync), DateTime.UtcNow, SanitizeForLogging(mcpServerUrl));
        }
    }

    /// <summary>
    /// Sanitizes a string value for safe logging by removing line breaks and control characters.
    /// </summary>
    /// <param name="value">The value to sanitize.</param>
    /// <returns>The sanitized value suitable for logging.</returns>
    private static string SanitizeForLogging(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        var withoutLineEndings = value.Replace("\r", string.Empty).Replace("\n", string.Empty);
        var sanitizedChars = withoutLineEndings.Where(c => !char.IsControl(c) || c == '\t');
        return new string(sanitizedChars.ToArray());
    }

    #endregion
}