using AI.Agents.Laboratory.Functions.Shared.Models;

namespace AI.Agents.Laboratory.Functions.Business.Contracts;

/// <summary>
/// Defines the contract for a service that processes push notification requests.
/// </summary>
public interface IPushNotificationsService
{
    /// <summary>
    /// Processes an incoming push notification request and returns a boolean indicating the success of the operation.
    /// </summary>
    /// <param name="request">The notification request to process.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<bool> ReceivePushNotificationAsync(
        NotificationRequest request,
        CancellationToken cancellationToken = default
    );
}
