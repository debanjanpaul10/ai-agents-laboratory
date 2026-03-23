using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Base;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.AuthorizationTypes;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.RouteConstants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.ApplicationAdminController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// The <c>ApplicationAdminController</c> class is an API controller responsible for handling application administration related endpoints,
/// such as retrieving submitted feature requests. 
/// </summary>
/// <remarks>It utilizes the <see cref="IApplicationAdminHandler"/> to perform the necessary operations and returns appropriate HTTP responses based on the outcome of the requests.</remarks>
/// <param name="httpContextAccessor">The http context accessor.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="correlationContext">The correlation context for logging and tracing.</param>
/// <param name="logger">The logger instance for logging information and errors.</param>
/// <param name="applicationAdminHandler">The application admin api adapter handler.</param>
/// <seealso cref="BaseController"/>
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV2)]
[Route("aiagentsapi/v{version:apiVersion}/[controller]")]
public sealed class ApplicationAdminController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
    ILogger<ApplicationAdminController> logger, ICorrelationContext correlationContext, IApplicationAdminHandler applicationAdminHandler) : BaseController(httpContextAccessor, configuration)
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
    public async Task<ResponseDto> GetAllSubmittedFeatureRequestsAsync()
    {
        IEnumerable<NewFeatureRequestDataDto> response = [];
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail }));

            if (base.IsAuthorized(UserBased))
            {
                response = await applicationAdminHandler.GetAllSubmittedFeatureRequestsAsync(base.UserEmail).ConfigureAwait(false);
                if (response is not null)
                    return HandleSuccessRequestResponse(response);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, response }));
        }
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
    public async Task<ResponseDto> GetAllBugReportsDataAsync()
    {
        IEnumerable<BugReportDataDto> response = [];
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail }));

            if (base.IsAuthorized(UserBased))
            {
                response = await applicationAdminHandler.GetAllBugReportsDataAsync(base.UserEmail).ConfigureAwait(false);
                if (response is not null)
                    return HandleSuccessRequestResponse(response);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, response }));
        }
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
    public ResponseDto IsAdminAccessEnabledAsync()
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(IsAdminAccessEnabledAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail }));

            if (base.IsAuthorized(UserBased))
            {
                response = applicationAdminHandler.IsAdminAccessEnabledAsync(base.UserEmail);
                return HandleSuccessRequestResponse(response);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(IsAdminAccessEnabledAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(IsAdminAccessEnabledAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, response }));
        }
    }
}
