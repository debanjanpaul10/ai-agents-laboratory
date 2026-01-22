using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.Extensions.AI;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers.Constants;

namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers;

/// <summary>
/// The agent service helpers.
/// </summary>
internal static class AgentServiceHelpers
{
    /// <summary>
    /// Deserializes tool selection result with proper error handling.
    /// </summary>
    /// <param name="toolSelectionResultResponse">The tool selection result response.</param>
    /// <returns>The deserialized tool selection result.</returns>
    /// <exception cref="InvalidOperationException">Thrown when deserialization fails.</exception>
    internal static ToolSelectionResultDomain DeserializeToolSelectionResult(string toolSelectionResultResponse)
    {
        var extractedJson = DomainUtilities.ExtractJsonFromMarkdown(toolSelectionResultResponse);
        return JsonConvert.DeserializeObject<ToolSelectionResultDomain>(extractedJson) ?? throw new InvalidOperationException(ExceptionConstants.DefaultAIExceptionMessage);
    }

    /// <summary>
    /// Sanitizes error messages to prevent sensitive information exposure.
    /// </summary>
    /// <param name="errorMessage">The original error message.</param>
    /// <returns>The sanitized error message.</returns>
    internal static string SanitizeErrorMessage(string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
            return ExceptionConstants.SomethingWentWrongMessage;

        // Remove potential API keys, tokens, or other sensitive information
        var sanitized = errorMessage;

        // Remove patterns that look like API keys (alphanumeric strings of certain lengths)
        sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, @"\b[A-Za-z0-9]{20,}\b", "[REDACTED]");

        // Remove Bearer tokens
        sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, @"Bearer\s+[A-Za-z0-9\-._~+/]+=*", "Bearer [REDACTED]", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove URLs with credentials
        sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, @"https?://[^:]+:[^@]+@[^\s]+", "https://[REDACTED]@[REDACTED]");

        // Remove connection strings
        sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, @"password\s*=\s*[^;]+", "password=[REDACTED]", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        sanitized = System.Text.RegularExpressions.Regex.Replace(sanitized, @"pwd\s*=\s*[^;]+", "pwd=[REDACTED]", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        return sanitized;
    }

    /// <summary>
    /// Sanitizes service provider information for logging.
    /// </summary>
    /// <param name="serviceProvider">The service provider name.</param>
    /// <returns>The sanitized service provider name.</returns>
    internal static string SanitizeServiceProvider(string? serviceProvider) => string.IsNullOrWhiteSpace(serviceProvider) ? "[UNKNOWN]" : serviceProvider;

    /// <summary>
    /// Validates input parameters for AI function calls.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input to validate.</param>
    /// <param name="pluginName">The plugin name to validate.</param>
    /// <param name="functionName">The function name to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when any parameter is null or empty.</exception>
    internal static void ValidateInputParameters<TInput>(TInput input, string pluginName, string functionName)
    {
        if (input is null)
            throw new ArgumentNullException(nameof(input), ExceptionConstants.SomethingWentWrongMessage);

        if (string.IsNullOrWhiteSpace(pluginName))
            throw new ArgumentNullException(nameof(pluginName), ExceptionConstants.SomethingWentWrongMessage);

        if (string.IsNullOrWhiteSpace(functionName))
            throw new ArgumentNullException(nameof(functionName), ExceptionConstants.SomethingWentWrongMessage);
    }

    /// <summary>
    /// Validates the MCP server URL parameter.
    /// </summary>
    /// <param name="mcpServerUrl">The MCP server URL to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the MCP server URL is null or empty.</exception>
    internal static void ValidateMcpServerUrl(string mcpServerUrl)
    {
        if (string.IsNullOrWhiteSpace(mcpServerUrl))
            throw new ArgumentNullException(nameof(mcpServerUrl), ExceptionConstants.SomethingWentWrongMessage);
    }

    /// <summary>
    /// Builds chat messages from conversation history and current user message.
    /// </summary>
    /// <param name="conversationDataDomain">The conversation history data domain.</param>
    /// <param name="userMessage">The current user message.</param>
    /// <param name="agentPrompt">The agent prompt.</param>
    /// <returns>A list of chat messages.</returns>
    internal static List<Models.ChatMessage> BuildChatMessages(ConversationHistoryDomain conversationDataDomain, string userMessage, string agentPrompt)
    {
        var chatMessages = new List<Models.ChatMessage>
        {
            new() { Role = ChatRole.System.ToString().ToLowerInvariant(), Content = agentPrompt }
        };

        // Add conversation history if available
        if (conversationDataDomain.ChatHistory is not null)
            foreach (var message in conversationDataDomain.ChatHistory)
                chatMessages.Add(new Models.ChatMessage { Role = message.Role, Content = message.Content });

        // Add the current user message
        chatMessages.Add(new Models.ChatMessage { Role = ArgumentsConstants.UserRoleConstant, Content = userMessage });
        return chatMessages;
    }

    /// <summary>
    /// Converts string role to ChatRole enum.
    /// </summary>
    /// <param name="role">The current chat role.</param>
    /// <returns>The chat role.</returns>
    internal static ChatRole ConvertToChatRole(string role) =>
        role.ToLowerInvariant() switch
        {
            "user" => ChatRole.User,
            "assistant" => ChatRole.Assistant,
            "system" => ChatRole.System,
            _ => ChatRole.User
        };
}
