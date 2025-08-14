// *********************************************************************************
//	<copyright file="GlobalExceptionMiddleware.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Global Exception Handler.</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Helpers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AIAgents.Laboratory.API.Middleware;

/// <summary>
/// The Global Exception Middleware Class.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <seealso cref="Microsoft.AspNetCore.Diagnostics.IExceptionHandler" />
public class GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger) : IExceptionHandler
{
	/// <summary>
	/// Tries to handle the specified exception asynchronously within the ASP.NET Core pipeline.
	/// Implementations of this method can provide custom exception-handling logic for different scenarios.
	/// </summary>
	/// <param name="httpContext">The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> for the request.</param>
	/// <param name="exception">The unhandled exception.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>
	/// A task that represents the asynchronous read operation. The value of its <see cref="P:System.Threading.Tasks.ValueTask`1.Result" />
	/// property contains the result of the handling operation.
	/// <see langword="true" /> if the exception was handled successfully; otherwise <see langword="false" />.
	/// </returns>
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		var problemDetails = new ProblemDetails();
		problemDetails.Instance = httpContext.Request.Path;
		if (exception is AIAgentsException ex)
		{
			httpContext.Response.StatusCode = (int)ex.StatusCode;
			problemDetails.Title = ex.Message;
		}
		else
		{
			problemDetails.Title = exception.Message;
		}

		logger.LogError(problemDetails.Title);
		problemDetails.Status = httpContext.Response.StatusCode;
		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);

		return true;
	}
}
