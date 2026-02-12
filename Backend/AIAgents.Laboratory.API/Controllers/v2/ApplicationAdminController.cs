using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Base;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.AuthorizationTypes;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.RouteConstants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.ApplicationAdminController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// The <c>ApplicationAdminController</c> class is an API controller responsible for handling application administration related endpoints,
/// such as retrieving submitted feature requests. 
/// <remarks>It utilizes the <see cref="IApplicationAdminHandler"/> to perform the necessary operations and returns appropriate HTTP responses based on the outcome of the requests.</remarks>
/// </summary>
/// <param name="httpContextAccessor">The http context accessor.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="applicationAdminHandler">The application admin api adapter handler.</param>
/// <seealso cref="BaseController"/>
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV2)]
[Route("aiagentsapi/v{version:apiVersion}/[controller]")]
public sealed class ApplicationAdminController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IApplicationAdminHandler applicationAdminHandler) : BaseController(httpContextAccessor, configuration)
{
    /// <summary>
    /// Gets all submitted feature requests asynchronous.
    /// </summary>
    /// <returns>The list of <see cref="NewFeatureRequestDataDto"/></returns>
    [HttpGet(ApplicationAdminRoutes.GetAllSubmittedFeatureRequests_Route)]
    [ProducesResponseType(typeof(IEnumerable<NewFeatureRequestDataDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetAllSubmittedFeatureRequestsAction.Summary, Description = GetAllSubmittedFeatureRequestsAction.Description, OperationId = GetAllSubmittedFeatureRequestsAction.OperationId)]
    public async Task<ResponseDTO> GetAllSubmittedFeatureRequestsAsync()
    {
        if (base.IsAuthorized(UserBased))
        {
            var result = await applicationAdminHandler.GetAllSubmittedFeatureRequestsAsync(base.UserEmail).ConfigureAwait(false);
            if (result is not null) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }

    /// <summary>
    /// Gets all bug reports data asynchronous.
    /// </summary>
    /// <returns>The list of <see cref="BugReportDataDto"/></returns>
    [HttpGet(ApplicationAdminRoutes.GetAllReportedBugs_Route)]
    [ProducesResponseType(typeof(IEnumerable<BugReportDataDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetAllBugReportsDataAction.Summary, Description = GetAllBugReportsDataAction.Description, OperationId = GetAllBugReportsDataAction.OperationId)]
    public async Task<ResponseDTO> GetAllBugReportsDataAsync()
    {
        if (base.IsAuthorized(UserBased))
        {
            var result = await applicationAdminHandler.GetAllBugReportsDataAsync(base.UserEmail).ConfigureAwait(false);
            if (result is not null) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }

    /// <summary>
    /// Checks if the admin access is enabled asynchronous.
    /// </summary>
    /// <returns>The boolean for success/failure.</returns>
    [HttpGet(ApplicationAdminRoutes.IsAdminAccessEnabled_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = IsAdminAccessEnabledAction.Summary, Description = IsAdminAccessEnabledAction.Description, OperationId = IsAdminAccessEnabledAction.OperationId)]
    public ResponseDTO IsAdminAccessEnabledAsync()
    {
        if (base.IsAuthorized(UserBased))
        {
            var result = applicationAdminHandler.IsAdminAccessEnabledAsync(base.UserEmail);
            return HandleSuccessRequestResponse(result);
        }

        return HandleUnAuthorizedRequestResponse();
    }
}
