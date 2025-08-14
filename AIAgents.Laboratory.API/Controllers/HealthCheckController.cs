// *********************************************************************************
//	<copyright file="HealthCheckController.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Health Check Endpoint API controller.</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static AIAgents.Laboratory.API.Helpers.Constants;

namespace AIAgents.Laboratory.API.Controllers;

/// <summary>
/// The Health Check Endpoint API controller.
/// </summary>
/// <param name="commonAiHandler">The common AI handler.</param>
/// <param name="logger">The logger service.</param>
/// <seealso cref="AIAgents.Laboratory.API.Controllers.BaseController" />
[ApiController]
[Route($"{RouteConstants.AiBase_RoutePrefix}/[controller]")]
public class HealthCheckController(ILogger<HealthCheckController> logger, ICommonAiHandler commonAiHandler) : BaseController
{
	/// <summary>
	/// Gets the agent status.
	/// </summary>
	/// <returns>The action result of the response.</returns>
	[HttpGet(RouteConstants.HealthCheck.GetAgentStatus_Route)]
	public IActionResult GetAgentStatus()
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetAgentStatus), DateTime.UtcNow));
			return Ok(commonAiHandler.GetAgentCurrentStatus());
		}
		catch (Exception ex)
		{
			logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetAgentStatus), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetAgentStatus), DateTime.UtcNow));
		}
	}
}
