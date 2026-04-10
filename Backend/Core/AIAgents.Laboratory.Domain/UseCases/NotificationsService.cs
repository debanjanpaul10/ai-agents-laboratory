using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;
using static AIAgents.Laboratory.Domain.Helpers.Constants.NotificationMessagesConstants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides an implementation of the INotificationsService interface, responsible for handling notification-related operations by interacting with the IApplicationNotificationsService.
/// </summary>
/// <param name="logger">The <c>ILogger</c> instance is used for logging information, warnings, and errors that occur within the service's methods.</param>
/// <param name="correlationContext">The <c>ICorrelationContext</c> is used to manage correlation IDs for requests, which helps in tracing and correlating logs across different components of the application, especially in distributed systems.</param>
/// <param name="configuration">The configuration service to retrieve the configuration values from environment or application configuration.</param>
/// <param name="notificationsService">The <c>IApplicationNotificationsService</c> is an abstraction that encapsulates the business logic for handling notifications.</param>
/// <param name="notificationsDataManager">The <c>INotificationsDataManager</c> is an abstraction for data access operations related to notifications, allowing the service to interact with the underlying data storage without being tightly coupled to a specific implementation.</param>
/// <param name="notificationsStream">The <c>INotificationsStream</c> is an abstraction for managing real-time streaming of notifications, enabling the service to publish notifications to subscribers as they are created or updated.</param>
/// <seealso cref="INotificationsService"/>
public sealed class NotificationsService(
    ILogger<NotificationsService> logger,
    ICorrelationContext correlationContext,
    IConfiguration configuration,
    IApplicationNotificationsService notificationsService,
    INotificationsDataManager notificationsDataManager,
    INotificationsStream notificationsStream) : INotificationsService
{
    /// <summary>
    /// The heart beat time delay in seconds.
    /// </summary>
    private readonly int HEART_BEAT_TIME_DELAY_IN_SECONDS = Convert.ToInt32(configuration[AzureAppConfigurationConstants.HeartbeatTimeDelayInSecondsConfig]);

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

            request.Id = request.Id == Guid.Empty ? Guid.NewGuid() : request.Id;
            request.IsRead = false;

            response = await notificationsService.SendNotificationAsync(
                notificationRequest: request,
                cancellationToken
            ).ConfigureAwait(false);
            if (response)
            {
                notificationsStream.Publish(
                    recipientUserName: request.RecipientUserName,
                    notification: request
                );
            }
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
    /// Deletes all notifications for user asynchronous.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A boolean value indicating whether the operation was successful (true) or not (false).
    /// </returns>
    public async Task<bool> DeleteAllNotificationsForUserAsync(
        string currentLoggedInUser,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(DeleteAllNotificationsForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser })
            );

            response = await notificationsDataManager.DeleteAllNotificationsForUserAsync(
                currentLoggedInUser,
                cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(DeleteAllNotificationsForUserAsync), DateTime.UtcNow, ex.Message
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
                nameof(DeleteAllNotificationsForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, response })
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
    /// <param name="currentLoggedInUser">The username of the user for whom to mark the notification as read.</param>
    /// <param name="notificationId">The identifier of the notification to be marked as read.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>A boolean value indicating whether the operation was successful (true) or not (false).</returns>
    public async Task<bool> MarkExistingNotificationAsReadAsync(
        string currentLoggedInUser,
        Guid notificationId,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(MarkExistingNotificationAsReadAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, notificationId })
            );

            response = await notificationsDataManager.MarkExistingNotificationAsReadAsync(
                currentLoggedInUser,
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
                nameof(MarkExistingNotificationAsReadAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, notificationId, response })
            );
        }
    }

    /// <summary>
    /// Streams notifications for a specific user in real-time using Server-Sent Events (SSE).
    /// </summary>
    /// <param name="recipientUserName">The username of the user for whom to stream notifications.</param>
    /// <param name="response">The HttpResponse object used to send the streamed notifications to the client.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <param name="requestAborted">The cancellation token to handle request abortion.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task StreamNotificationsForUserAsync(
        string recipientUserName,
        HttpResponse response,
        CancellationToken cancellationToken = default,
        CancellationToken requestAborted = default
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(recipientUserName);
        ArgumentNullException.ThrowIfNull(response);

        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(StreamNotificationsForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, recipientUserName })
            );

            var reader = notificationsStream.Subscribe(recipientUserName);

            using var heartbeat = new PeriodicTimer(TimeSpan.FromSeconds(HEART_BEAT_TIME_DELAY_IN_SECONDS));
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                requestAborted
            );
            var ct = linkedCts.Token;

            // Send an initial "connected" event so the client can mark the stream healthy.
            await response.WriteAsync(
                text: HeartbeatNotficationConstants.ConnectedMessageConstant,
                cancellationToken: ct
            ).ConfigureAwait(false);
            await response.Body.FlushAsync(cancellationToken: ct).ConfigureAwait(false);

            while (!ct.IsCancellationRequested)
            {
                var waitForData = reader.WaitToReadAsync(cancellationToken: ct).AsTask();
                var tick = heartbeat.WaitForNextTickAsync(cancellationToken: ct).AsTask();

                var completed = await Task.WhenAny(
                    waitForData, tick
                ).ConfigureAwait(false);

                if (completed.IsCanceled)
                    continue;

                if (completed == tick)
                {
                    // Heartbeat keeps proxies/load balancers from buffering/closing the stream.
                    await response.WriteAsync(
                        text: HeartbeatNotficationConstants.EventHeartbeatDataMessageConstant,
                        cancellationToken: ct
                    ).ConfigureAwait(false);
                    await response.Body.FlushAsync(cancellationToken: ct).ConfigureAwait(false);
                    continue;
                }

                if (!await waitForData.ConfigureAwait(false))
                    break;

                while (reader.TryRead(out var notification))
                {
                    var payload = JsonConvert.SerializeObject(notification);
                    await response.WriteAsync(
                        text: string.Format(HeartbeatNotficationConstants.EventNotificationDataMessageConstant, payload),
                        cancellationToken: ct
                    ).ConfigureAwait(false);
                }

                await response.Body.FlushAsync(cancellationToken: ct).ConfigureAwait(false);
            }

            linkedCts.Dispose();
        }
        catch (OperationCanceledException)
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(StreamNotificationsForUserAsync), DateTime.UtcNow, "Notification stream cancelled."
            );
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed, nameof(StreamNotificationsForUserAsync), DateTime.UtcNow, ex.Message
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
                nameof(StreamNotificationsForUserAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, recipientUserName })
            );
        }
    }
}
