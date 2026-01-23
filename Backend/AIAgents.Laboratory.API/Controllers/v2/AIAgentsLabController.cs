using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Base;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.RouteConstants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.ConfigurationController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// The AI Agents  Controller class.
/// </summary>
/// <param name="httpContextAccessor">The http context accessor.</param>
/// <param name="configuration">The configuration.</param>
/// <param name="commonAiHandler">The common ai handler.</param>
/// <param name="feedbackHandler">The feedback api handler.</param>
/// <seealso cref="BaseController"/>
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV2)]
[Route("aiagentsapi/v{version:apiVersion}/[controller]")]
public sealed class AIAgentsLabController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ICommonAiHandler commonAiHandler, IFeedbackHandler feedbackHandler) : BaseController(httpContextAccessor, configuration)
{
    /// <summary>
    /// Gets the configurations data for application.
    /// </summary>
    /// <returns>The dictionary containing the key-value pair.</returns>
    [AllowAnonymous]
    [HttpGet(CommonRoutes.GetConfigurations_Route)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetConfigurationsDataAction.Summary, Description = GetConfigurationsDataAction.Description, OperationId = GetConfigurationsDataAction.OperationId)]
    public ResponseDTO GetConfigurationsData()
    {
        if (base.IsAuthorized())
        {
            var result = commonAiHandler.GetConfigurationsData(base.UserEmail);
            if (result is not null && result.Count > 0) return HandleSuccessRequestResponse(result);

            return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
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
    public ResponseDTO GetConfigurationByKeyName([FromRoute] string configKey)
    {
        ArgumentException.ThrowIfNullOrEmpty(configKey, ExceptionConstants.MissingConfigurationMessage);
        if (base.IsAuthorized())
        {
            var result = commonAiHandler.GetConfigurationByKeyName(configKey);
            if (result is not null && result.Count > 0) return HandleSuccessRequestResponse(result);

            return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
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
    public async Task<ResponseDTO> AddBugReportDataAsync([FromBody] AddBugReportDTO addBugReport)
    {
        ArgumentNullException.ThrowIfNull(addBugReport, ExceptionConstants.InvalidBugReportDataMessage);
        if (base.IsAuthorized())
        {
            addBugReport.CreatedBy = base.UserEmail;
            var result = await feedbackHandler.AddNewBugReportDataAsync(addBugReport).ConfigureAwait(false);
            if (result) return HandleSuccessRequestResponse(result);

            return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
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
    public async Task<ResponseDTO> SubmitFeatureRequestDataAsync([FromBody] NewFeatureRequestDTO newFeatureRequest)
    {
        ArgumentNullException.ThrowIfNull(newFeatureRequest, ExceptionConstants.InvalidFeatureRequestDataMessage);
        if (base.IsAuthorized())
        {
            newFeatureRequest.CreatedBy = base.UserEmail;
            var result = await feedbackHandler.AddNewFeatureRequestDataAsync(newFeatureRequest).ConfigureAwait(false);
            if (result) return HandleSuccessRequestResponse(result);

            return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
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
    public async Task<ResponseDTO> GetTopActiveAgentsDataAsync()
    {
        if (base.IsAuthorized())
        {
            var result = await commonAiHandler.GetTopActiveAgentsDataAsync(base.UserEmail).ConfigureAwait(false);
            if (result is not null) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status500InternalServerError, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }
}
