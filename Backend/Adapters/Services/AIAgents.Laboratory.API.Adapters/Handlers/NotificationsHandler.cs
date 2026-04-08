using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Ports.In;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// Provides an implementation of the INotificationsHandler interface, responsible for handling notification-related operations by interacting with the INotificationsService.
/// </summary>
/// <param name="mapper">The IMapper instance is used to map between the API request models and the domain entities, facilitating the transformation of data as it flows between different layers of the application.</param>
/// <param name="notificationsService">The INotificationsService is an abstraction that encapsulates the business logic for handling notifications, allowing the handler to delegate the actual processing of notification creation to the service layer.</param>
/// <seealso cref="INotificationsHandler"/>
public sealed class NotificationsHandler(
    IMapper mapper,
    INotificationsService notificationsService) : INotificationsHandler
{
    /// <summary>
    /// Creates a new notification based on the provided request data.
    /// </summary>
    /// <param name="request">The request object containing the details of the notification to be created.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>True if the notification was created successfully; otherwise, false.</returns>
    public async Task<bool> CreateNewNotificationAsync(
        CreateNotificationRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var domainInput = mapper.Map<NotificationsDomain>(request);
        return await notificationsService.CreateNewNotificationAsync(
            request: domainInput,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a list of notifications for a specific user based on their username. 
    /// This method allows clients to fetch all notifications that are relevant to a particular user
    /// </summary>
    /// <param name="recipientUserName">The username of the user for whom to retrieve notifications.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>A list of notifications relevant to the specified user.</returns>
    public async Task<IEnumerable<NotificationsResponseDto>> GetNotificationsForUserAsync(
        string recipientUserName,
        CancellationToken cancellationToken = default)
    {
        var domainResponse = await notificationsService.GetNotificationsForUserAsync(
            recipientUserName: recipientUserName,
            cancellationToken: cancellationToken
        ).ConfigureAwait(false);
        return mapper.Map<IEnumerable<NotificationsResponseDto>>(domainResponse);
    }
}
