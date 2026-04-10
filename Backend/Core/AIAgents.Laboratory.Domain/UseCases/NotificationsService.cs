using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides an implementation of the INotificationsService interface, responsible for handling notification-related operations by interacting with the IApplicationNotificationsService.
/// </summary>
/// <param name="logger">The <c>ILogger</c> instance is used for logging information, warnings, and errors that occur within the service's methods.</param>
/// <param name="correlationContext">The <c>ICorrelationContext</c> is used to manage correlation IDs for requests, which helps in tracing and correlating logs across different components of the application, especially in distributed systems.</param>
/// <param name="notificationsService">The <c>IApplicationNotificationsService</c> is an abstraction that encapsulates the business logic for handling notifications.</param>
/// <param name="notificationsDataManager">The <c>INotificationsDataManager</c> is an abstraction for data access operations related to notifications, allowing the service to interact with the underlying data storage without being tightly coupled to a specific implementation.</param>
/// <seealso cref="INotificationsService"/>
public sealed class NotificationsService(
    ILogger<NotificationsService> logger,
    ICorrelationContext correlationContext,
    IApplicationNotificationsService notificationsService,
    INotificationsDataManager notificationsDataManager) : INotificationsService
{
    /// <summary>
    /// Creates a new notification based on the provided request data.
    /// </summary>
    /// <param name="request">The request object containing the details of the notification to be created.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>True if the notification was created successfully; otherwise, false.</returns>
    public async Task<bool> CreateNewNotificationAsync(
        NotificationsDomain request,
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

            request.Id = Guid.NewGuid();
            response = await notificationsService.SendNotificationAsync(
                notificationRequest: request,
                cancellationToken
            ).ConfigureAwait(false);
            return response;
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
    /// This method allows clients to fetch all notifications that are relevant to a particular user
    /// </summary>
    /// <param name="recipientUserName">The username of the user for whom to retrieve notifications.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>A list of notifications relevant to the specified user.</returns>
    public async Task<IEnumerable<NotificationsDomain>> GetNotificationsForUserAsync(
        string recipientUserName,
        CancellationToken cancellationToken = default
    )
    {
        IEnumerable<NotificationsDomain>? response = null;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetNotificationsForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, recipientUserName })
            );

            response = await notificationsDataManager.GetNotificationsForUserAsync(
                recipientUserName: recipientUserName,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(GetNotificationsForUserAsync), DateTime.UtcNow, ex.Message
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
                nameof(GetNotificationsForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, recipientUserName, response })
            );
        }
    }

    /// <summary>
    /// Marks an existing notification as read for a specific user based on the notification identifier.
    /// </summary>
    /// <remarks>
    /// Marking a notification as read typically involves updating the status of the notification in the data store to indicate that it has been acknowledged or viewed by the recipient user.
    /// </remarks>
    /// <param name="recipientUserName">The username of the user for whom to mark the notification as read.</param>
    /// <param name="notificationId">The identifier of the notification to be marked as read.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>A boolean value indicating whether the operation was successful (true) or not (false).</returns>
    public async Task<bool> MarkExistingNotificationAsReadAsync(
        string recipientUserName,
        Guid notificationId,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(MarkExistingNotificationAsReadAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, recipientUserName, notificationId })
            );

            response = await notificationsDataManager.MarkExistingNotificationAsReadAsync(
                recipientUserName: recipientUserName,
                notificationId: notificationId,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(MarkExistingNotificationAsReadAsync), DateTime.UtcNow, ex.Message
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
                nameof(MarkExistingNotificationAsReadAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, recipientUserName, notificationId, response })
            );
        }
    }
}
