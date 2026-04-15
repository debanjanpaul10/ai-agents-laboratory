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
/// Provides functionality for sending email notifications using the specified configuration and logging context.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="correlationContext">The correlation context for logging.</param>
/// <param name="serviceBusManager">The service bus manager service.</param>
/// <seealso cref="IServiceBusNotificationService"/>
public sealed class EmailNotificationService(
    ILogger<EmailNotificationService> logger,
    IConfiguration configuration,
    ICorrelationContext correlationContext,
    IServiceBusManager serviceBusManager) : IServiceBusNotificationService
{
    /// <summary>
    /// Sends a notification asynchronously based on the provided notification request domain entity.
    /// </summary>
    /// <param name="notificationRequest">The notification request domain entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> SendNotificationAsync(
        NotificationsDomain notificationRequest,
        CancellationToken cancellationToken = default)
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(SendNotificationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, notificationRequest })
            );

            var emailNotificationsQueue = configuration[AzureAppConfigurationConstants.EmailNotificationsQueueName]
                ?? throw new KeyNotFoundException(ExceptionMessagesConstants.ConfigurationMissingExceptionMessage);
            await serviceBusManager.SendQueueMessageAsync(
                payload: notificationRequest,
                queueName: emailNotificationsQueue,
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
