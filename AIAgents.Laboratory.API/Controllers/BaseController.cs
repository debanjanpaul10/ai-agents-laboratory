using AIAgents.Laboratory.API.Adapters.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AIAgents.Laboratory.API.Helpers.Constants;

namespace AIAgents.Laboratory.API.Controllers;

/// <summary>
/// The Base Controller Class.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
[Authorize]
public abstract class BaseController : ControllerBase
{
	/// <summary>
	/// The user email
	/// </summary>
	protected string UserEmail = string.Empty;

	/// <summary>
	/// Initializes a new instance of <see cref="BaseController"/>
	/// </summary>
	/// <param name="httpContextAccessor">The http context accessor.</param>
	public BaseController(IHttpContextAccessor httpContextAccessor)
	{
		if (httpContextAccessor.HttpContext is not null && httpContextAccessor.HttpContext?.User is not null)
		{
			var userEmail = httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(claim => claim.Type.Equals(HeaderConstants.UserEmailClaimConstant))?.Value;

			if (!string.IsNullOrEmpty(userEmail)) UserEmail = userEmail;
			else UserEmail = HeaderConstants.NotApplicableStringConstant;
		}
	}

	/// <summary>
	/// Prepares the success response.
	/// </summary>
	/// <param name="responseData">The response data.</param>
	/// <returns>The response DTO.</returns>
	protected ResponseDTO HandleSuccessRequestResponse(object responseData) =>
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
	protected ResponseDTO HandleBadRequestResponse(int statusCode, string message) =>
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
	protected ResponseDTO HandleUnAuthorizedRequestResponse() =>
		new()
		{
			IsSuccess = false,
			ResponseData = ExceptionConstants.UnauthorizedAccessMessageConstant,
			StatusCode = StatusCodes.Status401Unauthorized,
		};

	/// <summary>
	/// Handles request authentication response.
	/// </summary>
	/// <returns>The boolean for authentication.</returns>
	protected bool IsRequestAuthorized() => (!string.IsNullOrEmpty(UserEmail));
}
