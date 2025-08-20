// *********************************************************************************
//	<copyright file="FitGymToolAIController.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The FitGym Tool AI API controller.</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Request.FitGymTool;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.API.Adapters.Models.Response.FitGymTool;
using AIAgents.Laboratory.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static AIAgents.Laboratory.API.Helpers.Constants;

namespace AIAgents.Laboratory.API.Controllers;

/// <summary>
/// The FitGym Tool AI API controller.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="fitGymToolAIHandler">The fitgym tool ai handler.</param>
[ApiController]
[Route($"{RouteConstants.AiBase_RoutePrefix}/[controller]")]
public class FitGymToolAIController(ILogger<FitGymToolAIController> logger, IFitGymToolAIHandler fitGymToolAIHandler) : BaseController
{
	/// <summary>
	/// Gets the bug severity data.
	/// </summary>
	/// <param name="bugSeverityInput">The bug severity input.</param>
	/// <returns>The bug severity ai response dto.</returns>
	[HttpPost(RouteConstants.FitGymToolAi.GetBugSeverity_Route)]
	public async Task<BugSeverityResponseDTO> GetBugSeverityAsync([FromBody] BugSeverityInputDTO bugSeverityInput)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetBugSeverityAsync), DateTime.UtcNow));

			var result = await fitGymToolAIHandler.GetBugSeverityAsync(bugSeverityInput).ConfigureAwait(false);
			if (string.IsNullOrEmpty(result.BugSeverity))
			{
				var exception = new Exception(ExceptionConstants.AiServicesDownMessage);
				logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetBugSeverityAsync), DateTime.UtcNow, exception.Message));
				throw exception;
			}
			else
			{
				return result;
			}
		}
		catch (Exception ex)
		{
			logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetBugSeverityAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetBugSeverityAsync), DateTime.UtcNow));
		}
	}

	/// <summary>
	/// Gets the chatbot response asynchronous.
	/// </summary>
	/// <param name="userQueryRequest">The user query request.</param>
	/// <returns>The ai response.</returns>
	[HttpPost(RouteConstants.FitGymToolAi.GetChatbotResponse_Route)]
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ResponseDTO> GetChatbotResponseAsync([FromBody] UserQueryRequestDTO userQueryRequest)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetChatbotResponseAsync), DateTime.UtcNow));

			var result = await fitGymToolAIHandler.GetOrchestratorResponseAsync(userQueryRequest).ConfigureAwait(false);
			if (!string.IsNullOrEmpty(result))
			{
				return HandleSuccessRequestResponse(result);
			}

			return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
		}
		catch (Exception ex)
		{
			logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetChatbotResponseAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetChatbotResponseAsync), DateTime.UtcNow));
		}
	}



}
