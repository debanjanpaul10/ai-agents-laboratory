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
using static AIAgents.Laboratory.API.Helpers.SwaggerConstants.NotificationsController;

namespace AIAgents.Laboratory.API.Controllers.v2;

/// <summary>
/// The NotificationsController class is an API controller responsible for handling HTTP requests related to notifications in the AIAgents Laboratory application. 
/// </summary>
/// <remarks>
/// It provides endpoints for creating new notifications based on incoming requests. 
/// The controller interacts with the INotificationsHandler to process the business logic of notification creation, ensuring that the appropriate responses are returned to the client based on the success or failure of the operations. 
/// This controller is versioned as part of the API versioning strategy and includes proper logging and error handling mechanisms to maintain robustness and traceability of operations.
/// </remarks>
/// <param name="httpContextAccessor">The IHttpContextAccessor is used to access the current HTTP context, allowing the controller to retrieve information about the incoming request.</param>
/// <param name="configuration">The IConfiguration interface is used to access the application's configuration settings, enabling the controller to retrieve necessary configuration values that may influence the behavior of the notification handling logic.</param>
/// <param name="logger">The ILogger instance is used for logging information, warnings, and errors that occur within the controller's actions. This helps in monitoring the application's behavior and diagnosing issues when they arise.</param>
/// <param name="correlationContext">The ICorrelationContext is used to manage correlation IDs for requests, which helps in tracing and correlating logs across different components of the application, especially in distributed systems.</param>
/// <param name="notificationsHandler">The INotificationsHandler is an abstraction that encapsulates the business logic for handling notifications.</param>
/// <seealso cref="BaseController"/>
[ApiController]
[ApiVersion(ApiVersionsConstants.ApiVersionV2)]
[Route(ApiBaseRoute)]
public sealed class NotificationsController(
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration,
    ILogger<NotificationsController> logger,
    ICorrelationContext correlationContext,
    INotificationsHandler notificationsHandler) : BaseController(httpContextAccessor, configuration)
{
    /// <summary>
    /// Creates a new notification based on the provided request data. 
    /// </summary>
    /// <remarks>
    /// This endpoint accepts a POST request with the necessary information to create a notification, such as the title, message, recipient username, notification type, and the creator's information. 
    /// The method checks for authorization, processes the request through the notifications handler, and returns an appropriate response indicating the success or failure of the operation.
    /// </remarks>
    /// <param name="request">The CreateNotificationRequestDto contains the details required to create a new notification, including the title, message, recipient username, notification type, and the creator's information.</param>
    /// <param name="cancellationToken">The CancellationToken allows the operation to be cancelled, providing a way to gracefully handle request cancellations and free up resources when the client aborts the request.</param>
    /// <returns>The method returns a ResponseDto indicating whether the notification was successfully created. A successful response includes a boolean value of true, while a failure results in an appropriate error message and status code.</returns>
    [HttpPost(NotificationsRoutes.CreateNewNotification_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = CreateNewNotificationAction.Summary,
        Description = CreateNewNotificationAction.Description,
        OperationId = CreateNewNotificationAction.OperationId)]
    public async Task<ResponseDto> CreateNewNotificationAsync(
        [FromBody] CreateNotificationRequestDto request,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(CreateNewNotificationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, request })
            );

            ArgumentNullException.ThrowIfNull(request);
            if (base.IsAuthorized(authorizationType: ApplicationBased))
            {
                response = await notificationsHandler.CreateNewNotificationAsync(
                    request,
                    cancellationToken
                ).ConfigureAwait(false);

                if (response)
                    return HandleSuccessRequestResponse(
                        responseData: response
                    );
                else
                    return HandleBadRequestResponse(
                        statusCode: StatusCodes.Status400BadRequest,
                        message: ExceptionConstants.SomethingWentWrongDefaultMessage
                    );
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(CreateNewNotificationAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(CreateNewNotificationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, request, response })
            );
        }
    }

    /// <summary>
    /// Retrieves a list of notifications for a specific user based on their username. 
    /// This method allows clients to fetch all notifications that are relevant to a particular user.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>A list of notifications relevant to the specified user.</returns>
    [HttpGet(NotificationsRoutes.PollNotificationsForUser_Route)]
    [ProducesResponseType(typeof(IEnumerable<NotificationsResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = PollNotificationsForUserAction.Summary,
        Description = PollNotificationsForUserAction.Description,
        OperationId = PollNotificationsForUserAction.OperationId)]
    public async Task<ResponseDto> PollNotificationsForUserAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<NotificationsResponseDto> response = [];
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(PollNotificationsForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail })
            );

            ArgumentException.ThrowIfNullOrWhiteSpace(base.UserEmail);
            if (base.IsAuthorized(authorizationType: UserBased))
            {
                response = await notificationsHandler.GetNotificationsForUserAsync(
                    recipientUserName: base.UserEmail,
                    cancellationToken
                ).ConfigureAwait(false);

                if (response is not null)
                    return HandleSuccessRequestResponse(
                        responseData: response
                    );
                else
                    return HandleBadRequestResponse(
                        statusCode: StatusCodes.Status400BadRequest,
                        message: ExceptionConstants.SomethingWentWrongDefaultMessage
                    );
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed, nameof(PollNotificationsForUserAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(PollNotificationsForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, response })
            );
        }
    }

    /// <summary>
    /// Marks an existing notification as read for a specific user based on the notification identifier.
    /// </summary>
    /// <param name="notificationId">The identifier of the notification to be marked as read.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>A boolean value indicating whether the operation was successful (true) or not (false).</returns>
    [HttpPost(NotificationsRoutes.MarkExistingNotificationAsRead_Route)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = MarkExistingNotificationAsReadAction.Summary,
        Description = MarkExistingNotificationAsReadAction.Description,
        OperationId = MarkExistingNotificationAsReadAction.OperationId)]
    public async Task<ResponseDto> MarkExistingNotificationAsReadAsync(
        [FromQuery] Guid notificationId,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(MarkExistingNotificationAsReadAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, notificationId })
            );

            ArgumentException.ThrowIfNullOrWhiteSpace(base.UserEmail);
            if (base.IsAuthorized(authorizationType: UserBased))
            {
                response = await notificationsHandler.MarkExistingNotificationAsReadAsync(
                    recipientUserName: base.UserEmail,
                    notificationId: notificationId,
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);

                if (response)
                    return HandleSuccessRequestResponse(
                        responseData: response
                    );
                else
                    return HandleBadRequestResponse(
                        statusCode: StatusCodes.Status400BadRequest,
                        message: ExceptionConstants.SomethingWentWrongDefaultMessage
                    );
            }

            return HandleUnAuthorizedRequestResponse();
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed, nameof(MarkExistingNotificationAsReadAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(MarkExistingNotificationAsReadAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, base.UserEmail, notificationId, response })
            );
        }
    }
}
