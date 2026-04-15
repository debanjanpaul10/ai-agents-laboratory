using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using AIAgents.Laboratory.Domain.Ports.Out;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The <c>FeedbackService</c> class provides functionalities to handle user feedback, including bug reports and feature requests. 
/// </summary>
/// <remarks>It interacts with a data manager to store feedback data and an email notification service to notify administrators of new feedback submissions. 
/// The service also includes methods to retrieve submitted feedback for administrative review, ensuring that only authorized users can access this information.</remarks>
/// <param name="logger">The logger service.</param>
/// <param name="configuration">The configuration.</param>
/// <param name="correlationContext">The correlation context for logging.</param>
/// <param name="feedbackDataManager">The feedback data manager.</param>
/// <param name="serviceBusManager">The service bus manager service.</param>
/// <seealso cref="IFeedbackService"/>
public sealed class FeedbackService(
    ILogger<FeedbackService> logger,
    IConfiguration configuration,
    ICorrelationContext correlationContext,
    IFeedbackDataManager feedbackDataManager,
    IServiceBusManager serviceBusManager) : IFeedbackService
{
    /// <summary>
    /// The admin email address from configuration.
    /// </summary>
    private readonly string ADMIN_EMAIL_ADDRESS = configuration[AzureAppConfigurationConstants.AdminEmailAddressConstant]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The email notifications queue.
    /// </summary>
    private readonly string EmailNotificationsQueue = configuration[AzureAppConfigurationConstants.EmailNotificationsQueueName]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// Adds the new bug report data asynchronous.
    /// </summary>
    /// <param name="bugReportData">The bug report data.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> AddNewBugReportDataAsync(
        BugReportData bugReportData,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(bugReportData);

        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(AddNewBugReportDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, bugReportData })
            );

            bugReportData.PrepareAuditEntityData(
                currentUser: bugReportData.CreatedBy
            );
            var feedbackSaveResult = await feedbackDataManager.AddNewBugReportDataAsync(
                bugReportData,
                cancellationToken
            ).ConfigureAwait(false);

            var emailNotificationMessage = await this.PrepareEmailNotificationData(
                subject: bugReportData.Title,
                content: bugReportData.Description,
                senderName: bugReportData.CreatedBy,
                cancellationToken
            ).ConfigureAwait(false);
            var emailSendResult = await serviceBusManager.SendQueueMessageAsync(
                payload: emailNotificationMessage,
                queueName: EmailNotificationsQueue,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            response = feedbackSaveResult && emailSendResult;
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(AddNewBugReportDataAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(AddNewBugReportDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, bugReportData, response })
            );
        }
    }

    /// <summary>
    /// Adds the new feature request data asynchronous.
    /// </summary>
    /// <param name="featureRequestData">The feature request data.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> AddNewFeatureRequestDataAsync(
        NewFeatureRequestData featureRequestData,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(featureRequestData);

        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, featureRequestData })
            );

            featureRequestData.PrepareAuditEntityData(
                currentUser: featureRequestData.CreatedBy
            );
            var feedbackSaveResult = await feedbackDataManager.AddNewFeatureRequestDataAsync(
                featureRequestData,
                cancellationToken
            ).ConfigureAwait(false);

            var emailNotificationMessage = await this.PrepareEmailNotificationData(
                subject: featureRequestData.Title,
                content: featureRequestData.Description,
                senderName: featureRequestData.CreatedBy,
                cancellationToken
            ).ConfigureAwait(false);
            var emailSendResult = await serviceBusManager.SendQueueMessageAsync(
                payload: emailNotificationMessage,
                queueName: EmailNotificationsQueue,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            response = feedbackSaveResult && emailSendResult;
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, featureRequestData, response })
            );
        }
    }

    /// <summary>
    /// Gets all bug reports data asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation. Optional.</param>
    /// <returns>A list of <see cref="BugReportData"/></returns>
    public async Task<IEnumerable<BugReportData>> GetAllBugReportsDataAsync(
        string currentLoggedinUser,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser })
            );

            if (currentLoggedinUser == ADMIN_EMAIL_ADDRESS)
                return await feedbackDataManager.GetAllBugReportsDataAsync(
                    currentLoggedinUser,
                    cancellationToken
                ).ConfigureAwait(false);
            else
                throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser })
            );
        }
    }

    /// <summary>
    /// Gets all submitted feature requests asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation. Optional.</param>
    /// <returns>A list of <see cref="NewFeatureRequestData"/></returns>
    public async Task<IEnumerable<NewFeatureRequestData>> GetAllSubmittedFeatureRequestsAsync(
        string currentLoggedinUser,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodStart,
                nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser })
            );

            if (currentLoggedinUser == ADMIN_EMAIL_ADDRESS)
                return await feedbackDataManager.GetAllSubmittedFeatureRequestsAsync(
                    currentLoggedinUser,
                    cancellationToken
                ).ConfigureAwait(false);
            else
                throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.LogHelperMethodEnd,
                nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser })
            );
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Prepares the email notification data.
    /// </summary>
    /// <param name="subject">The subject.</param>
    /// <param name="content">The content.</param>
    /// <param name="senderName">Name of the sender.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The notifications domain model.</returns>
    private async Task<NotificationsDomain> PrepareEmailNotificationData(
        string subject,
        string content,
        string senderName,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var template = await File.ReadAllTextAsync(
                path: FeedbackTemplateConstants.FileName,
                cancellationToken
            ).ConfigureAwait(false);

            var subs = new Dictionary<string, string>
            {
                { "{{ApplicationName}}", FeedbackTemplateConstants.CurrentApplicationName },
                { "{{Subject}}", subject },
                { "{{Message}}", content },
                { "{{SenderAddress}}", senderName }
            };

            // Replace placeholders with the actual values
            var emailMessage = template;
            foreach (var kvp in subs)
                emailMessage = emailMessage.Replace(kvp.Key, kvp.Value);

            return new NotificationsDomain
            {
                Title = subject,
                Message = emailMessage,
                RecipientUserName = ADMIN_EMAIL_ADDRESS,
                NotificationType = nameof(NotificationTypes.Email),
                CreatedBy = senderName
            };
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.LogHelperMethodFailed,
                nameof(PrepareEmailNotificationData), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
    }

    #endregion

}
