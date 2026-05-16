using System.Security.Claims;
using AIAgents.Laboratory.API.Adapters.Models.Base;
using AIAgents.Laboratory.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AIAgents.Laboratory.API.Helpers.Constants;

namespace AIAgents.Laboratory.API.Controllers;

/// <summary>
/// The <c>BaseController</c> class serves as a base API controller that provides common functionality for handling HTTP requests in the AIAgents Laboratory application.
/// </summary>
/// <remarks>It includes methods for extracting user information from the HTTP context, checking authorization based on user or application level, and preparing standardized responses for successful, bad, or unauthorized requests.</remarks>
/// <param name="httpContextAccessor">The http context accessor.</param>
/// <param name="configuration">The configuration.</param>
/// <seealso cref="ControllerBase"/>
[Authorize]
public abstract class BaseController(
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration) : ControllerBase
{
    /// <summary>
    /// The user email extracted from the token claims.
    /// </summary>
    protected string UserEmail => httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(claim => claim.Type.Equals(HeaderConstants.UserEmailClaimConstant))?.Value
        ?? HeaderConstants.NotApplicableStringConstant;

    /// <summary>
    /// Determines whether the request is authorized based on the authorization type.
    /// </summary>
    /// <param name="authorizationType">The authorization type.</param>
    /// <returns>The boolean <c>true</c> if the request is authorized, otherwise <c>false</c>.</returns>
    /// <exception cref="Exception">Thrown when the configuration is missing.</exception>
    protected bool IsAuthorized(
        AuthorizationTypes authorizationType
    )
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user is null)
            return false;

        return authorizationType switch
        {
            AuthorizationTypes.ApplicationBased => CheckApplicationLevelAuthorization(),
            AuthorizationTypes.UserBased => HasValidUserEmail(user) && CheckApplicationLevelAuthorization(),
            _ => false
        };
    }

    /// <summary>
    /// Prepares the success response.
    /// </summary>
    /// <param name="responseData">The response data.</param>
    /// <returns>The response DTO.</returns>
    protected static ResponseDto HandleSuccessRequestResponse(
        object responseData
    ) =>
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
    protected static ResponseDto HandleBadRequestResponse(
        int statusCode,
        string message
    ) =>
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
    protected static ResponseDto HandleUnAuthorizedRequestResponse() =>
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
        var currentClientId = this.User?.Claims?
            .FirstOrDefault(claim => claim.Type.Equals(HeaderConstants.ClientIdClaimConstant))?.Value;
        var aiAgentsClientIdFromConfig = configuration[AzureAppConfigurationConstants.AIAgentsClientIdConstant]
            ?? throw new KeyNotFoundException(configuration[AzureAppConfigurationConstants.AIAgentsClientIdConstant]);

        if (!string.IsNullOrEmpty(currentClientId) && !currentClientId.Equals(HeaderConstants.NotApplicableStringConstant, StringComparison.OrdinalIgnoreCase))
            return currentClientId.Equals(aiAgentsClientIdFromConfig, StringComparison.OrdinalIgnoreCase);

        return false;
    }

    /// <summary>
    /// Determines whether the user has a valid email claim.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns><c>true</c> if the user has a valid email claim, otherwise <c>false</c>.</returns>
    private static bool HasValidUserEmail(
        ClaimsPrincipal user
    )
    {
        var userEmail = user.Claims
            .FirstOrDefault(claim => claim.Type.Equals(HeaderConstants.UserEmailClaimConstant))?.Value;

        return !string.IsNullOrWhiteSpace(userEmail)
            && !userEmail.Equals(HeaderConstants.NotApplicableStringConstant, StringComparison.OrdinalIgnoreCase);
    }
}
