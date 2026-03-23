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
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.ConfigurationController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// Provides API endpoints for retrieving AI agent configurations, submitting bug reports and feature requests, and obtaining top active agent data within the application.
/// </summary>
/// <remarks>This controller is versioned and secured, requiring proper authorization for most endpoints. It centralizes AI agent-related operations and feedback submission, supporting integration with external clients and services.</remarks>
/// <param name="httpContextAccessor">The HTTP context accessor used to access the current HTTP context for request-specific information.</param>
/// <param name="configuration">The application configuration instance used to access configuration settings.</param>
/// <param name="logger">The logger instance used for logging information, warnings, and errors related to controller operations.</param>
/// <param name="correlationContext">The correlation context used to track and correlate requests across distributed components.</param>
/// <param name="commonAiHandler">The handler responsible for managing common AI agent operations such as retrieving configuration and agent data.</param>
/// <param name="feedbackHandler">The handler responsible for processing feedback operations, including bug reports and feature requests.</param>
/// <seealso cref="BaseController"/>
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV2)]
[Route("aiagentsapi/v{version:apiVersion}/[controller]")]
public sealed class AIAgentsLabController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILogger<AIAgentsLabController> logger, ICorrelationContext correlationContext,
    ICommonAiHandler commonAiHandler, IFeedbackHandler feedbackHandler) : BaseController(httpContextAccessor, configuration)
{
    /// <summary>
    /// Gets the configurations data for application.
    /// </summary>
    /// <returns>The dictionary containing the key-value pair.</returns>
    [HttpGet(CommonRoutes.GetConfigurations_Route)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetConfigurationsDataAction.Summary, Description = GetConfigurationsDataAction.Description, OperationId = GetConfigurationsDataAction.OperationId)]
    public ResponseDto GetConfigurationsData()
    {
        Dictionary<string, string> result = [];
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetConfigurationsData), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail }));
            if (base.IsAuthorized(UserBased))
            {
                result = commonAiHandler.GetConfigurationsData(base.UserEmail);
                if (result is not null && result.Count > 0)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetConfigurationsData), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetConfigurationsData), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Gets the configuration by key name.
    /// </summary>
    /// <param name="configKey">The configuration key name.</param>
    /// <returns>The dicitionary containing the key and value.</returns>
    [HttpGet(CommonRoutes.GetConfigurationByKey_Route)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetConfigurationsDataAction.Summary, Description = GetConfigurationsDataAction.Description, OperationId = GetConfigurationsDataAction.OperationId)]
    public ResponseDto GetConfigurationByKeyName([FromQuery] string configKey)
    {
        Dictionary<string, string> result = [];
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetConfigurationByKeyName), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, configKey }));

            ArgumentException.ThrowIfNullOrEmpty(configKey);
            if (base.IsAuthorized(UserBased))
            {
                result = commonAiHandler.GetConfigurationByKeyName(key: configKey);
                if (result is not null && result.Count > 0)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetConfigurationByKeyName), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetConfigurationByKeyName), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, configKey, result }));
        }
    }

    /// <summary>
    /// Adds the bug report data asynchronous.
    /// </summary>
    /// <param name="addBugReport">The input dto for add new bug data.</param>
    /// <returns>The boolean for success/failure.</returns>
    [HttpPost(CommonRoutes.AddBugReport_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = AddBugReportDataAction.Summary, Description = AddBugReportDataAction.Description, OperationId = AddBugReportDataAction.OperationId)]
    public async Task<ResponseDto> AddBugReportDataAsync([FromBody] AddBugReportDTO addBugReport)
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(AddBugReportDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, addBugReport }));

            ArgumentNullException.ThrowIfNull(addBugReport);
            if (base.IsAuthorized(UserBased))
            {
                addBugReport.CreatedBy = base.UserEmail;
                result = await feedbackHandler.AddNewBugReportDataAsync(bugReportData: addBugReport).ConfigureAwait(false);
                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(AddBugReportDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(AddBugReportDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Submit the feature request data asynchronous.
    /// </summary>
    /// <param name="newFeatureRequest">The new feature request data dto.</param>
    /// <returns>The boolean for success/failure.</returns>
    [HttpPost(CommonRoutes.SubmitFeatureRequest_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = SubmitFeatureRequestDataAction.Summary, Description = SubmitFeatureRequestDataAction.Description, OperationId = SubmitFeatureRequestDataAction.OperationId)]
    public async Task<ResponseDto> SubmitFeatureRequestDataAsync([FromBody] NewFeatureRequestDTO newFeatureRequest)
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(SubmitFeatureRequestDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, newFeatureRequest }));

            ArgumentNullException.ThrowIfNull(newFeatureRequest);
            if (base.IsAuthorized(UserBased))
            {
                newFeatureRequest.CreatedBy = base.UserEmail;
                result = await feedbackHandler.AddNewFeatureRequestDataAsync(featureRequestData: newFeatureRequest).ConfigureAwait(false);
                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(SubmitFeatureRequestDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(SubmitFeatureRequestDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Gets the list of top active agents.
    /// </summary>
    /// <returns>The top active agents data DTO.</returns>
    [HttpGet(CommonRoutes.GetTopActiveAgents_Route)]
    [ProducesResponseType(typeof(TopActiveAgentsDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ResponseDto> GetTopActiveAgentsDataAsync()
    {
        TopActiveAgentsDTO result = new();
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetTopActiveAgentsDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail }));
            if (base.IsAuthorized(UserBased))
            {
                result = await commonAiHandler.GetTopActiveAgentsDataAsync(base.UserEmail).ConfigureAwait(false);
                if (result is not null)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status500InternalServerError, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetTopActiveAgentsDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetTopActiveAgentsDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }
}
