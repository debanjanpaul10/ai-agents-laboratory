using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.Ports.Out;

/// <summary>
/// Email-specific notification service.
/// </summary>
public interface IEmailNotificationService
{
    /// <summary>
    /// Sends an email notification asynchronously based on the provided notification request domain entity.
    /// </summary>
    /// <param name="notificationRequest">The notification request domain entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> SendNotificationAsync(
        NotificationsDomain notificationRequest,
        CancellationToken cancellationToken = default);
}

