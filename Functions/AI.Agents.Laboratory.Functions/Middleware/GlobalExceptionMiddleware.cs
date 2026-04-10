using AI.Agents.Laboratory.Functions.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AI.Agents.Laboratory.Functions.Middleware;

/// <summary>
/// Middleware to handle unhandled exceptions globally across all Azure Functions. 
/// Catches exceptions thrown during function execution, logs the error, and returns a standardized JSON error response with appropriate HTTP status codes based on the exception type.
/// </summary>
/// <param name="logger">The logger instance for logging errors.</param>
/// <seealso cref="IFunctionsWorkerMiddleware"/>
public class GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger) : IFunctionsWorkerMiddleware
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionMiddleware"/> class with the specified logger.
    /// </summary>
    private readonly ILogger<GlobalExceptionMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Invokes the middleware to catch unhandled exceptions during function execution. 
    /// If an exception occurs, it logs the error and constructs a standardized error response based on the exception type. 
    /// For known business exceptions, it returns a specific status code and message; for unexpected exceptions, it returns a generic 503 Service Unavailable response.
    /// </summary>
    /// <param name="context">The function execution context.</param>
    /// <param name="next">The next middleware delegate in the pipeline.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Invoke(
        FunctionContext context,
        FunctionExecutionDelegate next)
    {
        try
        {
            await next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred while processing the function.");
            await this.HandleExceptionAsync(
                context,
                ex
            ).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Handles exceptions by creating a standardized error response and setting it as the HTTP response.
    /// </summary>
    /// <param name="context">The function execution context.</param>
    /// <param name="exception">The exception that occurred during function execution.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task HandleExceptionAsync(
        FunctionContext context,
        Exception exception)
    {
        var httpRequest = await context.GetHttpRequestDataAsync().ConfigureAwait(false);
        if (httpRequest is null)
        {
            _logger.LogWarning("No HTTP request data found in the context. Unable to set error response.");
            return;
        }

        var errorResponse = CreateErrorResponse(exception);
        var response = httpRequest.CreateResponse();
        response.Headers.Add("Content-Type", "application/json");
        await response.WriteStringAsync(
            JsonConvert.SerializeObject(errorResponse)
        ).ConfigureAwait(false);

        var invocationResult = context.GetInvocationResult();
        invocationResult.Value = response;
    }

    /// <summary>
    /// Creates an <see cref="ErrorResponse"/> object based on the type of exception.
    /// </summary>
    /// <param name="exception">The exception that occurred during function execution.</param>
    /// <returns>An <see cref="ErrorResponse"/> object representing the error.</returns>
    private static ErrorResponse CreateErrorResponse(Exception exception) =>
        exception switch
        {
            AIAgentsBusinessException validationException => new ErrorResponse
            {
                StatusCode = validationException.StatusCode,
                Message = validationException.Message,
                Details = validationException.ExceptionMessage,
                Timestamp = DateTime.UtcNow
            },
            _ => new ErrorResponse
            {
                StatusCode = StatusCodes.Status503ServiceUnavailable,
                Message = "An unexpected error occurred. Please try again later.",
                Details = exception.Message,
                Timestamp = DateTime.UtcNow
            }
        };
}

/// <summary>
/// Represents a standardized error response to be returned to clients when an exception occurs during function execution.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Gets or sets the HTTP status code to be returned in the response. 
    /// This is determined based on the type of exception (e.g., 400 for business exceptions, 503 for unexpected exceptions).
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets a user-friendly error message to be included in the response body. 
    /// For known business exceptions, this will be the specific error message; for unexpected exceptions, it will be a generic message indicating that an error occurred.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets detailed information about the error, which can be useful for debugging purposes.
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// Gets or sets the correlation ID associated with the request that caused the exception, if available. 
    /// This can help correlate logs and trace the error across different components of the system. 
    /// The correlation ID is typically extracted from the logging context or request headers if it was set by the CorrelationIdMiddleware.
    /// </summary>
    public string? CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the timestamp indicating when the error occurred. This can be useful for tracking and correlating errors in logs and monitoring systems.
    /// </summary>
    public DateTime Timestamp { get; set; }
}