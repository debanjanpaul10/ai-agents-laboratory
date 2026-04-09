using AI.Agents.Laboratory.Functions.Shared.Models;

namespace AI.Agents.Laboratory.Functions.Data.Contracts;

/// <summary>
/// Defines the contract for a notifications data manager that provides methods for saving push notification data to a data store. 
/// This interface abstracts the underlying data access implementation, allowing for flexibility and separation of concerns in the business logic layer.
/// </summary>
public interface INotificationsDataManager
{
    /// <summary>
    /// Saves push notification data to a data store, returning a boolean indicating the success of the operation. The method takes a NotificationRequest object as input, which contains the details of the notification to be saved. The implementation of this method is responsible for handling the actual data persistence logic, including any necessary transformations or validations before saving the data.
    /// </summary>
    /// <param name="request">The notification request containing the details to be saved.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean indicating success.</returns>
    Task<bool> SavePushNotificationsDataAsync(
        NotificationRequest request,
        CancellationToken cancellationToken = default
    );
}
