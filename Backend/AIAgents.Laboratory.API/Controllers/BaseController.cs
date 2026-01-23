using AIAgents.Laboratory.API.Adapters.Models.Base;
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
    /// The user email.
    /// </summary>
    protected string UserEmail = HeaderConstants.NotApplicableStringConstant;

    /// <summary>
    /// Determines whether the request is authorized.
    /// </summary>
    /// <returns>The boolean <c>true</c> if the request is authorized, otherwise <c>false</c>.</returns>
    protected bool IsAuthorized()
    {
        if (httpContextAccessor.HttpContext is not null && httpContextAccessor.HttpContext?.User is not null)
        {
            // User Authentication
            var userEmail = httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(claim => claim.Type.Equals(HeaderConstants.UserEmailClaimConstant))?.Value;
            if (!string.IsNullOrEmpty(userEmail))
            {
                this.UserEmail = userEmail;
                return true;
            }

            // Application Client ID authentication
            var clientIdClaim = this.User?.Claims?.FirstOrDefault(claim => claim.Type.Equals(HeaderConstants.ClientIdClaimConstant))?.Value;
            if (!string.IsNullOrEmpty(clientIdClaim))
            {
                var aiAgentsClientIdFromConfig = configuration[AzureAppConfigurationConstants.AIAgentsClientIdConstant] ?? string.Empty;
                return clientIdClaim.Equals(aiAgentsClientIdFromConfig, StringComparison.OrdinalIgnoreCase);
            }
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
}
