namespace AIAgents.Laboratory.Domain.Ports.Out;

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
    /// <param name="queueName">The queue name where message is to be published.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean response for success/failure.</returns>
    Task<bool> SendQueueMessageAsync<T>(
        T payload,
        string queueName,
        CancellationToken cancellationToken = default
    );
}
