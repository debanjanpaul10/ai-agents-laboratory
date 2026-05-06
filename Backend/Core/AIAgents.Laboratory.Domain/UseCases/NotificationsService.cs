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
/// Provides an implementation of the INotificationsService interface, responsible for handling notification-related operations by interacting with the IServiceBusNotificationService.
/// </summary>
/// <param name="logger">The <c>ILogger</c> instance is used for logging information, warnings, and errors that occur within the service's methods.</param>
/// <param name="correlationContext">The <c>ICorrelationContext</c> is used to manage correlation IDs for requests, which helps in tracing and correlating logs across different components of the application, especially in distributed systems.</param>
/// <param name="configuration">The configuration service to retrieve the configuration values from environment or application configuration.</param>
/// <param name="serviceBusManager">The <c>IServiceBusManager</c> is an abstraction that encapsulates the business logic for handling service bus notifications.</param>
/// <param name="notificationsDataManager">The <c>INotificationsDataManager</c> is an abstraction for data access operations related to notifications, allowing the service to interact with the underlying data storage without being tightly coupled to a specific implementation.</param>
/// <param name="notificationsStream">The <c>INotificationsStream</c> is an abstraction for managing real-time streaming of notifications, enabling the service to publish notifications to subscribers as they are created or updated.</param>
/// <seealso cref="INotificationsService"/>
public sealed class NotificationsService(
    ILogger<NotificationsService> logger,
    ICorrelationContext correlationContext,
    IConfiguration configuration,
    IServiceBusManager serviceBusManager,
    INotificationsDataManager notificationsDataManager,
    INotificationsStream notificationsStream) : INotificationsService
{
    /// <summary>
    /// The heart beat time delay in seconds.
    /// </summary>
    private readonly int HEART_BEAT_TIME_DELAY_IN_SECONDS = Convert.ToInt32(configuration[AzureAppConfigurationConstants.HeartbeatTimeDelayInSecondsConfig]);

    /// <inheritdoc />
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
            request.DateCreated = DateTime.UtcNow;
            request.IsActive = true;

            var pushNotificationsQueue = configuration[AzureAppConfigurationConstants.PushNotificationsQueueName]
                ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);
            response = await serviceBusManager.SendQueueMessageAsync(
                payload: request,
                queueName: pushNotificationsQueue,
                cancellationToken
            ).ConfigureAwait(false);
            if (response)
                notificationsStream.Publish(
                    recipientUserName: request.RecipientUserName,
                    notification: request
                );

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

    /// <inheritdoc />
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

    /// <inheritdoc />
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
                nameof(GetNotificationsForUserAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, recipientUserName })
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
                nameof(GetNotificationsForUserAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, recipientUserName, response })
            );
        }
    }

    /// <inheritdoc />
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
                nameof(MarkExistingNotificationAsReadAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, notificationId })
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
                nameof(MarkExistingNotificationAsReadAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser, notificationId, response })
            );
        }
    }

    /// <inheritdoc />
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

            using var subscription = notificationsStream.Subscribe(recipientUserName);
            var reader = subscription.Reader;

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

            Task<bool>? tick = null;
            while (!ct.IsCancellationRequested)
            {
                var waitForData = reader.WaitToReadAsync(cancellationToken: ct).AsTask();
                tick ??= heartbeat.WaitForNextTickAsync(cancellationToken: ct).AsTask();

                var completed = await Task.WhenAny(
                    waitForData, tick
                ).ConfigureAwait(false);

                if (completed.IsCanceled)
                    continue;

                if (completed == tick)
                {
                    tick = null;
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
            logger.LogAppWarning(
                LoggingConstants.LogHelperMethodEnd,
                nameof(StreamNotificationsForUserAsync), DateTime.UtcNow, ExceptionConstants.NotificationsStreamCancelledExceptionMessage
            );
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
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
