using AI.Agents.Laboratory.Functions.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AI.Agents.Laboratory.Functions.Middleware;

public class GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger) : IFunctionsWorkerMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

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

public class ErrorResponse
{
    public int StatusCode { get; set; }

    public string Message { get; set; } = string.Empty;

    public string? Details { get; set; }

    public string? CorrelationId { get; set; }

    public DateTime Timestamp { get; set; }
}