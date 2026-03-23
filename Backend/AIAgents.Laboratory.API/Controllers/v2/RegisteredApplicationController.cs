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
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.RegisteredApplicationController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// Provides API endpoints for managing registered applications for the authenticated user, including retrieving the list of registered applications, 
/// getting details of a specific application, creating new applications, updating existing applications, and deleting applications.
/// </summary>
/// <remarks>It handles HTTP requests related to registered applications and interacts with the <see cref="IRegisteredApplicationHandler"/> to perform the necessary operations.</remarks>
/// <param name="httpContextAccessor">The http context accessor service.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="logger">The logger service.</param>
/// <param name="correlationContext">The correlation context service for tracking request correlation IDs.</param>
/// <param name="registerAppHandler">The registered application adapter handler.</param>
/// <seealso cref="BaseController"/>
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV2)]
[Route(ApiBaseRoute)]
public sealed class RegisteredApplicationController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
    ILogger<RegisteredApplicationController> logger, ICorrelationContext correlationContext, IRegisteredApplicationHandler registerAppHandler) : BaseController(httpContextAccessor, configuration)
{
    /// <summary>
    /// Retrieves all registered applications for the authenticated user.
    /// </summary>
    /// <remarks>This method requires the user to be authorized. If the user is not authorized, a 401 Unauthorized response is returned. 
    /// If the request is successful but no applications are found, a 400 Bad Request response is returned with an appropriate message.</remarks>
    /// <returns>A <see cref="ResponseDto"/> containing a collection of <see cref="RegisteredApplicationDto"/> objects representing the registered applications. 
    /// Returns a bad request response if the service is unavailable.</returns>
    [HttpGet(RegisteredApplicationRoutes.GetAllRegisteredApplications_Route)]
    [ProducesResponseType(typeof(IEnumerable<RegisteredApplicationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetAllRegisteredApplicationsAction.Summary, Description = GetAllRegisteredApplicationsAction.Description, OperationId = GetAllRegisteredApplicationsAction.OperationId)]
    public async Task<ResponseDto> GetAllRegisteredApplicationsAsync()
    {
        IEnumerable<RegisteredApplicationDto> result = [];
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllRegisteredApplicationsAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail }));

            if (base.IsAuthorized(UserBased))
            {
                result = await registerAppHandler.GetRegisteredApplicationsAsync(base.UserEmail).ConfigureAwait(false);
                if (result is not null)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllRegisteredApplicationsAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllRegisteredApplicationsAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Registers a new application asynchronously and returns the result of the registration operation.
    /// </summary>
    /// <remarks>The caller must be authorized to perform this operation. If authorization fails, an unauthorized response is returned. 
    /// If registration fails due to service issues or invalid input, a bad request response is provided.</remarks>
    /// <param name="newApplicationDtoModel">The details of the application to be registered. Must contain all required information for registration.</param>
    /// <returns>A ResponseDto indicating whether the registration was successful. Returns a success response if the application is registered; otherwise, returns a bad request or unauthorized response.</returns>
    [HttpPost(RegisteredApplicationRoutes.RegisterNewApplication_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = RegisterNewApplicationAction.Summary, Description = RegisterNewApplicationAction.Description, OperationId = RegisterNewApplicationAction.OperationId)]
    public async Task<ResponseDto> RegisterNewApplicationAsync([FromBody] RegisteredApplicationDto newApplicationDtoModel)
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(RegisterNewApplicationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, newApplicationDtoModel }));

            ArgumentNullException.ThrowIfNull(newApplicationDtoModel);
            if (base.IsAuthorized(UserBased))
            {
                result = await registerAppHandler.CreateNewRegisteredApplicationAsync(base.UserEmail, newApplicationDtoModel).ConfigureAwait(false);
                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(RegisterNewApplicationAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(RegisterNewApplicationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Retrieves the details of a registered application identified by the specified application ID.
    /// </summary>
    /// <remarks>This method requires the caller to be authorized. If the user is not authorized, a 401 Unauthorized response is returned. 
    /// If the application ID is invalid or the application is not found, a 400 Bad Request or 404 Not Found response is returned.</remarks>
    /// <param name="applicationId">The unique identifier of the registered application to retrieve. Must be a positive integer.</param>
    /// <returns>A ResponseDto containing the registered application's details if found; otherwise, an error response indicating unauthorized access, invalid input, or that the application was not found.</returns>
    [HttpGet(RegisteredApplicationRoutes.GetRegisteredApplicationById_Route)]
    [ProducesResponseType(typeof(RegisteredApplicationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = GetRegisteredApplicationByIdAction.Summary, Description = GetRegisteredApplicationByIdAction.Description, OperationId = GetRegisteredApplicationByIdAction.OperationId)]
    public async Task<ResponseDto> GetRegisteredApplicationByIdAsync([FromRoute] int applicationId)
    {
        RegisteredApplicationDto result = new();
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, applicationId }));

            if (base.IsAuthorized(UserBased))
            {
                result = await registerAppHandler.GetRegisteredApplicationByIdAsync(base.UserEmail, applicationId).ConfigureAwait(false);
                if (result is not null)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetRegisteredApplicationByIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, applicationId, result }));
        }
    }

    /// <summary>
    /// Updates an existing registered application with the specified data transfer object.
    /// </summary>
    /// <remarks>This method requires the user to be authorized. If the user is not authorized, a 401 Unauthorized response is returned. 
    /// If the update fails due to application service issues, a 400 Bad Request response is returned with an appropriate error message.</remarks>
    /// <param name="updateApplicationDtoModel">The data transfer object containing the updated information for the registered application. This parameter cannot be null.</param>
    /// <returns>A ResponseDto indicating the outcome of the update operation. Returns <see langword="true"/> if the update is successful; otherwise, returns <see langword="false"/>.</returns>
    [HttpPut(RegisteredApplicationRoutes.UpdateExistingRegisteredApplication_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = UpdateExistingRegisteredApplicationDataAction.Summary, Description = UpdateExistingRegisteredApplicationDataAction.Description, OperationId = UpdateExistingRegisteredApplicationDataAction.OperationId)]
    public async Task<ResponseDto> UpdateExistingRegisteredApplicationAsync([FromBody] RegisteredApplicationDto updateApplicationDtoModel)
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, updateApplicationDtoModel }));

            ArgumentNullException.ThrowIfNull(updateApplicationDtoModel);
            if (base.IsAuthorized(UserBased))
            {
                result = await registerAppHandler.UpdateExistingRegisteredApplicationAsync(
                    currentLoggedInUser: base.UserEmail,
                    updateApplicationData: updateApplicationDtoModel).ConfigureAwait(false);

                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(UpdateExistingRegisteredApplicationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, result }));
        }
    }

    /// <summary>
    /// Deletes a registered application identified by its unique application ID.
    /// </summary>
    /// <remarks>This method requires the caller to be authorized. If the user is not authorized, a 401 Unauthorized response is returned. 
    /// If the application ID is invalid or the deletion fails, a 400 Bad Request response is returned.</remarks>
    /// <param name="applicationId">The unique identifier of the application to be deleted. Must be a valid positive integer.</param>
    /// <returns>A ResponseDto indicating the success or failure of the deletion operation. Returns true if the application was successfully deleted; otherwise, returns false.</returns>
    [HttpDelete(RegisteredApplicationRoutes.DeleteRegisteredApplicationById_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = DeleteExistingRegisteredApplicationAction.Summary, Description = DeleteExistingRegisteredApplicationAction.Description, OperationId = DeleteExistingRegisteredApplicationAction.OperationId)]
    public async Task<ResponseDto> DeleteRegisteredApplicationByIdAsync([FromRoute] int applicationId)
    {
        bool result = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, applicationId }));

            if (base.IsAuthorized(UserBased))
            {
                result = await registerAppHandler.DeleteRegisteredApplicationByIdAsync(base.UserEmail, applicationId).ConfigureAwait(false);
                if (result)
                    return HandleSuccessRequestResponse(result);
                else
                    return HandleBadRequestResponse(StatusCodes.Status400BadRequest, ExceptionConstants.SomethingWentWrongDefaultMessage);
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteRegisteredApplicationByIdAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, applicationId, result }));
        }
    }
}
