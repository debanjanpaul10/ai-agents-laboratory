using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using Microsoft.AspNetCore.Http;

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
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves a list of notifications for a specific user based on their username. 
    /// This method allows clients to fetch all notifications that are relevant to a particular user
    /// </summary>
    /// <param name="recipientUserName">The username of the user for whom to retrieve notifications.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>A list of notifications relevant to the specified user.</returns>
    Task<IEnumerable<NotificationsResponseDto>> GetNotificationsForUserAsync(
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
        Guid notificationId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Streams notifications for a specific user in real-time using Server-Sent Events (SSE).
    /// </summary>
    /// <param name="recipientUserName">The username of the user for whom to stream notifications.</param>
    /// <param name="response">The HttpResponse object used to send the streamed notifications to the client.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <param name="requestAborted">The cancellation token to handle request abortion.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task StreamNotificationsForUserAsync(
        string recipientUserName,
        HttpResponse response,
        CancellationToken cancellationToken = default,
        CancellationToken requestAborted = default
    );
}
