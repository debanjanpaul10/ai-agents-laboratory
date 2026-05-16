using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Mapper;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.Ports.In;
using Microsoft.AspNetCore.Http;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// Provides an implementation of the INotificationsHandler interface, responsible for handling notification-related operations by interacting with the INotificationsService.
/// </summary>
/// <param name="notificationsService">The INotificationsService is an abstraction that encapsulates the business logic for handling notifications, allowing the handler to delegate the actual processing of notification creation to the service layer.</param>
/// <seealso cref="INotificationsHandler"/>
public sealed class NotificationsHandler(INotificationsService notificationsService) : INotificationsHandler
{
    /// <inheritdoc/>
    public async Task<bool> CreateNewNotificationAsync(
        CreateNotificationRequestDto request,
        CancellationToken cancellationToken = default
    )
    {
        var domainInput = DomainMapperProfile.MapToDomain(request);
        return await notificationsService.CreateNewNotificationAsync(
            request: domainInput,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAllNotificationsForUserAsync(
        string currentLoggedInUser,
        CancellationToken cancellationToken = default
    )
    {
        return await notificationsService.DeleteAllNotificationsForUserAsync(
            currentLoggedInUser,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NotificationsResponseDto>> GetNotificationsForUserAsync(
        string recipientUserName,
        CancellationToken cancellationToken = default
    )
    {
        var domainResponse = await notificationsService.GetNotificationsForUserAsync(
            recipientUserName,
            cancellationToken
        ).ConfigureAwait(false);
        return [.. domainResponse.Select(DomainMapperProfile.MapToDto)];
    }

    /// <inheritdoc/>
    public async Task<bool> MarkExistingNotificationAsReadAsync(
        string currentLoggedInUser,
        Guid notificationId,
        CancellationToken cancellationToken = default
    )
    {
        return await notificationsService.MarkExistingNotificationAsReadAsync(
            currentLoggedInUser,
            notificationId,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task StreamNotificationsForUserAsync(
        string recipientUserName,
        HttpResponse response,
        CancellationToken cancellationToken = default,
        CancellationToken requestAborted = default
    )
    {
        await notificationsService.StreamNotificationsForUserAsync(
            recipientUserName,
            response,
            cancellationToken,
            requestAborted
        ).ConfigureAwait(false);
    }
}
