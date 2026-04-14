using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Messaging.Adapters.Contracts;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Messaging.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.Messaging.Adapters.Services;

/// <summary>
/// Provides functionality for managing interactions with Azure Service Bus, including sending messages to queues. 
/// This implementation of the <see cref="IServiceBusManager"/> interface allows for decoupling the message sending logic from the underlying implementation, enabling easier testing and flexibility in how messages are sent to Azure Service Bus.
/// </summary>
/// <param name="serviceBusClient">The Azure Service Bus client used to create senders and send messages to the specified queues.</param>
/// <param name="logger">The logger used to record informational and error messages related to Service Bus operations, including message sending attempts and any exceptions that may occur during the process.</param>
/// <param name="correlationContext">The correlation context used to track and correlate Service Bus operations across system boundaries, allowing for better observability and troubleshooting of message sending activities.</param>
/// <seealso cref="IServiceBusManager"/>
public sealed class ServiceBusManager(
    ServiceBusClient serviceBusClient,
    ILogger<ServiceBusManager> logger,
    ICorrelationContext correlationContext) : IServiceBusManager
{
    /// <summary>
    /// Sends a message to an Azure Service Bus queue.
    /// </summary>
    /// <typeparam name="T">The payload type.</typeparam>
    /// <param name="payload">The payload to serialize and send.</param>
    /// <param name="queueName">Optional queue override; if null/empty default queue is used.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task SendQueueMessageAsync<T>(
        T payload,
        string queueName,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(payload);

        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(SendQueueMessageAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, queueName })
            );

            var sender = serviceBusClient.CreateSender(queueName);
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
                nameof(SendQueueMessageAsync), DateTime.UtcNow, ex.Message
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
                nameof(SendQueueMessageAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, queueName })
            );
        }
    }
}
