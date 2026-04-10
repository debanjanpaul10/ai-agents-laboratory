using System.Collections.Concurrent;
using System.Threading.Channels;
using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Implements the INotificationsStream interface to manage a notifications stream using in-memory channels.
/// </summary>
/// <seealso cref="INotificationsStream"/>
public sealed class NotificationsStream : INotificationsStream
{
    /// <summary>
    /// The concurrent dictionary containing the notifications domain channels.
    /// </summary>
    private readonly ConcurrentDictionary<string, Channel<NotificationsDomain>> _channels = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Subscribes to the notifications stream for a specific recipient user name, returning a ChannelReader that can be used to read incoming notifications for that user.
    /// </summary>
    /// <param name="recipientUserName">The user name of the recipient for whom to subscribe to notifications.</param>
    /// <returns>
    /// A ChannelReader that can be used to read incoming notifications for the specified recipient user name.
    /// </returns>
    public ChannelReader<NotificationsDomain> Subscribe(string recipientUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(recipientUserName);

        var channel = this._channels.GetOrAdd(recipientUserName, _ =>
            Channel.CreateBounded<NotificationsDomain>(new BoundedChannelOptions(capacity: 100)
            {
                SingleReader = false,
                SingleWriter = false,
                FullMode = BoundedChannelFullMode.DropOldest
            })
        );

        return channel.Reader;
    }

    /// <summary>
    /// Publishes a new notification to the stream for a specific recipient user name, allowing any subscribers for that user name to receive the notification.
    /// </summary>
    /// <param name="recipientUserName">The user name of the recipient for whom to publish the notification.</param>
    /// <param name="notification">The notification to be published to the stream.</param>
    /// <returns>
    /// True if the notification was successfully published; otherwise, false.
    /// </returns>
    public bool Publish(string recipientUserName, NotificationsDomain notification)
    {
        if (string.IsNullOrWhiteSpace(recipientUserName))
            return false;

        var channel = this._channels.GetOrAdd(recipientUserName, _ =>
            Channel.CreateBounded<NotificationsDomain>(new BoundedChannelOptions(capacity: 100)
            {
                SingleReader = false,
                SingleWriter = false,
                FullMode = BoundedChannelFullMode.DropOldest
            })
        );

        return channel.Writer.TryWrite(notification);
    }
}
