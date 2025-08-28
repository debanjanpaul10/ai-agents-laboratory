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
/// The Plugins Controller Class.
/// </summary>
/// <param name="pluginsHandler">The plugins api adapter class.</param>
/// <seealso cref="BaseController"/>
[ApiController]
[Route($"{RouteConstants.AiBase_RoutePrefix}/[controller]")]
public class PluginsController(IPluginsHandler pluginsHandler) : BaseController
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
	public async Task<ResponseDTO> RewriteTextAsync(UserStoryRequestDTO requestDto)
	{
		var result = await pluginsHandler.RewriteTextAsync(requestDto.Story).ConfigureAwait(false);
		if (!string.IsNullOrEmpty(result.RewrittenStory))
		{
			return HandleSuccessRequestResponse(result);
		}

		return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
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
	public async Task<ResponseDTO> GenerateTagForStoryAsync(UserStoryRequestDTO requestDto)
	{
		var result = await pluginsHandler.GenerateTagForStoryAsync(requestDto.Story).ConfigureAwait(false);
		if (!string.IsNullOrEmpty(result.UserStoryTag))
		{
			return HandleSuccessRequestResponse(result);
		}

		return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
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
	public async Task<ResponseDTO> ModerateContentDataAsync(UserStoryRequestDTO requestDto)
	{
		var result = await pluginsHandler.ModerateContentDataAsync(requestDto.Story).ConfigureAwait(false);
		if (!string.IsNullOrEmpty(result.ContentRating))
		{
			return HandleSuccessRequestResponse(result);
		}

		return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
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
	public async Task<ResponseDTO> GetBugSeverityAsync([FromBody] BugSeverityInputDTO bugSeverityInput)
	{

		var result = await pluginsHandler.GetBugSeverityAsync(bugSeverityInput).ConfigureAwait(false);
		if (!string.IsNullOrEmpty(result.BugSeverity))
		{
			return HandleSuccessRequestResponse(result);
		}

		return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
	}
}
