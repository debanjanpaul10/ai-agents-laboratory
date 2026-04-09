using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.Ports.Out;

/// <summary>
/// Provides an interface for data access operations related to notifications, allowing the service layer to interact with the underlying data storage without being tightly coupled to a specific implementation. 
/// This abstraction enables flexibility in choosing different data storage solutions (e.g., MongoDB, SQL Server) and promotes separation of concerns within the application architecture.
/// </summary>
public interface INotificationsDataManager
{
    /// <summary>
    /// Retrieves a list of notifications for a specific user based on their username. 
    /// This method allows clients to fetch all notifications that are relevant to a particular user
    /// </summary>
    /// <param name="recipientUserName">The username of the user for whom to retrieve notifications.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>A list of notifications relevant to the specified user.</returns>
    Task<IEnumerable<NotificationsDomain>> GetNotificationsForUserAsync(
        string recipientUserName,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Marks an existing notification as read for a specific user based on the notification identifier.
    /// </summary>
    /// <remarks>
    /// Marking a notification as read typically involves updating the status of the notification in the data store to indicate that it has been acknowledged or viewed by the recipient user.
    /// </remarks>
    /// <param name="recipientUserName">The username of the user for whom to mark the notification as read.</param>
    /// <param name="notificationId">The identifier of the notification to be marked as read.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>A boolean value indicating whether the operation was successful (true) or not (false).</returns>
    Task<bool> MarkExistingNotificationAsReadAsync(
        string recipientUserName,
        int notificationId,
        CancellationToken cancellationToken = default
    );
}
