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
/// This Azure Function listens to a Service Bus queue for incoming push notification messages.
/// </summary>
/// <remarks>
/// Upon receiving a message, it deserializes the message body into a NotificationRequest object and processes it using the IPushNotificationsService. 
/// The function includes error handling to manage invalid payloads and exceptions during processing, ensuring that messages are either completed, abandoned, or moved to the dead-letter queue as appropriate. 
/// Logging is implemented throughout the function to track the processing flow and any issues that arise.
/// </remarks>
/// <param name="logger">The logger instance for logging information and errors during the function execution.</param>
/// <param name="correlationContext">The correlation context for tracking the correlation ID across logs and operations.</param>
/// <param name="pushNotificationsService">The service responsible for handling the business logic of processing push notifications.</param>
public sealed class PushNotificationsFunction(
    ILogger<PushNotificationsFunction> logger,
    ICorrelationContext correlationContext,
    IPushNotificationsService pushNotificationsService)
{
    /// <summary>
    /// This method is triggered when a new message arrives in the specified Service Bus queue. 
    /// </summary>
    /// <remarks>
    /// It processes the message by deserializing it into a NotificationRequest and then calling the push notification service to handle the business logic. 
    /// The method includes error handling to manage invalid messages and exceptions, ensuring proper message lifecycle management with complete, abandon, or dead-letter actions as needed.
    /// </remarks>
    /// <param name="message">The incoming Service Bus message containing the push notification request data.</param>
    /// <param name="messageActions">The actions available for managing the message lifecycle, such as completing, abandoning, or dead-lettering the message.</param>
    /// <param name="cancellationToken">The cancellation token to signal cancellation of the function execution, allowing for graceful shutdowns and timeouts.</param>
    /// <returns>A task representing the asynchronous operation of processing the push notification message.</returns>
    [Function(nameof(PushNotificationsFunction))]
    public async Task RunAsync(
        [ServiceBusTrigger("%PushNotificationsQueue%", Connection = "AzureServiceBusConnectionString")] ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        CancellationToken cancellationToken = default
    )
    {
        var messageBody = Encoding.UTF8.GetString(message.Body);
        var notificationMessageBody = JsonConvert.DeserializeObject<NotificationRequest>(messageBody.ToString());
        if (notificationMessageBody is null)
        {
            logger.LogError("Message {MessageId} has null payload - moving to dead-letter queue", message.MessageId);
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
                nameof(PushNotificationsFunction), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, notificationMessageBody })
            );

            response = await pushNotificationsService.ReceivePushNotificationAsync(
                request: notificationMessageBody,
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
                nameof(PushNotificationsFunction), DateTime.UtcNow, ex.Message
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
                nameof(PushNotificationsFunction), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, notificationMessageBody, response })
            );
        }
    }
}
