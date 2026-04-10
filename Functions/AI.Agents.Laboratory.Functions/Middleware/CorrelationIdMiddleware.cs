using AI.Agents.Laboratory.Functions.Shared.Constants;
using AI.Agents.Laboratory.Functions.Shared.Helpers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace AI.Agents.Laboratory.Functions.Middleware;

/// <summary>
/// Middleware to handle correlation IDs for incoming HTTP requests.
/// </summary>
public sealed class CorrelationIdMiddleware(ILogger<CorrelationIdMiddleware> logger) : IFunctionsWorkerMiddleware
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CorrelationIdMiddleware"/> class.
    /// </summary>
    private readonly ILogger<CorrelationIdMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Invokes the middleware to extract or generate a correlation ID, store it in the context for logging, and ensure it's included in the response headers for HTTP triggers.
    /// </summary>
    /// <param name="context">The function execution context.</param>
    /// <param name="next">The next middleware delegate in the pipeline.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Invoke(
        FunctionContext context,
        FunctionExecutionDelegate next
    )
    {
        var httpRequest = await context.GetHttpRequestDataAsync().ConfigureAwait(false);
        var correlationId = GetOrCreateCorrelationId(httpRequest);

        context.Items[LoggerConstants.CorrelationIdHeader] = correlationId;

        // Store in custom LogContext for async-safe access
        LogContext.CorrelationId = correlationId;

        try
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                [LoggerConstants.HeaderLoggingConstants.CorrelationId] = correlationId,
                [LoggerConstants.HeaderLoggingConstants.RequestPath] = httpRequest?.Url?.AbsolutePath ?? string.Empty,
                [LoggerConstants.HeaderLoggingConstants.RequestMethod] = httpRequest?.Method ?? string.Empty
            }))
            {
                await next(context).ConfigureAwait(false);
            }

            // For HTTP triggers, add correlation id to the response (without overwriting an existing one).
            var invocationResult = context.GetInvocationResult();
            if (invocationResult.Value is HttpResponseData httpResponse && !httpResponse.Headers.TryGetValues(LoggerConstants.CorrelationIdHeader, out _))
                httpResponse.Headers.Add(LoggerConstants.CorrelationIdHeader, correlationId);
        }
        finally
        {
            LogContext.Clear();
            context.Items.Remove(LoggerConstants.CorrelationIdHeader);
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Retrieves the correlation ID from the incoming HTTP request headers or generates a new one if not present.
    /// </summary>
    /// <param name="httpRequest">The incoming HTTP request data.</param>
    /// <returns>The correlation ID to be used for the current request context.</returns>
    private static string GetOrCreateCorrelationId(HttpRequestData? httpRequest)
    {
        if (httpRequest is not null &&
            httpRequest.Headers.TryGetValues(LoggerConstants.CorrelationIdHeader, out var values))
        {
            var headerValue = values?.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(headerValue))
                return headerValue;
        }

        return Guid.NewGuid().ToString();
    }

    #endregion
}
