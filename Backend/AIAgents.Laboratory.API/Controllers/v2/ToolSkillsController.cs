using System.Net.Mime;
using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Base;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.AuthorizationTypes;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.RouteConstants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.ToolSkillsController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// The <c>ToolSkillsController</c> class is an API controller responsible for handling HTTP requests related to tool skills management in the AIAgents Laboratory application.
/// </summary>
/// <remarks>It provides endpoints for creating, retrieving, updating, and deleting tool skills, as well as fetching available MCP tools from a given MCP server URL.</remarks>
/// <param name="httpContextAccessor">The http context accessor service.</param>
/// <param name="configuration">The configuration.</param>
/// <param name="toolSkillsHandler">The tool skills handler</param>
/// <seealso cref="BaseController"/>
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV2)]
[Route("aiagentsapi/v{version:apiVersion}/[controller]")]
public sealed class ToolSkillsController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
    ICorrelationContext correlationContext, ILogger<ToolSkillsController> logger, IToolSkillsHandler toolSkillsHandler) : BaseController(httpContextAccessor, configuration)
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
    public async Task<ResponseDto> GetAllToolSkillsAsync()
    {
        IEnumerable<ToolSkillDTO> result = [];
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllToolSkillsAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail }));

            if (base.IsAuthorized(UserBased))
            {
                result = await toolSkillsHandler.GetAllToolSkillsAsync(base.UserEmail).ConfigureAwait(false);
                if (result is not null)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllToolSkillsAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllToolSkillsAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
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
    public async Task<ResponseDto> GetToolSkillBySkillIdAsync([FromQuery] string skillId)
    {
        ToolSkillDTO result = new();
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetToolSkillBySkillIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, skillId }));

            ArgumentException.ThrowIfNullOrEmpty(skillId);
            if (base.IsAuthorized(UserBased))
            {
                result = await toolSkillsHandler.GetToolSkillBySkillIdAsync(skillId, base.UserEmail).ConfigureAwait(false);
                if (result is not null && result.ToolSkillGuid is not null)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status500InternalServerError, ExceptionConstants.DataCannotBeFoundExceptionMessage);
            }

            return HandleUnAuthorizedRequestResponse();

        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetToolSkillBySkillIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetToolSkillBySkillIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, skillId, result }));
        }
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
    public async Task<ResponseDto> AddNewToolSkillAsync([FromForm] ToolSkillDTO toolSkillData)
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(AddNewToolSkillAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, toolSkillData }));

            ArgumentNullException.ThrowIfNull(toolSkillData);
            if (base.IsAuthorized(UserBased))
            {
                result = await toolSkillsHandler.AddNewToolSkillAsync(toolSkillData, base.UserEmail).ConfigureAwait(false);
                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(AddNewToolSkillAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(AddNewToolSkillAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
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
    public async Task<ResponseDto> UpdateExistingToolSkillDataAsync([FromForm] ToolSkillDTO updateToolSkillData)
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingToolSkillDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, updateToolSkillData }));

            ArgumentNullException.ThrowIfNull(updateToolSkillData);
            if (base.IsAuthorized(UserBased))
            {
                result = await toolSkillsHandler.UpdateExistingToolSkillDataAsync(updateToolSkillData, base.UserEmail).ConfigureAwait(false);
                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(UpdateExistingToolSkillDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(UpdateExistingToolSkillDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
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
    public async Task<ResponseDto> DeleteExistingToolSkillBySkillIdAsync([FromQuery] string skillId)
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteExistingToolSkillBySkillIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, skillId }));

            ArgumentException.ThrowIfNullOrWhiteSpace(skillId);
            if (base.IsAuthorized(UserBased))
            {
                result = await toolSkillsHandler.DeleteExistingToolSkillBySkillIdAsync(skillId, base.UserEmail).ConfigureAwait(false);
                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DeleteExistingToolSkillBySkillIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteExistingToolSkillBySkillIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, skillId, result }));
        }
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
    public async Task<ResponseDto> GetAllMcpToolsAvailableAsync(McpServerToolRequestDTO mcpServerToolRequest)
    {
        IEnumerable<McpServerToolsDTO> result = [];
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllMcpToolsAvailableAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, mcpServerToolRequest }));

            ArgumentNullException.ThrowIfNull(mcpServerToolRequest);
            ArgumentException.ThrowIfNullOrWhiteSpace(mcpServerToolRequest.ServerUrl);
            if (base.IsAuthorized(UserBased))
            {
                result = await toolSkillsHandler.GetAllMcpToolsAvailableAsync(
                    serverUrl: mcpServerToolRequest.ServerUrl,
                    currentUserEmail: base.UserEmail).ConfigureAwait(false);

                if (result is not null)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllMcpToolsAvailableAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllMcpToolsAvailableAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }
}
