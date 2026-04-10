using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Messaging.Adapters.Contracts;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Messaging.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.Messaging.Adapters.Services;

/// <summary>
/// Provides functionality for managing interactions with Azure Service Bus, including sending messages to queues. 
/// This implementation of the <see cref="IServiceBusManager"/> interface allows for decoupling the message sending logic from the underlying implementation, enabling easier testing and flexibility in how messages are sent to Azure Service Bus.
/// </summary>
/// <param name="serviceBusClient">The Azure Service Bus client used to create senders and send messages to the specified queues.</param>
/// <param name="configuration">The configuration used to retrieve necessary settings such as the default queue name for sending messages.</param>
/// <param name="logger">The logger used to record informational and error messages related to Service Bus operations, including message sending attempts and any exceptions that may occur during the process.</param>
/// <param name="correlationContext">The correlation context used to track and correlate Service Bus operations across system boundaries, allowing for better observability and troubleshooting of message sending activities.</param>
/// <seealso cref="IServiceBusManager"/>
public sealed class ServiceBusManager(
    ServiceBusClient serviceBusClient,
    IConfiguration configuration,
    ILogger<ServiceBusManager> logger,
    ICorrelationContext correlationContext) : IServiceBusManager
{
    /// <summary>
    /// The default queue name to be used for sending messages when no specific queue name is provided. 
    /// This value is retrieved from the configuration using the key defined in <see cref="AzureAppConfigurationConstants.PushNotificationsQueueName"/>. 
    /// If the configuration value is missing or empty, a <see cref="KeyNotFoundException"/> is thrown to indicate that the necessary configuration is not available for the Service Bus operations to function correctly.
    /// </summary>
    private readonly string DEFAULT_QUEUE_NAME =
        configuration[AzureAppConfigurationConstants.PushNotificationsQueueName]
        ?? throw new KeyNotFoundException(ExceptionMessagesConstants.ConfigurationMissingExceptionMessage);

    /// <summary>
    /// Sends a message to an Azure Service Bus queue.
    /// </summary>
    /// <typeparam name="T">The payload type.</typeparam>
    /// <param name="payload">The payload to serialize and send.</param>
    /// <param name="queueName">Optional queue override; if null/empty default queue is used.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task SendQueueMessageAsync<T>(
        T payload,
        string? queueName = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        var resolvedQueueName = string.IsNullOrWhiteSpace(queueName) ? DEFAULT_QUEUE_NAME : queueName;

        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(SendQueueMessageAsync),
                DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, resolvedQueueName }));

            var sender = serviceBusClient.CreateSender(resolvedQueueName);
            var body = JsonConvert.SerializeObject(payload);
            var message = new ServiceBusMessage(body)
            {
                ContentType = AzureAppConfigurationConstants.ApplicationJsonConstant,
                CorrelationId = correlationContext.CorrelationId
            };

            await sender.SendMessageAsync(
                message,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(SendQueueMessageAsync), DateTime.UtcNow, ex.Message);

            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(SendQueueMessageAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, resolvedQueueName }));
        }
    }
}
