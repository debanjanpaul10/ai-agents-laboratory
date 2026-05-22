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
/// <seealso cref="INotificationService" />
public sealed class EmailNotificationsService(
    ILogger<EmailNotificationsService> logger,
    IConfiguration configuration,
    ICorrelationContext correlationContext,
    EmailClient emailClient) : INotificationService
{
    /// <summary>
    /// The email communication service sender address.
    /// </summary>
    private readonly string EMAIL_COMMUNICATION_SENDER = configuration[AzureAppConfigurationConstants.EmailNotificationServiceSenderAddress]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationMissingExceptionMessage);

    /// <inheritdoc/>
    public async Task<bool> SendNotificationsAsync(
        NotificationRequest notificationModel,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodStart,
                nameof(SendNotificationsAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, notificationModel })
            );

            var emailSendingOperation = await emailClient.SendAsync(
                wait: Azure.WaitUntil.Completed,
                senderAddress: EMAIL_COMMUNICATION_SENDER,
                recipientAddress: notificationModel.RecipientUserName,
                subject: notificationModel.Title,
                htmlContent: notificationModel.Message,
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
                nameof(SendNotificationsAsync), DateTime.UtcNow, ex.Message
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
                nameof(SendNotificationsAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, notificationModel, response })
            );
        }
    }
}
