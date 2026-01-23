using System.Net.Mime;
using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Base;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.RouteConstants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.ToolSkillsController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// The tool skills controller.
/// </summary>
/// <param name="httpContextAccessor">The http context accessor service.</param>
/// <param name="configuration">The configuration.</param>
/// <param name="toolSkillsHandler">The tool skills handler</param>
/// <seealso cref="BaseController"/>
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV2)]
[Route("aiagentsapi/v{version:apiVersion}/[controller]")]
public sealed class ToolSkillsController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IToolSkillsHandler toolSkillsHandler) : BaseController(httpContextAccessor, configuration)
{
    /// <summary>
    /// Gets the list of all active tool skills asynchronously.
    /// </summary>
    /// <returns>The list of <see cref="ToolSkillDTO"/></returns>
    [HttpGet(ToolSkillsRoutes.GetAllToolSkills_Route)]
    [ProducesResponseType(typeof(IEnumerable<ToolSkillDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetAllToolSkillsAction.Summary, Description = GetAllToolSkillsAction.Description, OperationId = GetAllToolSkillsAction.OperationId)]
    public async Task<ResponseDTO> GetAllToolSkillsAsync()
    {
        if (base.IsAuthorized())
        {
            var result = await toolSkillsHandler.GetAllToolSkillsAsync(base.UserEmail).ConfigureAwait(false);
            if (result is not null) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }

    /// <summary>
    /// Gets a single tool skill by id.
    /// </summary>
    /// <param name="skillId">The tool skill id.</param>
    /// <returns>The tool skill dto model.</returns>
    [HttpGet(ToolSkillsRoutes.GetToolSkillBySkillId_Route)]
    [ProducesResponseType(typeof(ToolSkillDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetToolSkillBySkillIdAction.Summary, Description = GetToolSkillBySkillIdAction.Description, OperationId = GetToolSkillBySkillIdAction.OperationId)]
    public async Task<ResponseDTO> GetToolSkillBySkillIdAsync([FromRoute] string skillId)
    {
        ArgumentException.ThrowIfNullOrEmpty(skillId);
        if (base.IsAuthorized())
        {
            var result = await toolSkillsHandler.GetToolSkillBySkillIdAsync(skillId, base.UserEmail).ConfigureAwait(false);
            if (result is not null && result.ToolSkillGuid is not null) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status500InternalServerError, ExceptionConstants.DataCannotBeFoundExceptionMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }

    /// <summary>
    /// Creates a new tool skill data.
    /// </summary>
    /// <param name="toolSkillData">The tool skill data dto model.</param>
    /// <returns>The boolean for <c>success/failure.</c></returns>
    [HttpPost(ToolSkillsRoutes.AddNewToolSkill_Route)]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = AddNewToolSkillAction.Summary, Description = AddNewToolSkillAction.Description, OperationId = AddNewToolSkillAction.OperationId)]
    public async Task<ResponseDTO> AddNewToolSkillAsync([FromForm] ToolSkillDTO toolSkillData)
    {
        ArgumentNullException.ThrowIfNull(toolSkillData);
        if (base.IsAuthorized())
        {
            var result = await toolSkillsHandler.AddNewToolSkillAsync(toolSkillData, base.UserEmail).ConfigureAwait(false);
            if (result) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }

    /// <summary>
    /// Updates an existing tool skill data.
    /// </summary>
    /// <param name="updateToolSkillData">The updated tool skill data dto model.</param>
    /// <returns>The boolean for <c>success/failure.</c></returns>
    [HttpPut(ToolSkillsRoutes.UpdateExistingToolSkillData_Route)]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = UpdateExistingToolSkillDataAction.Summary, Description = UpdateExistingToolSkillDataAction.Description, OperationId = UpdateExistingToolSkillDataAction.OperationId)]
    public async Task<ResponseDTO> UpdateExistingToolSkillDataAsync([FromForm] ToolSkillDTO updateToolSkillData)
    {
        ArgumentNullException.ThrowIfNull(updateToolSkillData);
        if (IsAuthorized())
        {
            var result = await toolSkillsHandler.UpdateExistingToolSkillDataAsync(updateToolSkillData, base.UserEmail).ConfigureAwait(false);
            if (result) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }

    /// <summary>
    /// Deletes an existing tool skill by its skill id.
    /// </summary>
    /// <param name="skillId">The tool skill id.</param>
    /// <returns>The boolean for <c>success/failure.</c></returns>
    [HttpDelete(ToolSkillsRoutes.DeleteExistingToolSkillBySkillId_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = DeleteExistingToolSkillBySkillIdAction.Summary, Description = DeleteExistingToolSkillBySkillIdAction.Description, OperationId = DeleteExistingToolSkillBySkillIdAction.OperationId)]
    public async Task<ResponseDTO> DeleteExistingToolSkillBySkillIdAsync([FromRoute] string skillId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(skillId);
        if (IsAuthorized())
        {
            var result = await toolSkillsHandler.DeleteExistingToolSkillBySkillIdAsync(skillId, base.UserEmail).ConfigureAwait(false);
            if (result) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }

    /// <summary>
    /// Gets all the MCP tools available from the given MCP server url.
    /// </summary>
    /// <param name="mcpServerToolRequest">The provided MCP server tools data request model DTO.</param>
    /// <returns>The list of <see cref="McpServerToolsDTO"/></returns>
    [HttpPost(ToolSkillsRoutes.GetAllMcpToolsAvailable_Route)]
    [ProducesResponseType(typeof(IEnumerable<McpServerToolsDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetAllMcpToolsAvailableAction.Summary, Description = GetAllMcpToolsAvailableAction.Description, OperationId = GetAllMcpToolsAvailableAction.OperationId)]
    public async Task<ResponseDTO> GetAllMcpToolsAvailableAsync(McpServerToolRequestDTO mcpServerToolRequest)
    {
        ArgumentNullException.ThrowIfNull(mcpServerToolRequest);
        ArgumentException.ThrowIfNullOrWhiteSpace(mcpServerToolRequest.ServerUrl);
        if (IsAuthorized())
        {
            var result = await toolSkillsHandler.GetAllMcpToolsAvailableAsync(mcpServerToolRequest.ServerUrl, base.UserEmail).ConfigureAwait(false);
            if (result is not null) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }
}
