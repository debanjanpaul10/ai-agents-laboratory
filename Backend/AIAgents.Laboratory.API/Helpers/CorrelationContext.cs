using AIAgents.Laboratory.Domain.Contracts;
using static AIAgents.Laboratory.API.Helpers.Constants;

namespace AIAgents.Laboratory.API.Helpers;

/// <summary>
/// Implementation of correlation context that retrieves the correlation ID from HTTP context.
/// </summary>
/// <param name="httpContextAccessor">The HTTP context accessor.</param>
public sealed class CorrelationContext(IHttpContextAccessor httpContextAccessor) : ICorrelationContext
{
    /// <summary>
    /// Gets the correlation ID for the current request, extracted from the HTTP context items.
    /// </summary>
    public string CorrelationId => httpContextAccessor.HttpContext?.Items[LoggingConstants.CorrelationIdHeader]?.ToString() ?? string.Empty;
}
