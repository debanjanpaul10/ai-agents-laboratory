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
public class WorkspacesController(IHttpContextAccessor httpContextAccessor, IWorkspacesHandler workspacesHandler) : BaseController(httpContextAccessor)
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
}
