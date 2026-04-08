using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// Provides an implementation of the INotificationsService interface, responsible for handling notification-related operations by interacting with the IApplicationNotificationsService.
/// </summary>
/// <param name="logger">The ILogger instance is used for logging information, warnings, and errors that occur within the service's methods. This helps in monitoring the application's behavior and diagnosing issues when they arise.</param>
/// <param name="correlationContext">The ICorrelationContext is used to manage correlation IDs for requests, which helps in tracing and correlating logs across different components of the application, especially in distributed systems.</param>
/// <param name="notificationsService">The IApplicationNotificationsService is an abstraction that encapsulates the business logic for handling notifications.</param>
/// <seealso cref="INotificationsService"/>
public sealed class NotificationsService(
    ILogger<NotificationsService> logger,
    ICorrelationContext correlationContext,
    IApplicationNotificationsService notificationsService) : INotificationsService
{
    /// <summary>
    /// Creates a new notification based on the provided request data.
    /// </summary>
    /// <param name="request">The request object containing the details of the notification to be created.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation if needed.</param>
    /// <returns>True if the notification was created successfully; otherwise, false.</returns>
    public async Task<bool> CreateNewNotificationAsync(
        NotificationRequestDomain request,
        CancellationToken cancellationToken = default)
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(CreateNewNotificationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, request }));

            response = await notificationsService.SendNotificationAsync(
                notificationRequest: request,
                cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(CreateNewNotificationAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(CreateNewNotificationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, request, response }));
        }
    }
}
