using AIAgents.Laboratory.Domain.Contracts;
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
public sealed class EmailNotificationService(ILogger<EmailNotificationService> logger, IConfiguration configuration, ICorrelationContext correlationContext, EmailClient emailClient) : IEmailNotificationService
{
    /// <summary>
    /// The email communication service sender address.
    /// </summary>
    private readonly string EMAIL_COMMUNICATION_SENDER = configuration[AzureAppConfigurationConstants.EmailNotificationServiceSenderAddress]
        ?? throw new KeyNotFoundException(ExceptionMessagesConstants.ConfigurationMissingExceptionMessage);

    /// <summary>
    /// Sends an email notification asynchronously.
    /// </summary>
    /// <param name="subject">The email subject.</param>
    /// <param name="content">The email content.</param>
    /// <param name="recipient">The email recipient.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> SendEmailNotificationAsync(string subject, string content, string recipient, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(SendEmailNotificationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, subject, content, recipient }));

            EmailSendOperation emailSendingOperation = await emailClient.SendAsync(
                wait: Azure.WaitUntil.Completed,
                senderAddress: EMAIL_COMMUNICATION_SENDER,
                recipientAddress: recipient,
                subject,
                htmlContent: content,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            EmailSendResult status = emailSendingOperation.Value;
            return status.Status == EmailSendStatus.Succeeded;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(SendEmailNotificationAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(SendEmailNotificationAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, subject, content, recipient }));
        }
    }
}
