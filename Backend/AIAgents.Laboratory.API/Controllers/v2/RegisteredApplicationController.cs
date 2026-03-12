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
    /// Retrieves all registered applications for the authenticated user.
    /// </summary>
    /// <remarks>This method requires the user to be authorized. If the user is not authorized, a 401 Unauthorized response is returned. 
    /// If the request is successful but no applications are found, a 400 Bad Request response is returned with an appropriate message.</remarks>
    /// <returns>A <see cref="ResponseDTO"/> containing a collection of <see cref="RegisteredApplicationDto"/> objects representing the registered applications. 
    /// Returns a bad request response if the service is unavailable.</returns>
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
    /// Registers a new application asynchronously and returns the result of the registration operation.
    /// </summary>
    /// <remarks>The caller must be authorized to perform this operation. If authorization fails, an unauthorized response is returned. 
    /// If registration fails due to service issues or invalid input, a bad request response is provided.</remarks>
    /// <param name="newApplicationDtoModel">The details of the application to be registered. Must contain all required information for registration.</param>
    /// <returns>A ResponseDTO indicating whether the registration was successful. Returns a success response if the application is registered; otherwise, returns a bad request or unauthorized response.</returns>
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

    /// <summary>
    /// Retrieves the details of a registered application identified by the specified application ID.
    /// </summary>
    /// <remarks>This method requires the caller to be authorized. If the user is not authorized, a 401 Unauthorized response is returned. 
    /// If the application ID is invalid or the application is not found, a 400 Bad Request or 404 Not Found response is returned.</remarks>
    /// <param name="applicationId">The unique identifier of the registered application to retrieve. Must be a positive integer.</param>
    /// <returns>A ResponseDTO containing the registered application's details if found; otherwise, an error response indicating unauthorized access, invalid input, or that the application was not found.</returns>
    [HttpGet(RegisteredApplicationRoutes.GetRegisteredApplicationById_Route)]
    [ProducesResponseType(typeof(RegisteredApplicationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ResponseDTO> GetRegisteredApplicationByIdAsync([FromQuery] int applicationId)
    {
        if (base.IsAuthorized(UserBased))
        {
            var result = await raHandler.GetRegisteredApplicationByIdAsync(base.UserEmail, applicationId).ConfigureAwait(false);
            if (result is not null) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }

    /// <summary>
    /// Updates an existing registered application with the specified data transfer object.
    /// </summary>
    /// <remarks>This method requires the user to be authorized. If the user is not authorized, a 401 Unauthorized response is returned. 
    /// If the update fails due to application service issues, a 400 Bad Request response is returned with an appropriate error message.</remarks>
    /// <param name="updateApplicationDtoModel">The data transfer object containing the updated information for the registered application. This parameter cannot be null.</param>
    /// <returns>A ResponseDTO indicating the outcome of the update operation. Returns <see langword="true"/> if the update is successful; otherwise, returns <see langword="false"/>.</returns>
    [HttpPut(RegisteredApplicationRoutes.UpdateExistingRegisteredApplication_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ResponseDTO> UpdateExistingRegisteredApplicationAsync([FromBody] RegisteredApplicationDto updateApplicationDtoModel)
    {
        if (base.IsAuthorized(UserBased))
        {
            var result = await raHandler.UpdateExistingRegisteredApplicationAsync(
                currentLoggedInUser: base.UserEmail,
                updateApplicationData: updateApplicationDtoModel).ConfigureAwait(false);

            if (result) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }

    /// <summary>
    /// Deletes a registered application identified by its unique application ID.
    /// </summary>
    /// <remarks>This method requires the caller to be authorized. If the user is not authorized, a 401 Unauthorized response is returned. 
    /// If the application ID is invalid or the deletion fails, a 400 Bad Request response is returned.</remarks>
    /// <param name="applicationId">The unique identifier of the application to be deleted. Must be a valid positive integer.</param>
    /// <returns>A ResponseDTO indicating the success or failure of the deletion operation. Returns true if the application was successfully deleted; otherwise, returns false.</returns>
    [HttpDelete(RegisteredApplicationRoutes.DeleteRegisteredApplicationById_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ResponseDTO> DeleteRegisteredApplicationByIdAsync([FromQuery] int applicationId)
    {
        if (base.IsAuthorized(UserBased))
        {
            var result = await raHandler.DeleteRegisteredApplicationByIdAsync(base.UserEmail, applicationId).ConfigureAwait(false);
            if (result) return HandleSuccessRequestResponse(result);
            else return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.AiServicesDownMessage);
        }

        return HandleUnAuthorizedRequestResponse();
    }
}
