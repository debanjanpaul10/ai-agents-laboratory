using System.Net.Mime;
using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.RouteConstants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.WorkspacesController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// The workspaces controller class.
/// </summary>
/// <param name="httpContextAccessor">The http context accessor.</param>
/// <param name="workspacesHandler">The workspaces api adapter handler.</param>
/// <seealso cref="BaseController"/>
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV2)]
[Route("aiagentsapi/v{version:apiVersion}/[controller]")]
public sealed class WorkspacesController(IHttpContextAccessor httpContextAccessor, IWorkspacesHandler workspacesHandler) : BaseController(httpContextAccessor)
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
    public async Task<ResponseDTO> GetAllWorkspacesAsync()
    {
        if (base.IsRequestAuthorized())
        {
            var result = await workspacesHandler.GetAllWorkspacesAsync(base.UserEmail).ConfigureAwait(false);
            if (result is not null) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
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
    public async Task<ResponseDTO> GetWorkspaceByWorkspaceIdAsync([FromRoute] string workspaceId)
    {
        ArgumentException.ThrowIfNullOrEmpty(workspaceId);
        if (base.IsRequestAuthorized())
        {
            var result = await workspacesHandler.GetWorkspaceByWorkspaceIdAsync(workspaceId, base.UserEmail).ConfigureAwait(false);
            if (result is not null) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }

    /// <summary>
    /// Creates a new workspace.
    /// </summary>
    /// <param name="agentsWorkspaceData">The workspace data dto model.</param>
    /// <returns>A boolean for success/failure.</returns>
    [HttpPost(WorkspacesRoutes.AddNewWorkspace_Route)]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = CreateNewWorkspaceAction.Summary, Description = CreateNewWorkspaceAction.Description, OperationId = CreateNewWorkspaceAction.OperationId)]
    public async Task<ResponseDTO> CreateNewWorkspaceAsync([FromForm] AgentsWorkspaceDTO agentsWorkspaceData)
    {
        ArgumentNullException.ThrowIfNull(agentsWorkspaceData);
        if (base.IsRequestAuthorized())
        {
            var result = await workspacesHandler.CreateNewWorkspaceAsync(agentsWorkspaceData, base.UserEmail).ConfigureAwait(false);
            if (result) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }

    /// <summary>
    /// Deletes an existing workspace.
    /// </summary>
    /// <param name="workspaceGuidId">The workspace guid id.</param>
    /// <returns>A boolean for success/failure.</returns>
    [HttpPost(WorkspacesRoutes.DeleteExistingWorkspace_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = DeleteExistingWorkspaceAction.Summary, Description = DeleteExistingWorkspaceAction.Description, OperationId = DeleteExistingWorkspaceAction.OperationId)]
    public async Task<ResponseDTO> DeleteExistingWorkspaceAsync([FromRoute] string workspaceGuidId)
    {
        ArgumentException.ThrowIfNullOrEmpty(workspaceGuidId);
        if (base.IsRequestAuthorized())
        {
            var result = await workspacesHandler.DeleteExistingWorkspaceAsync(workspaceGuidId, base.UserEmail).ConfigureAwait(false);
            if (result) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }

    /// <summary>
    /// Updates an existing workspace.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data dto model.</param>
    /// <returns>The boolean for success/failure.</returns>
    [HttpPost(WorkspacesRoutes.UpdateExistingWorkspace_Route)]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = UpdateExistingWorkspaceDataAction.Summary, Description = UpdateExistingWorkspaceDataAction.Description, OperationId = UpdateExistingWorkspaceDataAction.OperationId)]
    public async Task<ResponseDTO> UpdateExistingWorkspaceDataAsync([FromForm] AgentsWorkspaceDTO agentsWorkspaceData)
    {
        ArgumentNullException.ThrowIfNull(agentsWorkspaceData);
        if (base.IsRequestAuthorized())
        {
            var result = await workspacesHandler.UpdateExistingWorkspaceDataAsync(agentsWorkspaceData, base.UserEmail).ConfigureAwait(false);
            if (result) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }
}
