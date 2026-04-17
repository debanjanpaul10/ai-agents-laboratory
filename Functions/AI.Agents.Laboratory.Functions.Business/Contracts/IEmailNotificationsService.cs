using AI.Agents.Laboratory.Functions.Shared.Models;

namespace AI.Agents.Laboratory.Functions.Business.Contracts;

/// <summary>
/// Defines contract for a service that processes the email notifications.
/// </summary>
public interface IEmailNotificationsService
{
    /// <summary>
    /// Sends the email notification asynchronous.
    /// </summary>
    /// <param name="emailNotificationModel">The email notification model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for success/failure.</returns>
    Task<bool> SendEmailNotificationAsync(
        NotificationRequest emailNotificationModel,
        CancellationToken cancellationToken = default
    );
}
