using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Messaging.Adapters.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Messaging.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.Messaging.Adapters.Services;

/// <summary>
/// Provides functionality for sending application push notifications using the specified configuration and logging context.
/// </summary>
/// <param name="logger">The logger used to record informational and error messages related to notification operations.</param>
/// <param name="correlationContext">The correlation context used to track and correlate notification operations across system boundaries.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="serviceBusManager">The service bus manager used to send messages to the appropriate Azure Service Bus queue for processing push notifications.</param>
/// <seealso cref="IApplicationNotificationsService"/>
public sealed class AppPushNotificationService(
    ILogger<AppPushNotificationService> logger,
    ICorrelationContext correlationContext,
    IConfiguration configuration,
    IServiceBusManager serviceBusManager) : IApplicationNotificationsService
{
    /// <summary>
    /// Sends a notification asynchronously based on the provided notification request domain entity.
    /// </summary>
    /// <param name="notificationRequest">The notification request domain entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> SendNotificationAsync(
        NotificationsDomain notificationRequest,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(SendNotificationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, notificationRequest })
            );

            var pushNotificationsQueue = configuration[AzureAppConfigurationConstants.PushNotificationsQueueName]
                ?? throw new KeyNotFoundException(ExceptionMessagesConstants.ConfigurationMissingExceptionMessage);
            await serviceBusManager.SendQueueMessageAsync(
                payload: notificationRequest,
                queueName: pushNotificationsQueue,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            response = true;
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(SendNotificationAsync), DateTime.UtcNow, ex.Message
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
                nameof(SendNotificationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, notificationRequest, response })
            );
        }
    }
}