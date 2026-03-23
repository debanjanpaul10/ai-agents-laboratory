using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Helpers;
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
/// <param name="correlationContext">The correlation context used to track and correlate logs and operations across different components and services during interactions with the MCP server.</param>
/// <seealso cref="IMcpClientServices"/>
public sealed class McpAgentServices(IConfiguration configuration, ILogger<McpAgentServices> logger, ICorrelationContext correlationContext) : IMcpClientServices
{
    /// <summary>
    /// Asynchronously retrieves all available MCP client tools from the specified MCP server endpoint.
    /// </summary>
    /// <param name="mcpServerUrl">The URL of the MCP server endpoint from which to retrieve the list of client tools. Must be a valid, absolute URI.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
    /// cref="McpClientTool"/> objects representing the available client tools. The collection will be empty if no tools are found.</returns>
    public async Task<IEnumerable<McpClientTool>> GetAllMcpToolsAsync(string mcpServerUrl, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllMcpToolsAsync), DateTime.UtcNow, SanitizeForLogging(mcpServerUrl));

            var mcpClient = await this.CreateMcpClientAsync(mcpServerUrl).ConfigureAwait(false);
            return await mcpClient.ListToolsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllMcpToolsAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllMcpToolsAsync), DateTime.UtcNow, SanitizeForLogging(mcpServerUrl));
        }
    }

    /// <summary>
    /// Gets the MCP tool response asynchronous.
    /// </summary>
    /// <param name="mcpServerUrl">The MCP server URL.</param>
    /// <param name="toolName">Name of the tool.</param>
    /// <param name="toolArguments">The tool arguments.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// The MCP Tool Response.
    /// </returns>
    public async Task<string> GetMcpToolResponseAsync(string mcpServerUrl, string toolName, Dictionary<string, object?> toolArguments, CancellationToken cancellationToken = default)
    {
        string response = string.Empty;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetMcpToolResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, mcpServerUrl, toolName, toolArguments }));

            var arguments = toolArguments ?? [];
            var mcpClient = await this.CreateMcpClientAsync(mcpServerUrl, cancellationToken).ConfigureAwait(false);
            var callToolResult = await mcpClient.CallToolAsync(toolName, arguments, cancellationToken: cancellationToken).ConfigureAwait(false);

            response = JsonConvert.SerializeObject(callToolResult);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetMcpToolResponseAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetMcpToolResponseAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, mcpServerUrl, toolName, toolArguments, response }));
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Creates the MCP client asynchronous.
    /// </summary>
    /// <param name="mcpServerUrl">The MCP server URL.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The MCP client.</returns>
    private async Task<McpClient> CreateMcpClientAsync(string mcpServerUrl, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateMcpClientAsync), DateTime.UtcNow, SanitizeForLogging(mcpServerUrl));

            var aiAgentsToken = await TokenHelper.GetAiAgentsLabTokenAsync(
                correlationId: correlationContext.CorrelationId, configuration, logger).ConfigureAwait(false);
            var transportOptions = new HttpClientTransportOptions
            {
                Endpoint = new Uri(mcpServerUrl),
                AdditionalHeaders = new Dictionary<string, string>
                {
                    { ConfigurationConstants.AuthorizationConstant, string.Format(ConfigurationConstants.BearerTokenConstant, aiAgentsToken) }
                }
            };

            var httpClientTransport = new HttpClientTransport(transportOptions);
            return await McpClient.CreateAsync(clientTransport: httpClientTransport, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(CreateMcpClientAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(CreateMcpClientAsync), DateTime.UtcNow, SanitizeForLogging(mcpServerUrl));
        }
    }

    /// <summary>
    /// Sanitizes a string value for safe logging by removing line breaks and control characters.
    /// </summary>
    /// <param name="value">The value to sanitize.</param>
    /// <returns>The sanitized value suitable for logging.</returns>
    private static string SanitizeForLogging(string value)
    {
        if (string.IsNullOrEmpty(value)) return value;

        var withoutLineEndings = value.Replace("\r", string.Empty).Replace("\n", string.Empty);
        var sanitizedChars = withoutLineEndings.Where(c => !char.IsControl(c) || c == '\t');
        return new string([.. sanitizedChars]);
    }

    #endregion
}