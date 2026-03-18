using Microsoft.Extensions.Logging;

namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// Provides extension methods for the ILogger interface to simplify logging of informational, warning, and error messages with formatted arguments.
/// </summary>
/// <remarks>These extension methods enable concise and consistent logging at various log levels by allowing message templates with formatting arguments. 
/// Each method checks if the corresponding log level is enabled before writing the log entry. 
/// Use these methods to streamline application logging and ensure that log messages are only written when the appropriate log level is active.</remarks>
public static class LoggerExtensions
{
#pragma warning disable CA2254

    /// <summary>
    /// Writes an informational log entry using the specified logger, message, and formatting arguments.
    /// </summary>
    /// <remarks>This method logs the message only if the logger is enabled for the Information log level. 
    /// Placeholders in the message template are replaced with the corresponding values from the arguments array.</remarks>
    /// <param name="logger">The logger instance used to write the informational message. Cannot be null.</param>
    /// <param name="message">The message template to log. May include placeholders for formatting with the provided arguments.</param>
    /// <param name="args">An array of objects to format into the message template.</param>
    public static void LogAppInformation(this ILogger logger, string message, params object[] args)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(message, args);
    }

    /// <summary>
    /// Logs an error message and exception using the specified logger if error-level logging is enabled.
    /// </summary>
    /// <remarks>This method is an extension for ILogger that simplifies logging exceptions with a formatted message at the error level. 
    /// The log entry is only written if the logger is enabled for error-level events.</remarks>
    /// <param name="logger">The logger instance used to write the error log entry. Cannot be null.</param>
    /// <param name="exception">The exception to log. Provides details about the error that occurred. Cannot be null.</param>
    /// <param name="message">The message template describing the error. May include format placeholders for additional arguments.</param>
    /// <param name="args">An array of objects to format into the message template. Optional; can be empty.</param>
    public static void LogAppError(this ILogger logger, Exception exception, string message, params object[] args)
    {
        if (logger.IsEnabled(LogLevel.Error))
            logger.LogError(exception, message, args);
    }

    /// <summary>
    /// Writes a warning log entry using the specified logger, message, and formatting arguments.
    /// </summary>
    /// <remarks>This method only writes the log entry if the logger is enabled for the Warning level. 
    /// Use this extension method to simplify logging warnings with formatted messages.</remarks>
    /// <param name="logger">The logger instance used to write the warning message. Cannot be null.</param>
    /// <param name="message">The message template to log. May contain format placeholders for the arguments.</param>
    /// <param name="args">An array of objects to format into the message template.</param>
    public static void LogAppWarning(this ILogger logger, string message, params object[] args)
    {
        if (logger.IsEnabled(LogLevel.Warning))
            logger.LogWarning(message, args);
    }
}
