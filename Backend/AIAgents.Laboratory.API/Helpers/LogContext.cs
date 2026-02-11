namespace AIAgents.Laboratory.API.Helpers;

/// <summary>
/// Provides a thread-safe context for storing correlation ID and other request-scoped logging data.
/// This class uses AsyncLocal to maintain context across async operations.
/// </summary>
public static class LogContext
{
    /// <summary>
    /// The correlation ID for the current async context.
    /// </summary>
    private static readonly AsyncLocal<string?> _correlationId = new();

    /// <summary>
    /// The properties dictionary for the current async context.
    /// </summary>
    private static readonly AsyncLocal<Dictionary<string, object>?> _properties = new();

    /// <summary>
    /// Gets or sets the correlation ID for the current async context.
    /// </summary>
    public static string? CorrelationId
    {
        get => _correlationId.Value;
        set => _correlationId.Value = value;
    }

    /// <summary>
    /// Gets the properties dictionary for the current async context.
    /// </summary>
    public static Dictionary<string, object> Properties
    {
        get
        {
            _properties.Value ??= [];
            return _properties.Value;
        }
    }

    /// <summary>
    /// Adds a property to the current logging context.
    /// </summary>
    /// <param name="key">The property key.</param>
    /// <param name="value">The property value.</param>
    public static void AddProperty(string key, object value)
    {
        Properties[key] = value;
    }

    /// <summary>
    /// Gets a property from the current logging context.
    /// </summary>
    /// <param name="key">The property key.</param>
    /// <returns>The property value, or null if not found.</returns>
    public static object? GetProperty(string key) => Properties.TryGetValue(key, out var value) ? value : null;

    /// <summary>
    /// Clears all context data. Should be called at the end of a request.
    /// </summary>
    public static void Clear()
    {
        _correlationId.Value = null;
        _properties.Value?.Clear();
        _properties.Value = null;
    }
}
