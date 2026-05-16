using AI.Agents.Laboratory.Functions.Shared.Models;

namespace AI.Agents.Laboratory.Functions.Business.Contracts;

/// <summary>
/// Defines contract for a service that processes the notifications.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Sends the notifications asynchronously.
    /// </summary>
    /// <param name="notificationModel">The notification model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for success/failure.</returns>
    Task<bool> SendNotificationsAsync(
        NotificationRequest notificationModel,
        CancellationToken cancellationToken = default
    );
}
