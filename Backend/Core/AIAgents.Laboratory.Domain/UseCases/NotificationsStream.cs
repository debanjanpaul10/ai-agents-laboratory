using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;
using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides services for managing a notifications stream, allowing clients to subscribe to receive notifications for specific users and publish new notifications to the stream.
/// </summary>
/// <seealso cref="AIAgents.Laboratory.Domain.Contracts.INotificationsStream" />
[ExcludeFromCodeCoverage]
public sealed class NotificationsStream : INotificationsStream
{
    /// <summary>
    /// The concurrent dictionary of subscribers.
    /// </summary>
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, Channel<NotificationsDomain>>> _subscribers = new(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public INotificationsSubscription Subscribe(string recipientUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(recipientUserName);

        var channel = Channel.CreateBounded<NotificationsDomain>(new BoundedChannelOptions(capacity: 100)
        {
            SingleReader = true,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.DropOldest
        });

        var id = Guid.NewGuid();
        var userChannels = this._subscribers.GetOrAdd(
            recipientUserName,
            _ => new ConcurrentDictionary<Guid, Channel<NotificationsDomain>>()
        );
        userChannels[id] = channel;

        return new NotificationsSubscription(channel.Reader, () =>
        {
            userChannels.TryRemove(id, out _);
            // Remove the user entry entirely once the last subscriber disconnects
            if (userChannels.IsEmpty)
                this._subscribers.TryRemove(recipientUserName, out _);
        });
    }

    /// <inheritdoc />
    public bool Publish(
        string recipientUserName,
        NotificationsDomain notification
    )
    {
        if (string.IsNullOrWhiteSpace(recipientUserName))
            return false;

        if (!_subscribers.TryGetValue(recipientUserName, out var userChannels) || userChannels.IsEmpty)
            return false;

        var published = false;
        foreach (var (_, channel) in userChannels)
            published |= channel.Writer.TryWrite(notification);

        return published;
    }

    /// <summary>
    /// The notifications subscription class.
    /// </summary>
    /// <param name="onDispose">The on action dispose event handler.</param>
    /// <param name="reader">The notifications channel reader service.</param>
    /// <seealso cref="AIAgents.Laboratory.Domain.Contracts.INotificationsSubscription" />
    private sealed class NotificationsSubscription(
        ChannelReader<NotificationsDomain> reader,
        Action onDispose) : INotificationsSubscription
    {
        /// <summary>
        /// The disposed
        /// </summary>
        private int _disposed;

        /// <inheritdoc/>
        public ChannelReader<NotificationsDomain> Reader { get; } = reader;

        /// <inheritdoc/>
        public void Dispose()
        {
            if (Interlocked.Exchange(ref this._disposed, 1) == 0)
                onDispose();
        }
    }
}
