using System.Text;
using AI.Agents.Laboratory.Functions.Business.Contracts;
using AI.Agents.Laboratory.Functions.Shared.Constants;
using AI.Agents.Laboratory.Functions.Shared.Helpers;
using AI.Agents.Laboratory.Functions.Shared.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AI.Agents.Laboratory.Functions;

/// <summary>
/// This Azure Function listens to a Service Bus queue for incoming email notification messages.
/// </summary>
/// <remarks>
/// Upon receiving a message, it deserializes the message body into a NotificationRequest object and processes it using the IEmailNotificationsService. 
/// The function includes error handling to manage invalid payloads and exceptions during processing, ensuring that messages are either completed, abandoned, or moved to the dead-letter queue as appropriate. 
/// Logging is implemented throughout the function to track the processing flow and any issues that arise.
/// </remarks>
/// <param name="logger">The logger instance for logging information and errors during the function execution.</param>
/// <param name="correlationContext">The correlation context for tracking the correlation ID across logs and operations.</param>
/// <param name="emailNotificationsService">The service responsible for handling the business logic of processing email notifications.</param>
public sealed class SendEmailNotificationFunction(
    ILogger<SendEmailNotificationFunction> logger,
    ICorrelationContext correlationContext,
    IEmailNotificationsService emailNotificationsService)
{
    /// <summary>
    /// Runs the function asynchronous.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="messageActions">The message actions.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    [Function(nameof(SendEmailNotificationFunction))]
    public async Task RunAsync(
        [ServiceBusTrigger("%EmailNotificationsQueue%", Connection = "AzureServiceBusConnectionString")] ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        CancellationToken cancellationToken = default
    )
    {
        var messageBody = Encoding.UTF8.GetString(message.Body);
        var notificationMessageBody = JsonConvert.DeserializeObject<NotificationRequest>(messageBody.ToString());
        if (notificationMessageBody is null)
        {
            logger.LogError(
                ExceptionConstants.NullPayloadExceptionMessage,
                message.MessageId
            );
            await messageActions.DeadLetterMessageAsync(
                message,
                deadLetterReason: FunctionsDomainConstants.DeadLetterConstants.InvalidPayloadReason,
                deadLetterErrorDescription: FunctionsDomainConstants.DeadLetterConstants.InvalidPayloadDescription,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
            return;
        }

        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodStart,
                nameof(SendEmailNotificationFunction), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, notificationMessageBody })
            );

            response = await emailNotificationsService.SendEmailNotificationAsync(
                emailNotificationModel: notificationMessageBody,
                cancellationToken
            ).ConfigureAwait(false);
            await messageActions.CompleteMessageAsync(
                message,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggerConstants.LogHelperMethodFailed,
                nameof(SendEmailNotificationFunction), DateTime.UtcNow, ex.Message
            );
            await messageActions.AbandonMessageAsync(
                message,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
        }
        finally
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodEnd,
                nameof(SendEmailNotificationFunction), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, notificationMessageBody, response })
            );
        }
    }
}