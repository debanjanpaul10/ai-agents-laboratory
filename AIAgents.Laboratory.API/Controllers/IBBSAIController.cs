// *********************************************************************************
//	<copyright file="BulletinAiController.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Bulletin AI Controller Class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Core.Contracts;
using AIAgents.Laboratory.Shared.Constants;
using AIAgents.Laboratory.Shared.Models.IBBS;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AIAgents.Laboratory.API.Controllers;

[ApiController]
[Route($"{RouteConstants.AiBase_RoutePrefix}/[controller]")]
public class IBBSAIController(ILogger<IBBSAIController> logger, IBulletinAIServices bulletinAiServices) : BaseController
{
	/// <summary>
	/// Rewrites the text async.
	/// </summary>
	/// <param name="requestDto">The rewrite request dto.</param>
	/// <returns>The AI rewritten story.</returns>
	/// <exception cref="Exception"></exception>
	[HttpPost]
	[Route(RouteConstants.IBBSAi.RewriteText_Route)]
	public async Task<RewriteResponseDTO> RewriteTextAsync(UserStoryRequestDTO requestDto)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(RewriteTextAsync), DateTime.UtcNow));
			{
				var result = await bulletinAiServices.RewriteTextAsync(requestDto.Story).ConfigureAwait(false);
				if (string.IsNullOrEmpty(result.RewrittenStory))
				{
					var exception = new Exception(ExceptionConstants.AiServicesDownMessage);
					logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(RewriteTextAsync), DateTime.UtcNow, exception.Message));
					throw exception;
				}
				else
				{
					return result;
				}
			}

		}
		catch (Exception ex)
		{
			logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(RewriteTextAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(RewriteTextAsync), DateTime.UtcNow));
		}
	}
}
