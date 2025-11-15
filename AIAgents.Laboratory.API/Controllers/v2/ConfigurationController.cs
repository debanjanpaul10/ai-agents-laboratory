using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.API.Helpers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.ConfigurationController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// The Configuration Controller class.
/// </summary>
/// <param name="httpContextAccessor">The http context accessor.</param>
/// <param name="commonAiHandler">The common ai handler.</param>
/// <seealso cref="BaseController"/>
[ApiController]
[AllowAnonymous]
[ApiVersion(2.0)]
[Route($"{RouteConstants.AiBase_RoutePrefix}/[controller]")]
public class ConfigurationController(IHttpContextAccessor httpContextAccessor, ICommonAiHandler commonAiHandler) : BaseController(httpContextAccessor)
{
    /// <summary>
    /// Gets the configurations data for application.
    /// </summary>
    /// <returns>The dictionary containing the key-value pair.</returns>
    [HttpGet(RouteConstants.CommonRoutes.GetConfigurations_Route)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetConfigurationsDataAction.Summary, Description = GetConfigurationsDataAction.Description, OperationId = GetConfigurationsDataAction.OperationId)]
    public ResponseDTO GetConfigurationsData()
    {
        var result = commonAiHandler.GetConfigurationsData(base.UserEmail);
        if (result is not null && result.Count > 0)
            return base.HandleSuccessRequestResponse(result);

        return base.HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
    }

    /// <summary>
    /// Gets the configurations data for application.
    /// </summary>
    /// <returns>The dictionary containing the key-value pair.</returns>
    [HttpGet(RouteConstants.CommonRoutes.GetConfigurationByKey_Route)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetConfigurationsDataAction.Summary, Description = GetConfigurationsDataAction.Description, OperationId = GetConfigurationsDataAction.OperationId)]
    public ResponseDTO GetConfigurationByKeyName([FromRoute] string configKey)
    {
        var result = commonAiHandler.GetConfigurationByKeyName(configKey);
        if (result is not null && result.Count > 0)
            return base.HandleSuccessRequestResponse(result);

        return base.HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
    }
}
