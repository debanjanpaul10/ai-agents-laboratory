// *********************************************************************************
//	<copyright file="HealthCheckController.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The IBBS AI Controller Class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static AIAgents.Laboratory.API.Helpers.Constants;

namespace AIAgents.Laboratory.API.Controllers;

[ApiController]
[Route($"{RouteConstants.AiBase_RoutePrefix}/[controller]")]
public class HealthCheckController(ILogger<HealthCheckController> logger) : BaseController
{
	[HttpGet]
	[Route(RouteConstants.HealthCheck.GetAgentStatus_Route)]
	public async Task<bool> GetAgentStatusAsync()
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetAgentStatusAsync), DateTime.UtcNow));
			await Task.CompletedTask;
			return true;

		}
		catch (Exception ex)
		{
			logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetAgentStatusAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetAgentStatusAsync), DateTime.UtcNow));
		}
	}
}
