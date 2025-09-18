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

namespace AIAgents.Laboratory.API.Controllers.V2;

/// <summary>
/// The Agents Controller class.
/// </summary>
/// <param name="agentsHandler">The agents handler service.</param>
/// <seealso cref="BaseController" />
[ApiController]
[AllowAnonymous]
[ApiVersion(2.0)]
[Route($"{RouteConstants.AiBase_RoutePrefix}/[controller]")]
public class AgentsController(IAgentsHandler agentsHandler) : BaseController
{
	/// <summary>
	/// Creates the new agent asynchronous.
	/// </summary>
	/// <param name="agentData">The agent data.</param>
	/// <returns>The boolean for success/failure.</returns>
	/// <exception cref="ArgumentNullException"></exception>
	[HttpPost(RouteConstants.AgentsRoutes.CreateNewAgent_Route)]
	[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = CreateNewAgentAction.Summary, Description = CreateNewAgentAction.Description, OperationId = CreateNewAgentAction.OperationId)]
	public async Task<ResponseDTO> CreateNewAgentAsync([FromBody] CreateAgentDTO agentData)
	{
		ArgumentNullException.ThrowIfNull(agentData);
		var result = await agentsHandler.CreateNewAgentAsync(agentData).ConfigureAwait(false);
		if (result)
		{
			return HandleSuccessRequestResponse(result);
		}

		return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
	}

	/// <summary>
	/// Gets all agents data asynchronous.
	/// </summary>
	/// <returns>The list of <see cref="CreateAgentDTO"/></returns>
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
		{
			return HandleSuccessRequestResponse(result);
		}

		return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
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
		{
			return HandleSuccessRequestResponse(result);
		}

		return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
	}
}
