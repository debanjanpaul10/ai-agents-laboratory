using System.Threading.Channels;
using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.Contracts;

/// <summary>
/// Represents a single SSE connection's subscription to the notifications stream.
/// Dispose to unregister the connection and free its channel.
/// </summary>
public interface INotificationsSubscription : IDisposable
{
    ChannelReader<NotificationsDomain> Reader { get; }
}

