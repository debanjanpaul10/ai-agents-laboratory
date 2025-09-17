using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.API.Helpers;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.AgentSkillsController;

namespace AIAgents.Laboratory.API.Controllers.V2;

/// <summary>
/// The Agent Skills Controller class.
/// </summary>
/// <param name="agentSkillsHandler">The agent skills handler service.</param>
/// <seealso cref="BaseController" />
[ApiController]
[ApiVersion(2.0)]
[Route($"{RouteConstants.AiBase_RoutePrefix}/[controller]")]
public class AgentSkillsController(IAgentSkillsHandler agentSkillsHandler) : BaseController
{
	/// <summary>
	/// Creates the new skill asynchronous.
	/// </summary>
	/// <param name="skillRequestDTO">The skill request dto.</param>
	/// <returns>The boolean for success/failure.</returns>
	[HttpPost(RouteConstants.AgentSkillsRoutes.CreateNewSkill_Route)]
	[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = CreateNewSkillAction.Summary, Description = CreateNewSkillAction.Description, OperationId = CreateNewSkillAction.OperationId)]
	public async Task<ResponseDTO> CreateNewSkillAsync([FromBody] AgentSkillDTO skillRequestDTO)
	{
		ArgumentNullException.ThrowIfNull(skillRequestDTO);
		var result = await agentSkillsHandler.CreateNewSkillAsync(skillRequestDTO).ConfigureAwait(false);
		if (result)
		{
			return HandleSuccessRequestResponse(result);
		}

		return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
	}

	/// <summary>
	/// Gets all skills asynchronous.
	/// </summary>
	/// <returns>The list of <see cref="AgentSkillDTO"/></returns>
	[HttpGet(RouteConstants.AgentSkillsRoutes.GetAllSkills_Route)]
	[ProducesResponseType(typeof(IEnumerable<AgentSkillDTO>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = GetAllSkillsAction.Summary, Description = GetAllSkillsAction.Description, OperationId = GetAllSkillsAction.OperationId)]
	public async Task<ResponseDTO> GetAllSkillsAsync()
	{
		var result = await agentSkillsHandler.GetAllSkillsAsync().ConfigureAwait(false);
		if (result is not null && result.Any())
		{
			return HandleSuccessRequestResponse(result);
		}

		return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
	}

	/// <summary>
	/// Gets the skill by identifier asynchronous.
	/// </summary>
	/// <param name="skillId">The skill identifier.</param>
	/// <returns>The agent skill dto model.</returns>
	[HttpGet(RouteConstants.AgentSkillsRoutes.GetSkillById_Route)]
	[ProducesResponseType(typeof(AgentSkillDTO), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[SwaggerOperation(Summary = GetSkillByIdAction.Summary, Description = GetSkillByIdAction.Description, OperationId = GetSkillByIdAction.OperationId)]
	public async Task<ResponseDTO> GetSkillByIdAsync([FromRoute] string skillId)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(nameof(skillId));
		var result = await agentSkillsHandler.GetSkillByIdAsync(skillId).ConfigureAwait(false);
		if (result is not null)
		{
			return HandleSuccessRequestResponse(result);
		}

		return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
	}
}
