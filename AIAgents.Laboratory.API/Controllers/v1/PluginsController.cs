using System.Globalization;
using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.PluginsController;

namespace AIAgents.Laboratory.API.Controllers.v1;

/// <summary>
/// The AI plugins controller.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="pluginsHandler">The plugins handler</param>
[ApiController]
[Route($"{RouteConstants.AiBase_RoutePrefix}/[controller]")]
public class PluginsController(ILogger<PluginsController> logger, IPluginsHandler pluginsHandler)
{
	/// <summary>
	/// Rewrites the text async.
	/// </summary>
	/// <param name="requestDto">The rewrite request dto.</param>
	/// <returns>The AI rewritten story.</returns>
	/// <exception cref="Exception"></exception>
	[HttpPost(RouteConstants.Plugins.RewriteText_Route)]
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

			var result = await pluginsHandler.RewriteTextAsync(requestDto.Story).ConfigureAwait(false);
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
	[HttpPost(RouteConstants.Plugins.GenerateTag_Route)]
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

			var result = await pluginsHandler.GenerateTagForStoryAsync(requestDto.Story).ConfigureAwait(false);
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
	[HttpPost(RouteConstants.Plugins.ModerateContent_Route)]
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

			var result = await pluginsHandler.ModerateContentDataAsync(requestDto.Story).ConfigureAwait(false);
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

	/// <summary>
	/// Gets the bug severity data.
	/// </summary>
	/// <param name="bugSeverityInput">The bug severity input.</param>
	/// <returns>The bug severity ai response dto.</returns>
	[HttpPost(RouteConstants.Plugins.GetBugSeverity_Route)]
	[ProducesResponseType(typeof(BugSeverityResponseDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = GetBugSeverityAction.Summary, Description = GetBugSeverityAction.Description, OperationId = GetBugSeverityAction.OperationId)]
	public async Task<BugSeverityResponseDTO> GetBugSeverityAsync([FromBody] BugSeverityInputDTO bugSeverityInput)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetBugSeverityAsync), DateTime.UtcNow));

			var result = await pluginsHandler.GetBugSeverityAsync(bugSeverityInput).ConfigureAwait(false);
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
}
