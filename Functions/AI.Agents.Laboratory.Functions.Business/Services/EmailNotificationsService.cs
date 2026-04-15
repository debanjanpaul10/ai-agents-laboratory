using AI.Agents.Laboratory.Functions.Business.Contracts;
using AI.Agents.Laboratory.Functions.Shared.Constants;
using AI.Agents.Laboratory.Functions.Shared.Exceptions;
using AI.Agents.Laboratory.Functions.Shared.Helpers;
using AI.Agents.Laboratory.Functions.Shared.Models;
using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AI.Agents.Laboratory.Functions.Business.Services;

/// <summary>
/// Provides methods for handling the email notifications services.
/// </summary>
/// <param name="configuration">The configuration service.</param>
/// <param name="correlationContext">The correlation context used for telemetry logging and tracking.</param>
/// <param name="emailClient">The Azure email sender client service.</param>
/// <param name="logger">The logger service.</param>
/// <seealso cref="AI.Agents.Laboratory.Functions.Business.Contracts.IEmailNotificationsService" />
public sealed class EmailNotificationsService(
    ILogger<EmailNotificationsService> logger,
    IConfiguration configuration,
    ICorrelationContext correlationContext,
    EmailClient emailClient) : IEmailNotificationsService
{
    /// <summary>
    /// The email communication service sender address.
    /// </summary>
    private readonly string EMAIL_COMMUNICATION_SENDER = configuration[AzureAppConfigurationConstants.EmailNotificationServiceSenderAddress]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationMissingExceptionMessage);

    /// <summary>
    /// Sends the email notification asynchronous.
    /// </summary>
    /// <param name="emailNotificationModel">The email notification model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> SendEmailNotificationAsync(
        NotificationRequest emailNotificationModel,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodStart,
                nameof(SendEmailNotificationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, emailNotificationModel })
            );

            var emailSendingOperation = await emailClient.SendAsync(
                wait: Azure.WaitUntil.Completed,
                senderAddress: EMAIL_COMMUNICATION_SENDER,
                recipientAddress: emailNotificationModel.RecipientUserName,
                subject: emailNotificationModel.Title,
                htmlContent: emailNotificationModel.Message,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            response = emailSendingOperation.Value.Status == EmailSendStatus.Succeeded;
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggerConstants.LogHelperMethodFailed,
                nameof(SendEmailNotificationAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodEnd,
                nameof(SendEmailNotificationAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, emailNotificationModel, response })
            );
        }
    }
}
