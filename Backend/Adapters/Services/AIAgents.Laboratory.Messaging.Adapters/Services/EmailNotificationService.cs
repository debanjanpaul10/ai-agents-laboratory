using AIAgents.Laboratory.Domain.DrivenPorts;
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
/// <param name="emailClient">The email client service.</param>
/// <seealso cref="IEmailNotificationService"/>
public sealed class EmailNotificationService(ILogger<EmailNotificationService> logger, IConfiguration configuration, EmailClient emailClient) : IEmailNotificationService
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
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> SendEmailNotificationAsync(string subject, string content, string recipient)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(SendEmailNotificationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { subject, content, recipient }));

            EmailSendOperation emailSendingOperation = await emailClient.SendAsync(
                wait: Azure.WaitUntil.Completed,
                senderAddress: EMAIL_COMMUNICATION_SENDER,
                recipientAddress: recipient,
                subject: subject,
                htmlContent: content).ConfigureAwait(false);

            EmailSendResult status = emailSendingOperation.Value;
            return status.Status == EmailSendStatus.Succeeded;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(SendEmailNotificationAsync), DateTime.UtcNow, ex.Message);
            return false;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(SendEmailNotificationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { subject, content, recipient }));
        }
    }
}
