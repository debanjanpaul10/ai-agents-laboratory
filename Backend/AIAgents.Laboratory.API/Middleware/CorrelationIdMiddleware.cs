using Microsoft.Extensions.Primitives;
using static AIAgents.Laboratory.API.Helpers.Constants.LoggingConstants;

namespace AIAgents.Laboratory.API.Middleware;

/// <summary>
/// Middleware to handle correlation IDs for incoming HTTP requests.
/// </summary>
/// <param name="next">The next RequestDelegate in the pipeline.</param>
/// <param name="logger">The ILogger instance for logging.</param>
public sealed class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    /// <summary>
    /// Invokes the middleware to process the HTTP context.
    /// </summary>
    /// <param name="httpContext">The HttpContext to process.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        // Extract or generate correlation ID
        var correlationId = GetOrCreateCorrelationId(httpContext);

        // Add to response headers
        httpContext.Response.Headers.Append(CorrelationIdHeader, correlationId);

        // Store in HttpContext for access by other components
        httpContext.Items[CorrelationIdHeader] = correlationId;

        // Store in custom LogContext for async-safe access
        Helpers.LogContext.CorrelationId = correlationId;

        // Use Microsoft's ILogger.BeginScope to enrich ALL logs in this scope
        try
        {
            using (logger.BeginScope(new Dictionary<string, object>
            {
                [HeaderLoggingConstants.CorrelationId] = correlationId,
                [HeaderLoggingConstants.RequestPath] = httpContext.Request.Path.ToString(),
                [HeaderLoggingConstants.RequestMethod] = httpContext.Request.Method
            }))
            {
                await next(httpContext);
            }
        }
        finally
        {
            Helpers.LogContext.Clear();
            httpContext.Items.Remove(CorrelationIdHeader);
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Gets or creates a correlation ID from the HTTP context.
    /// </summary>
    /// <param name="httpContext">The HttpContext to extract or generate the correlation ID from.</param>
    /// <returns>The correlation ID.</returns>
    private static string GetOrCreateCorrelationId(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue(CorrelationIdHeader, out StringValues correlationId) && !string.IsNullOrWhiteSpace(correlationId))
            return correlationId.ToString();

        return Guid.NewGuid().ToString();
    }

    #endregion
}

/// <summary>
/// The extension methods for CorrelationIdMiddleware.
/// </summary>
public static class CorrelationIdMiddlewareExtensions
{
    /// <summary>
    /// Adds the CorrelationIdMiddleware to the application's request pipeline.
    /// </summary>
    /// <param name="builder">The IApplicationBuilder to which the middleware is added.</param>
    /// <returns>The modified IApplicationBuilder.</returns>
    public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<CorrelationIdMiddleware>();
}