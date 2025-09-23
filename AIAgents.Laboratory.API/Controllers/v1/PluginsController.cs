using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.API.Helpers;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.PluginsController;

namespace AIAgents.Laboratory.API.Controllers.v1;

/// <summary>
/// The PluginsRoutes Controller Class.
/// </summary>
/// <param name="httpContextAccessor">The http context accessor.</param>
/// <param name="pluginsHandler">The plugins api adapter class.</param>
/// <seealso cref="BaseController"/>
[ApiController]
[ApiVersion(1.0)]
[Route($"{RouteConstants.AiBase_RoutePrefix}/[controller]")]
public class PluginsController(IHttpContextAccessor httpContextAccessor, IPluginsHandler pluginsHandler) : BaseController(httpContextAccessor)
{
	/// <summary>
	/// Rewrites the text async.
	/// </summary>
	/// <param name="requestDto">The rewrite request dto.</param>
	/// <returns>The AI rewritten story.</returns>
	/// <exception cref="Exception"></exception>
	[HttpPost(RouteConstants.PluginsRoutes.RewriteText_Route)]
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = RewriteTextAction.Summary, Description = RewriteTextAction.Description, OperationId = RewriteTextAction.OperationId)]
	public async Task<ResponseDTO> RewriteTextAsync(UserStoryRequestDTO requestDto)
	{
		var result = await pluginsHandler.RewriteTextAsync(requestDto.Story).ConfigureAwait(false);
		if (!string.IsNullOrEmpty(result))
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
	[HttpPost(RouteConstants.PluginsRoutes.GenerateTag_Route)]
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = GenerateTagForStoryAction.Summary, Description = GenerateTagForStoryAction.Description, OperationId = GenerateTagForStoryAction.OperationId)]
	public async Task<ResponseDTO> GenerateTagForStoryAsync(UserStoryRequestDTO requestDto)
	{
		var result = await pluginsHandler.GenerateTagForStoryAsync(requestDto.Story).ConfigureAwait(false);
		if (!string.IsNullOrEmpty(result))
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
	[HttpPost(RouteConstants.PluginsRoutes.ModerateContent_Route)]
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = ModerateContentDataAction.Summary, Description = ModerateContentDataAction.Description, OperationId = ModerateContentDataAction.OperationId)]
	public async Task<ResponseDTO> ModerateContentDataAsync(UserStoryRequestDTO requestDto)
	{
		var result = await pluginsHandler.ModerateContentDataAsync(requestDto.Story).ConfigureAwait(false);
		if (!string.IsNullOrEmpty(result))
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
	[HttpPost(RouteConstants.PluginsRoutes.GetBugSeverity_Route)]
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = GetBugSeverityAction.Summary, Description = GetBugSeverityAction.Description, OperationId = GetBugSeverityAction.OperationId)]
	public async Task<ResponseDTO> GetBugSeverityAsync([FromBody] BugSeverityInputDTO bugSeverityInput)
	{
		var result = await pluginsHandler.GetBugSeverityAsync(bugSeverityInput).ConfigureAwait(false);
		if (!string.IsNullOrEmpty(result))
		{
			return HandleSuccessRequestResponse(result);
		}

		return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
	}
}
