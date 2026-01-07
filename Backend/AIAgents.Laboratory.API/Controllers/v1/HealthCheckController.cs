using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.RouteConstants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.HealthCheckController;

namespace AIAgents.Laboratory.API.Controllers.v1;

/// <summary>
/// The Health Check Endpoint API controller.
/// </summary>
/// <param name="commonAiHandler">The common AI handler.</param>
/// <param name="httpContextAccessor">The http context accessor.</param>
/// <seealso cref="BaseController" />
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV1)]
[Route("aiagentsapi/v{version:apiVersion}/[controller]")]
public class HealthCheckController(IHttpContextAccessor httpContextAccessor, ICommonAiHandler commonAiHandler) : BaseController(httpContextAccessor)
{
    /// <summary>
    /// Gets the agent status.
    /// </summary>
    /// <returns>The agent status data dto.</returns>
    [HttpGet(HealthCheckRoutes.GetAgentStatus_Route)]
    [ProducesResponseType(typeof(AgentStatusDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetAgentStatusAction.Summary, Description = GetAgentStatusAction.Description, OperationId = GetAgentStatusAction.OperationId)]
    public IActionResult GetAgentStatus()
    {
        return base.Ok(commonAiHandler.GetAgentCurrentStatus());
    }
}
