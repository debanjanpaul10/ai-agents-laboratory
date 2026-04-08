using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.Out;
using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Messaging.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.Messaging.Adapters.Services;

/// <summary>
/// The email notification service.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="correlationContext">The correlation context for logging.</param>
/// <param name="emailClient">The email client service.</param>
/// <seealso cref="IEmailNotificationService"/>
public sealed class EmailNotificationService(
    ILogger<EmailNotificationService> logger,
    IConfiguration configuration,
    ICorrelationContext correlationContext,
    EmailClient emailClient) : IEmailNotificationService
{
    /// <summary>
    /// The email communication service sender address.
    /// </summary>
    private readonly string EMAIL_COMMUNICATION_SENDER = configuration[AzureAppConfigurationConstants.EmailNotificationServiceSenderAddress]
        ?? throw new KeyNotFoundException(ExceptionMessagesConstants.ConfigurationMissingExceptionMessage);

    /// <summary>
    /// Sends a notification asynchronously based on the provided notification request domain entity.
    /// </summary>
    /// <param name="notificationRequest">The notification request domain entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> SendNotificationAsync(
        NotificationsDomain notificationRequest,
        CancellationToken cancellationToken = default)
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(SendNotificationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, notificationRequest }));

            EmailSendOperation emailSendingOperation = await emailClient.SendAsync(
                wait: Azure.WaitUntil.Completed,
                senderAddress: EMAIL_COMMUNICATION_SENDER,
                recipientAddress: notificationRequest.RecipientUserName,
                subject: notificationRequest.Title,
                htmlContent: notificationRequest.Message,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            response = emailSendingOperation.Value.Status == EmailSendStatus.Succeeded;
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(SendNotificationAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(SendNotificationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, notificationRequest, response }));
        }
    }
}
