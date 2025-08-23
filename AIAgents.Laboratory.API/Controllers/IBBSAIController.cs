// *********************************************************************************
//	<copyright file="BulletinAiController.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The IBBS AI Controller Class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request.IBBS;
using AIAgents.Laboratory.API.Adapters.Models.Response.IBBS;
using AIAgents.Laboratory.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.IBBSAIController;

namespace AIAgents.Laboratory.API.Controllers;

/// <summary>
/// The IBBS AI Controller Class.
/// </summary>
/// <param name="bulletinAiHandler">The Bulletin AI Handler.</param>
/// <param name="logger">The logger service.</param>
/// <seealso cref="AIAgents.Laboratory.API.Controllers.BaseController" />
[ApiController]
[Route($"{RouteConstants.AiBase_RoutePrefix}/[controller]")]
public class IBBSAIController(ILogger<IBBSAIController> logger, IBulletinAiHandler bulletinAiHandler) : BaseController
{
	/// <summary>
	/// Rewrites the text async.
	/// </summary>
	/// <param name="requestDto">The rewrite request dto.</param>
	/// <returns>The AI rewritten story.</returns>
	/// <exception cref="Exception"></exception>
	[HttpPost(RouteConstants.IBBSAi.RewriteText_Route)]
	[ProducesResponseType(typeof(RewriteResponseDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = RewriteTextAction.Summary, Description = RewriteTextAction.Description, OperationId = RewriteTextAction.OperationId)]
	public async Task<RewriteResponseDTO> RewriteTextAsync(UserStoryRequestDTO requestDto)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(RewriteTextAsync), DateTime.UtcNow));

			var result = await bulletinAiHandler.RewriteTextAsync(requestDto.Story).ConfigureAwait(false);
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

	/// <summary>
	/// Generates the tag for story asynchronous.
	/// </summary>
	/// <param name="requestDto">The request dto.</param>
	/// <returns>The tag response dto.</returns>
	[HttpPost(RouteConstants.IBBSAi.GenerateTag_Route)]
	[ProducesResponseType(typeof(TagResponseDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = GenerateTagForStoryAction.Summary, Description = GenerateTagForStoryAction.Description, OperationId = GenerateTagForStoryAction.OperationId)]
	public async Task<TagResponseDTO> GenerateTagForStoryAsync(UserStoryRequestDTO requestDto)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GenerateTagForStoryAsync), DateTime.UtcNow));

			var result = await bulletinAiHandler.GenerateTagForStoryAsync(requestDto.Story).ConfigureAwait(false);
			if (string.IsNullOrEmpty(result.UserStoryTag))
			{
				var exception = new Exception(ExceptionConstants.AiServicesDownMessage);
				logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GenerateTagForStoryAsync), DateTime.UtcNow, exception.Message));
				throw exception;
			}
			else
			{
				return result;
			}
		}
		catch (Exception ex)
		{
			logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GenerateTagForStoryAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GenerateTagForStoryAsync), DateTime.UtcNow));
		}
	}

	/// <summary>
	/// Moderates the content data asynchronous.
	/// </summary>
	/// <param name="requestDto">The request dto.</param>
	/// <returns>The moderation content response.</returns>
	[HttpPost(RouteConstants.IBBSAi.ModerateContent_Route)]
	[ProducesResponseType(typeof(ModerationContentResponseDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = ModerateContentDataAction.Summary, Description = ModerateContentDataAction.Description, OperationId = ModerateContentDataAction.OperationId)]
	public async Task<ModerationContentResponseDTO> ModerateContentDataAsync(UserStoryRequestDTO requestDto)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(ModerateContentDataAsync), DateTime.UtcNow));
			
			var result = await bulletinAiHandler.ModerateContentDataAsync(requestDto.Story).ConfigureAwait(false);
			if (string.IsNullOrEmpty(result.ContentRating))
			{
				var exception = new Exception(ExceptionConstants.AiServicesDownMessage);
				logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(ModerateContentDataAsync), DateTime.UtcNow, exception.Message));
				throw exception;
			}
			else
			{
				return result;
			}
		}
		catch (Exception ex)
		{
			logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(ModerateContentDataAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(ModerateContentDataAsync), DateTime.UtcNow));
		}
	}
}
