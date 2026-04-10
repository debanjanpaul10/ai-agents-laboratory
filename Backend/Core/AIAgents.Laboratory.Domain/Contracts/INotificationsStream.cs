using System.Threading.Channels;
using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.Contracts;

/// <summary>
/// Provides an interface for managing a notifications stream, allowing clients to subscribe to receive notifications for specific users and publish new notifications to the stream.
/// </summary>
public interface INotificationsStream
{
    /// <summary>
    /// Subscribes to the notifications stream for a specific recipient user name, returning a ChannelReader that can be used to read incoming notifications for that user.
    /// </summary>
    /// <param name="recipientUserName">The user name of the recipient for whom to subscribe to notifications.</param>
    /// <returns>A ChannelReader that can be used to read incoming notifications for the specified recipient user name.</returns>
    ChannelReader<NotificationsDomain> Subscribe(string recipientUserName);

    /// <summary>
    /// Publishes a new notification to the stream for a specific recipient user name, allowing any subscribers for that user name to receive the notification.
    /// </summary>
    /// <param name="recipientUserName">The user name of the recipient for whom to publish the notification.</param>
    /// <param name="notification">The notification to be published to the stream.</param>
    /// <returns>True if the notification was successfully published; otherwise, false.</returns>
    bool Publish(
        string recipientUserName,
        NotificationsDomain notification
    );
}

