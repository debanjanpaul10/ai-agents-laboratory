using System.Threading.Channels;
using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.Contracts;

/// <summary>
/// Provides an interface for managing a notifications stream, allowing clients to subscribe to receive notifications for specific users and publish new notifications to the stream.
/// </summary>
public interface INotificationsStream
{
    /// <summary>
    /// Subscribes to the notifications stream for a specific recipient user name.
    /// Each call creates an independent channel so multiple concurrent connections
    /// (e.g. multiple browser tabs) each receive every message.
    /// Dispose the returned handle when the connection closes to release resources.
    /// </summary>
    /// <param name="recipientUserName">The user name of the recipient for whom to subscribe to notifications.</param>
    /// <returns>
    /// A <see cref="INotificationsSubscription"/> that exposes a <see cref="ChannelReader{T}"/>
    /// and must be disposed when the SSE connection ends.
    /// </returns>
    INotificationsSubscription Subscribe(string recipientUserName);

    /// <summary>
    /// Publishes a new notification to the stream for a specific recipient user name, allowing any subscribers for that user name to receive the notification.
    /// </summary>
    /// <param name="recipientUserName">The user name of the recipient for whom to publish the notification.</param>
    /// <param name="notification">The notification to be published to the stream.</param>
    /// <returns>True if the notification was successfully published to at least one subscriber; otherwise, false.</returns>
    bool Publish(
        string recipientUserName,
        NotificationsDomain notification
    );
}
