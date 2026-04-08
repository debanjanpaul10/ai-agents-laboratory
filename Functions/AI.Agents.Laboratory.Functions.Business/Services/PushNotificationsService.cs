using AI.Agents.Laboratory.Functions.Business.Contracts;
using AI.Agents.Laboratory.Functions.Data.Contracts;
using AI.Agents.Laboratory.Functions.Shared.Constants;
using AI.Agents.Laboratory.Functions.Shared.Exceptions;
using AI.Agents.Laboratory.Functions.Shared.Helpers;
using AI.Agents.Laboratory.Functions.Shared.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AI.Agents.Laboratory.Functions.Business.Services;

/// <summary>
/// Implements the IPushNotificationsService to handle the processing of push notification requests.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="correlationContext">The correlation context.</param>
/// <param name="notificationsDataManager">The notifications data manager.</param>
/// <seealso cref="IPushNotificationsService"/>
public sealed class PushNotificationsService(
    ILogger<PushNotificationsService> logger,
    ICorrelationContext correlationContext,
    INotificationsDataManager notificationsDataManager) : IPushNotificationsService
{
    /// <summary>
    /// Processes an incoming push notification request and returns a boolean indicating the success of the operation.
    /// </summary>
    /// <param name="request">The notification request to process.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<bool> ReceivePushNotificationAsync(
        NotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(LoggerConstants.LogHelperMethodStart, nameof(ReceivePushNotificationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, request }));

            response = await notificationsDataManager.SavePushNotificationsDataAsync(
                request,
                cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggerConstants.LogHelperMethodFailed, nameof(ReceivePushNotificationAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggerConstants.LogHelperMethodEnd, nameof(ReceivePushNotificationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, request, response }));
        }
    }
}
