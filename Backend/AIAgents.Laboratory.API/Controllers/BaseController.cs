using AIAgents.Laboratory.API.Adapters.Models.Base;
using AIAgents.Laboratory.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AIAgents.Laboratory.API.Helpers.Constants;

namespace AIAgents.Laboratory.API.Controllers;

/// <summary>
/// The base controller.
/// </summary>
/// <param name="httpContextAccessor">The http context accessor.</param>
/// <param name="configuration">The configuration.</param>
/// <seealso cref="ControllerBase"/>
[Authorize]
public abstract class BaseController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : ControllerBase
{
    /// <summary>
    /// The user email extracted from the token claims.
    /// </summary>
    protected string UserEmail => httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(claim => claim.Type.Equals(HeaderConstants.UserEmailClaimConstant))?.Value ?? HeaderConstants.NotApplicableStringConstant;

    /// <summary>
    /// Determines whether the request is authorized based on the authorization type.
    /// </summary>
    /// <param name="authorizationType">The authorization type.</param>
    /// <returns>The boolean <c>true</c> if the request is authorized, otherwise <c>false</c>.</returns>
    /// <exception cref="Exception">Thrown when the configuration is missing.</exception>
    protected bool IsAuthorized(AuthorizationTypes authorizationType)
    {
        if (httpContextAccessor.HttpContext is not null && httpContextAccessor.HttpContext?.User is not null)
        {
            // User Authentication
            if (authorizationType == AuthorizationTypes.UserBased && !string.IsNullOrEmpty(this.UserEmail) && !this.UserEmail.Equals(HeaderConstants.NotApplicableStringConstant, StringComparison.OrdinalIgnoreCase))
                return this.CheckApplicationLevelAuthorization();

            // Application Authentication
            if (authorizationType == AuthorizationTypes.ApplicationBased)
                return this.CheckApplicationLevelAuthorization();
        }

        return false;
    }

    /// <summary>
    /// Prepares the success response.
    /// </summary>
    /// <param name="responseData">The response data.</param>
    /// <returns>The response DTO.</returns>
    protected static ResponseDTO HandleSuccessRequestResponse(object responseData) =>
        new()
        {
            IsSuccess = true,
            ResponseData = responseData,
            StatusCode = StatusCodes.Status200OK,
        };

    /// <summary>
    /// Handles the bad request response.
    /// </summary>
    /// <param name="statusCode">The status code.</param>
    /// <param name="message">The message.</param>
    /// <returns>The response DTO.</returns>
    protected static ResponseDTO HandleBadRequestResponse(int statusCode, string message) =>
        new()
        {
            IsSuccess = false,
            ResponseData = message,
            StatusCode = statusCode,
        };

    /// <summary>
    /// Handles the unauthorized request response.
    /// </summary>
    /// <returns>The response DTO.</returns>
    protected static ResponseDTO HandleUnAuthorizedRequestResponse() =>
        new()
        {
            IsSuccess = false,
            ResponseData = ExceptionConstants.UnauthorizedAccessMessageConstant,
            StatusCode = StatusCodes.Status401Unauthorized,
        };

    /// <summary>
    /// Checks the application level authorization.
    /// </summary>
    /// <returns><c>true</c> if the application is authorized, otherwise <c>false</c>.</returns>
    /// <exception cref="Exception">Thrown when the configuration is missing.</exception>
    private bool CheckApplicationLevelAuthorization()
    {
        var currentClientId = this.User?.Claims?.FirstOrDefault(claim => claim.Type.Equals(HeaderConstants.ClientIdClaimConstant))?.Value;
        var aiAgentsClientIdFromConfig = configuration[AzureAppConfigurationConstants.AIAgentsClientIdConstant]
            ?? throw new KeyNotFoundException(configuration[AzureAppConfigurationConstants.AIAgentsClientIdConstant]);
        if (!string.IsNullOrEmpty(currentClientId) && !currentClientId.Equals(HeaderConstants.NotApplicableStringConstant, StringComparison.OrdinalIgnoreCase))
            return currentClientId.Equals(aiAgentsClientIdFromConfig, StringComparison.OrdinalIgnoreCase);

        return false;
    }
}
