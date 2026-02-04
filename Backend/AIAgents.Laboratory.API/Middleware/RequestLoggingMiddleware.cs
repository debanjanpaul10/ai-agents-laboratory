using System.Diagnostics;
using static AIAgents.Laboratory.API.Helpers.Constants;

namespace AIAgents.Laboratory.API.Middleware;

/// <summary>
/// Middleware to log incoming HTTP requests and their responses.
/// </summary>
/// <param name="next">The next RequestDelegate in the pipeline.</param>
/// <param name="logger">The ILogger instance for logging.</param>
public sealed class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    /// <summary>
    /// Invokes the middleware to process the HTTP context.
    /// </summary>
    /// <param name="httpContext">The HttpContext to process.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var stopwatch = Stopwatch.StartNew();

        // Log request
        logger.LogInformation(LoggingConstants.HttpLoggingMessage, httpContext.Request.Method, httpContext.Request.Path);

        try
        {
            await next(httpContext);
        }
        finally
        {
            var loggingForWarning = httpContext.Response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
            var logLevel = httpContext.Response.StatusCode >= 500 ? LogLevel.Error : loggingForWarning;
            logger.Log(logLevel, LoggingConstants.HttpLoggingMessageWithTime,
                httpContext.Request.Method, httpContext.Request.Path, httpContext.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
    }
}

/// <summary>
/// The extension methods for RequestLoggingMiddleware.
/// </summary>
public static class RequestLoggingMiddlewareExtensions
{
    /// <summary>
    /// Adds the RequestLoggingMiddleware to the application's request pipeline.
    /// </summary>
    /// <param name="builder">The IApplicationBuilder to which the middleware is added.</param>
    /// <returns>The modified IApplicationBuilder.</returns>
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder) => builder.UseMiddleware<RequestLoggingMiddleware>();
}
