namespace AIAgents.Laboratory.Messaging.Adapters.Contracts;

/// <summary>
/// Provides an interface for managing interactions with Azure Service Bus, including sending messages to queues. 
/// This abstraction allows for decoupling the message sending logic from the underlying implementation, enabling easier testing and flexibility in how messages are sent to Azure Service Bus.
/// </summary>
public interface IServiceBusManager
{
    /// <summary>
    /// Sends a message to an Azure Service Bus queue.
    /// </summary>
    /// <typeparam name="T">The payload type.</typeparam>
    /// <param name="payload">The payload to serialize and send.</param>
    /// <param name="queueName">Optional queue override; if null/empty default queue is used.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task SendQueueMessageAsync<T>(
        T payload,
        string? queueName = null,
        CancellationToken cancellationToken = default);
}
