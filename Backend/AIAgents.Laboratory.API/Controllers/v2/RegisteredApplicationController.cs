using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Base;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using Microsoft.AspNetCore.Mvc;
using static AIAgents.Laboratory.API.Helpers.AuthorizationTypes;
using static AIAgents.Laboratory.API.Helpers.Constants;
using static AIAgents.Laboratory.API.Helpers.RouteConstants;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// Provides API endpoints for managing registered applications for the authenticated user, including retrieving the list of registered applications, 
/// getting details of a specific application, creating new applications, updating existing applications, and deleting applications.
/// </summary>
/// <param name="httpContextAccessor">The http context accessor service.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="raHandler">The registered application adapter handler.</param>
/// <seealso cref="BaseController"/>
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV2)]
[Route("aiagentsapi/v{version:apiVersion}/[controller]")]
public sealed class RegisteredApplicationController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IRegisteredApplicationHandler raHandler) : BaseController(httpContextAccessor, configuration)
{
    /// <summary>
    /// Gets all the registered applications for the authenticated user.
    /// </summary>
    /// <returns>The list of <see cref="RegisteredApplicationDto"/></returns>
    [HttpGet(RegisteredApplicationRoutes.GetAllRegisteredApplications_Route)]
    [ProducesResponseType(typeof(IEnumerable<RegisteredApplicationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ResponseDTO> GetAllRegisteredApplicationsAsync()
    {
        if (base.IsAuthorized(UserBased))
        {
            var result = await raHandler.GetRegisteredApplicationsAsync(base.UserEmail).ConfigureAwait(false);
            if (result is not null) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }

    /// <summary>
    /// Registers a new application for the authenticated user based on the provided <see cref="RegisteredApplicationDto"/> model.
    /// </summary>
    /// <param name="newApplicationDtoModel">The new application dto data model.</param>
    /// <returns>The boolean for success/failure.</returns>
    [HttpPost(RegisteredApplicationRoutes.RegisterNewApplication_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ResponseDTO> RegisterNewApplicationAsync([FromBody] RegisteredApplicationDto newApplicationDtoModel)
    {
        if (base.IsAuthorized(UserBased))
        {
            var result = await raHandler.CreateNewRegisteredApplicationAsync(base.UserEmail, newApplicationDtoModel).ConfigureAwait(false);
            if (result) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }
}
