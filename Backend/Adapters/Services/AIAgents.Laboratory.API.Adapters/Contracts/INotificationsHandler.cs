using AIAgents.Laboratory.API.Adapters.Models.Request;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// Provides an interface for handling notification-related operations, such as creating new notifications based on incoming requests. 
/// This abstraction allows for decoupling the notification handling logic from the underlying implementation, enabling easier testing and flexibility in how notifications are processed and sent to recipients.
/// </summary>
public interface INotificationsHandler
{
    /// <summary>
    /// Creates a new notification based on the provided request data.
    /// </summary>
    /// <param name="request">The request object containing the details of the notification to be created.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>True if the notification was created successfully; otherwise, false.</returns>
    Task<bool> CreateNewNotificationAsync(
        CreateNotificationRequestDto request,
        CancellationToken cancellationToken = default);
}
