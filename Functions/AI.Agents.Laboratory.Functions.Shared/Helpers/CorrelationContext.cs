namespace AI.Agents.Laboratory.Functions.Shared.Helpers;

/// <summary>
/// Provides access to the correlation ID for the current request context across all application layers.
/// </summary>
public interface ICorrelationContext
{
    /// <summary>
    /// Gets the correlation ID for the current request.
    /// </summary>
    string CorrelationId { get; }
}

/// <summary>
/// Implementation of correlation context that retrieves the correlation ID from <see cref="LogContext"/>.
/// </summary>
public sealed class CorrelationContext : ICorrelationContext
{
    /// <summary>
    /// Gets the correlation ID for the current request from the <see cref="LogContext"/>. Returns an empty string if no correlation ID is set.
    /// </summary>
    public string CorrelationId => LogContext.CorrelationId ?? string.Empty;
}
