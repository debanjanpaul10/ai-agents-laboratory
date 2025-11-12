using System.Net.Mime;
using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.API.Helpers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.AgentsController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// The Agents Controller class.
/// </summary>
/// <param name="httpContext">The http context accessor.</param>
/// <param name="agentsHandler">The agents handler service.</param>
/// <seealso cref="BaseController" />
[ApiController]
[ApiVersion(2.0)]
[Route($"{RouteConstants.AiBase_RoutePrefix}/[controller]")]
public class AgentsController(IHttpContextAccessor httpContext, IAgentsHandler agentsHandler) : BaseController(httpContext)
{
	/// <summary>
	/// Creates the new agent asynchronous.
	/// </summary>
	/// <param name="agentData">The agent data.</param>
	/// <returns>The boolean for success/failure.</returns>
	/// <exception cref="ArgumentNullException"></exception>
	[HttpPost(RouteConstants.AgentsRoutes.CreateNewAgent_Route)]
	[Consumes(MediaTypeNames.Multipart.FormData)]
	[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = CreateNewAgentAction.Summary, Description = CreateNewAgentAction.Description, OperationId = CreateNewAgentAction.OperationId)]
	public async Task<ResponseDTO> CreateNewAgentAsync([FromForm] CreateAgentDTO agentData)
	{
		if (base.IsRequestAuthorized())
		{
			ArgumentNullException.ThrowIfNull(agentData);

			var result = await agentsHandler.CreateNewAgentAsync(agentData, UserEmail).ConfigureAwait(false);
			if (result)
				return base.HandleSuccessRequestResponse(result);

			return base.HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
		}

		return base.HandleUnAuthorizedRequestResponse();
	}

	/// <summary>
	/// Gets all agents data asynchronous.
	/// </summary>
	/// <returns>The list of <see cref="CreateAgentDTO"/></returns>
	[AllowAnonymous]
	[HttpGet(RouteConstants.AgentsRoutes.GetAllAgents_Route)]
	[ProducesResponseType(typeof(IEnumerable<AgentDataDTO>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = GetAllAgentsDataAction.Summary, Description = GetAllAgentsDataAction.Description, OperationId = GetAllAgentsDataAction.OperationId)]
	public async Task<ResponseDTO> GetAllAgentsDataAsync()
	{
		var result = await agentsHandler.GetAllAgentsDataAsync().ConfigureAwait(false);
		if (result is not null && result.Any())
			return base.HandleSuccessRequestResponse(result);

		return base.HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
	}

	/// <summary>
	/// Gets the agent data by identifier asynchronous.
	/// </summary>
	/// <param name="agentId">The agent identifier.</param>
	/// <returns>The agent data dto model.</returns>
	[HttpGet(RouteConstants.AgentsRoutes.GetAgentById_Route)]
	[ProducesResponseType(typeof(AgentDataDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = GetAllAgentsDataAction.Summary, Description = GetAllAgentsDataAction.Description, OperationId = GetAllAgentsDataAction.OperationId)]
	public async Task<ResponseDTO> GetAgentDataByIdAsync([FromRoute] string agentId)
	{
		var result = await agentsHandler.GetAgentDataByIdAsync(agentId).ConfigureAwait(false);
		if (result is not null)
			return base.HandleSuccessRequestResponse(result);

		return base.HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
	}

	/// <summary>
	/// Updates the existing agent data asynchronous.
	/// </summary>
	/// <param name="updateAgentData">The update agent data.</param>
	/// <returns>The agent data dto.</returns>
	/// <exception cref="ArgumentNullException"></exception>
	[HttpPost(RouteConstants.AgentsRoutes.UpdateExistingAgent_Route)]
	[ProducesResponseType(typeof(AgentDataDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = UpdateExistingAgentDataAction.Summary, Description = UpdateExistingAgentDataAction.Description, OperationId = UpdateExistingAgentDataAction.OperationId)]
	public async Task<ResponseDTO> UpdateExistingAgentDataAsync([FromForm] AgentDataDTO updateAgentData)
	{
		if (IsRequestAuthorized())
		{
			ArgumentNullException.ThrowIfNull(updateAgentData);
			var result = await agentsHandler.UpdateExistingAgentDataAsync(updateAgentData).ConfigureAwait(false);
			if (result)
				return base.HandleSuccessRequestResponse(result);

			return base.HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
		}

		return base.HandleUnAuthorizedRequestResponse();
	}

	/// <summary>
	/// Deletes the existing agent data asynchronous.
	/// </summary>
	/// <param name="agentId">The agent identifier.</param>
	/// <returns>The boolean for success/failure.</returns>
	[HttpPost(RouteConstants.AgentsRoutes.DeleteExistingAgent_Route)]
	[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = DeleteExistingAgentDataAction.Summary, Description = DeleteExistingAgentDataAction.Description, OperationId = DeleteExistingAgentDataAction.OperationId)]
	public async Task<ResponseDTO> DeleteExistingAgentDataAsync([FromRoute] string agentId)
	{
		if (IsRequestAuthorized())
		{
			var result = await agentsHandler.DeleteExistingAgentDataAsync(agentId).ConfigureAwait(false);
			if (result)
				return base.HandleSuccessRequestResponse(result);

			return base.HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
		}

		return base.HandleUnAuthorizedRequestResponse();
	}
}
