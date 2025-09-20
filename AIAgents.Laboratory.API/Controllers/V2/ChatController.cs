﻿using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.API.Helpers;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.ChatController;

namespace AIAgents.Laboratory.API.Controllers.V2;

/// <summary>
/// The Chat API Controller class.
/// </summary>
/// <param name="httpContextAccessor">The http context accessor.</param>
/// <param name="chatHandler">The Chat API adapter handler.</param>
/// <seealso cref="AIAgents.Laboratory.API.Controllers.BaseController" />
[ApiController]
[ApiVersion(2.0)]
[Route($"{RouteConstants.AiBase_RoutePrefix}/[controller]")]
public class ChatController(IHttpContextAccessor httpContextAccessor, IChatHandler chatHandler) : BaseController(httpContextAccessor)
{
	/// <summary>
	/// Invokes the chat agent asynchronous.
	/// </summary>
	/// <param name="chatRequestDTO">The chat request dto.</param>
	/// <returns>The AI response.</returns>
	/// <exception cref="System.ArgumentNullException"></exception>
	[HttpPost(RouteConstants.ChatRoutes.InvokeAgent_Route)]
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = InvokeAgentAction.Summary, Description = InvokeAgentAction.Description, OperationId = InvokeAgentAction.OperationId)]
	public async Task<ResponseDTO> InvokeChatAgentAsync([FromBody] ChatRequestDTO chatRequestDTO)
	{
		ArgumentNullException.ThrowIfNull(chatRequestDTO);
		var result = await chatHandler.InvokeChatAgentAsync(chatRequestDTO).ConfigureAwait(false);
		if (!string.IsNullOrEmpty(result))
		{
			return HandleSuccessRequestResponse(result);
		}

		return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
	}
}
