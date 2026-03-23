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
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.WorkspacesController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// The <c>WorkspacesController</c> class is an API controller responsible for handling HTTP requests related to workspace management in the AIAgents Laboratory application.
/// </summary>
/// <remarks>It provides endpoints for creating, retrieving, updating, and deleting workspaces, as well as invoking workspace agents via chat.</remarks>
/// <param name="httpContextAccessor">The http context accessor.</param>
/// <param name="workspacesHandler">The workspaces api adapter handler.</param>
/// <param name="configuration">The configuration.</param>
/// <param name="correlationContext">The correlation context used for logging.</param>
/// <param name="logger">The logger service.</param>
/// <seealso cref="BaseController"/>
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV2)]
[Route("aiagentsapi/v{version:apiVersion}/[controller]")]
public sealed class WorkspacesController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
    ICorrelationContext correlationContext, ILogger<WorkspacesController> logger, IWorkspacesHandler workspacesHandler) : BaseController(httpContextAccessor, configuration)
{
    /// <summary>
    /// Gets the list of all available workspaces available.
    /// </summary>
    /// <returns>The list of <see cref="AgentsWorkspaceDTO"/></returns>
    [HttpGet(WorkspacesRoutes.GetAllWorkspaces_Route)]
    [ProducesResponseType(typeof(IEnumerable<AgentsWorkspaceDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetAllWorkspacesAction.Summary, Description = GetAllWorkspacesAction.Description, OperationId = GetAllWorkspacesAction.OperationId)]
    public async Task<ResponseDto> GetAllWorkspacesAsync()
    {
        IEnumerable<AgentsWorkspaceDTO> result = [];
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllWorkspacesAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail }));

            if (base.IsAuthorized(UserBased))
            {
                result = await workspacesHandler.GetAllWorkspacesAsync(base.UserEmail).ConfigureAwait(false);
                if (result is not null)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllWorkspacesAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllWorkspacesAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Gets the workspace by workspace id.
    /// </summary>
    /// <param name="workspaceId">The workspace guid id.</param>
    /// <returns>The agents workspace dto model.</returns>
    [HttpGet(WorkspacesRoutes.GetWorkspaceByWorkspaceId_Route)]
    [ProducesResponseType(typeof(AgentsWorkspaceDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetWorkspaceByWorkspaceIdAction.Summary, Description = GetWorkspaceByWorkspaceIdAction.Description, OperationId = GetWorkspaceByWorkspaceIdAction.OperationId)]
    public async Task<ResponseDto> GetWorkspaceByWorkspaceIdAsync([FromQuery] string workspaceId)
    {
        AgentsWorkspaceDTO result = new();
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetWorkspaceByWorkspaceIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, workspaceId }));

            ArgumentException.ThrowIfNullOrEmpty(workspaceId);
            if (base.IsAuthorized(UserBased))
            {
                result = await workspacesHandler.GetWorkspaceByWorkspaceIdAsync(workspaceId, base.UserEmail).ConfigureAwait(false);
                if (result is not null)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetWorkspaceByWorkspaceIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetWorkspaceByWorkspaceIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, workspaceId, result }));
        }
    }

    /// <summary>
    /// Creates a new workspace.
    /// </summary>
    /// <param name="agentsWorkspaceData">The workspace data dto model.</param>
    /// <returns>A boolean for success/failure.</returns>
    [HttpPost(WorkspacesRoutes.AddNewWorkspace_Route)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = CreateNewWorkspaceAction.Summary, Description = CreateNewWorkspaceAction.Description, OperationId = CreateNewWorkspaceAction.OperationId)]
    public async Task<ResponseDto> CreateNewWorkspaceAsync([FromBody] AgentsWorkspaceDTO agentsWorkspaceData)
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateNewWorkspaceAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, agentsWorkspaceData }));

            ArgumentNullException.ThrowIfNull(agentsWorkspaceData);
            if (base.IsAuthorized(UserBased))
            {
                result = await workspacesHandler.CreateNewWorkspaceAsync(agentsWorkspaceData, base.UserEmail).ConfigureAwait(false);
                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(CreateNewWorkspaceAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(CreateNewWorkspaceAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Deletes an existing workspace.
    /// </summary>
    /// <param name="workspaceGuidId">The workspace guid id.</param>
    /// <returns>A boolean for success/failure.</returns>
    [HttpDelete(WorkspacesRoutes.DeleteExistingWorkspace_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = DeleteExistingWorkspaceAction.Summary, Description = DeleteExistingWorkspaceAction.Description, OperationId = DeleteExistingWorkspaceAction.OperationId)]
    public async Task<ResponseDto> DeleteExistingWorkspaceAsync([FromQuery] string workspaceGuidId)
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteExistingWorkspaceAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, workspaceGuidId }));

            ArgumentException.ThrowIfNullOrEmpty(workspaceGuidId);
            if (base.IsAuthorized(UserBased))
            {
                result = await workspacesHandler.DeleteExistingWorkspaceAsync(workspaceGuidId, base.UserEmail).ConfigureAwait(false);
                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DeleteExistingWorkspaceAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteExistingWorkspaceAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, workspaceGuidId, result }));
        }
    }

    /// <summary>
    /// Updates an existing workspace.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data dto model.</param>
    /// <returns>The boolean for success/failure.</returns>
    [HttpPut(WorkspacesRoutes.UpdateExistingWorkspace_Route)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = UpdateExistingWorkspaceDataAction.Summary, Description = UpdateExistingWorkspaceDataAction.Description, OperationId = UpdateExistingWorkspaceDataAction.OperationId)]
    public async Task<ResponseDto> UpdateExistingWorkspaceDataAsync([FromBody] AgentsWorkspaceDTO agentsWorkspaceData)
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingWorkspaceDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, agentsWorkspaceData }));

            ArgumentNullException.ThrowIfNull(agentsWorkspaceData);
            if (base.IsAuthorized(UserBased))
            {
                result = await workspacesHandler.UpdateExistingWorkspaceDataAsync(agentsWorkspaceData, base.UserEmail).ConfigureAwait(false);
                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(UpdateExistingWorkspaceDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(UpdateExistingWorkspaceDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Invokes the workspace agent via chat.
    /// </summary>
    /// <param name="chatRequestDTO">The chat request DTO model.</param>
    /// <returns>The ai agent response string.</returns>
    [HttpPost(WorkspacesRoutes.InvokeWorkspaceAgent_Route)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = InvokeWorkspaceAgentAction.Summary, Description = InvokeWorkspaceAgentAction.Description, OperationId = InvokeWorkspaceAgentAction.OperationId)]
    public async Task<ResponseDto> InvokeWorkspaceAgentAsync([FromBody] WorkspaceAgentChatRequestDTO chatRequestDTO)
    {
        string result = string.Empty;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(InvokeWorkspaceAgentAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, chatRequestDTO }));

            ArgumentNullException.ThrowIfNull(chatRequestDTO);
            if (base.IsAuthorized(ApplicationBased))
            {
                result = await workspacesHandler.InvokeWorkspaceAgentAsync(chatRequestDTO).ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(result))
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(InvokeWorkspaceAgentAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(InvokeWorkspaceAgentAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Gets the workspace group chat response.
    /// </summary>
    /// <param name="chatRequestDTO">The workspace agent chat request dto model.</param>
    /// <returns>The response from the group chat.</returns>
    [HttpPost(WorkspacesRoutes.GetWorkspaceGroupChatResponse_Route)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(GroupChatResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetWorkspaceGroupChatResponseAction.Summary, Description = GetWorkspaceGroupChatResponseAction.Description, OperationId = GetWorkspaceGroupChatResponseAction.OperationId)]
    public async Task<ResponseDto> GetWorkspaceGroupChatResponseAsync([FromBody] WorkspaceAgentChatRequestDTO chatRequestDTO)
    {
        GroupChatResponseDTO result = new();
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetWorkspaceGroupChatResponseAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, chatRequestDTO }));

            ArgumentNullException.ThrowIfNull(chatRequestDTO);
            if (base.IsAuthorized(ApplicationBased))
            {
                result = await workspacesHandler.GetWorkspaceGroupChatResponseAsync(chatRequest: chatRequestDTO).ConfigureAwait(false);
                if (result is not null && !string.IsNullOrEmpty(result.AgentResponse))
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();

        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetWorkspaceGroupChatResponseAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetWorkspaceGroupChatResponseAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }
}
