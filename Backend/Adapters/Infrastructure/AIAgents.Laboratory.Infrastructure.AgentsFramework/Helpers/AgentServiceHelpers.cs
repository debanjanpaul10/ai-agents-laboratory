using static AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers.Constants;

namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers;

/// <summary>
/// The agent service helpers.
/// </summary>
internal static class AgentServiceHelpers
{
    /// <summary>
    /// Sanitizes error messages to prevent sensitive information exposure.
    /// </summary>
    /// <param name="errorMessage">The original error message.</param>
    /// <returns>The sanitized error message.</returns>
    internal static string SanitizeErrorMessage(string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
            return ExceptionConstants.SomethingWentWrongMessage;

        // Keep a copy of the original message to detect if redactions were applied
        var original = errorMessage;

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

        // If any redaction occurred, avoid storing potentially sensitive content at all
        if (!string.Equals(original, sanitized, StringComparison.Ordinal))
            return ExceptionConstants.SomethingWentWrongMessage;

        // Enforce a maximum length to reduce risk of logging large, detailed messages
        const int maxLength = 256;
        if (sanitized.Length > maxLength)
            return string.Concat(sanitized.AsSpan(0, maxLength), "... (truncated)");

        return sanitized;
    }

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
}
