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
        CancellationToken cancellationToken = default);
}
