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
/// <param name="serviceBusNotificationService">The service bus notification service.</param>
/// <param name="notificationsService">The notifications service for sending in-app or push notifications.</param>
/// <seealso cref="IFeedbackService"/>
public sealed class FeedbackService(
    ILogger<FeedbackService> logger,
    IConfiguration configuration,
    ICorrelationContext correlationContext,
    IFeedbackDataManager feedbackDataManager,
    IServiceBusNotificationService serviceBusNotificationService,
    INotificationsService notificationsService) : IFeedbackService
{
    /// <summary>
    /// The admin email address from configuration.
    /// </summary>
    private readonly string ADMIN_EMAIL_ADDRESS = configuration[AzureAppConfigurationConstants.AdminEmailAddressConstant]
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

            bugReportData.PrepareAuditEntityData(currentUser: bugReportData.CreatedBy);
            var feedbackSaveResult = await feedbackDataManager.AddNewBugReportDataAsync(
                bugReportData,
                cancellationToken
            ).ConfigureAwait(false);

            var template = await File.ReadAllTextAsync(
                path: FeedbackTemplateConstants.FileName,
                cancellationToken
            ).ConfigureAwait(false);

            var emailSendResult = await serviceBusNotificationService.SendNotificationAsync(
                notificationRequest: new NotificationsDomain
                {
                    Title = bugReportData.Title,
                    Message = string.Format(template, bugReportData.Title, bugReportData.Description, bugReportData.CreatedBy),
                    RecipientUserName = ADMIN_EMAIL_ADDRESS,
                    NotificationType = nameof(NotificationTypes.Email),
                    CreatedBy = bugReportData.CreatedBy
                },
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            response = feedbackSaveResult && emailSendResult;
            if (response)
                await this.SendFeedbackServiceNotificationAsync(
                    userToBeNotified: ADMIN_EMAIL_ADDRESS,
                    currentUserEmail: bugReportData.CreatedBy,
                    feedbackId: bugReportData.Id,
                    feedbackTitle: bugReportData.Title,
                    isBugReport: true,
                    cancellationToken
                ).ConfigureAwait(false);

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

            featureRequestData.PrepareAuditEntityData(currentUser: featureRequestData.CreatedBy);
            var feedbackSaveResult = await feedbackDataManager.AddNewFeatureRequestDataAsync(
                featureRequestData,
                cancellationToken
            ).ConfigureAwait(false);

            var template = await File.ReadAllTextAsync(
                path: FeedbackTemplateConstants.FileName,
                cancellationToken
            ).ConfigureAwait(false);

            var emailSendResult = await serviceBusNotificationService.SendNotificationAsync(
                notificationRequest: new NotificationsDomain
                {
                    Title = featureRequestData.Title,
                    Message = string.Format(template, featureRequestData.Title, featureRequestData.Description, featureRequestData.CreatedBy),
                    RecipientUserName = ADMIN_EMAIL_ADDRESS,
                    NotificationType = nameof(NotificationTypes.Email),
                    CreatedBy = featureRequestData.CreatedBy
                },
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            response = feedbackSaveResult && emailSendResult;
            if (response)
                await this.SendFeedbackServiceNotificationAsync(
                    userToBeNotified: ADMIN_EMAIL_ADDRESS,
                    currentUserEmail: featureRequestData.CreatedBy,
                    feedbackId: featureRequestData.Id,
                    feedbackTitle: featureRequestData.Title,
                    isBugReport: false,
                    cancellationToken
                ).ConfigureAwait(false);

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

    /// <summary>
    /// Sends the feedback service notification asynchronous.
    /// </summary>
    /// <param name="userToBeNotified">The user to be notified.</param>
    /// <param name="currentUserEmail">The current user email who submitted the feedback.</param>
    /// <param name="feedbackId">The feedback id.</param>
    /// <param name="feedbackTitle">The feedback title.</param>
    /// <param name="isBugReport">A boolean value indicating whether the feedback is a bug report or a feature request. Default is false (feature request).</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation. Optional.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task SendFeedbackServiceNotificationAsync(
        string userToBeNotified,
        string currentUserEmail,
        int feedbackId,
        string feedbackTitle,
        bool isBugReport = false,
        CancellationToken cancellationToken = default
    )
    {
        var titleTemplate = isBugReport ? NotificationMessagesConstants.BugReportSubmittedTitleTemplate : NotificationMessagesConstants.FeatureRequestSubmittedTitleTemplate;
        var bodyTemplate = isBugReport ? NotificationMessagesConstants.BugReportSubmittedMessageTemplate : NotificationMessagesConstants.FeatureRequestSubmittedMessageTemplate;
        var notificationsDomainModel = new NotificationsDomain
        {
            Id = Guid.NewGuid(),
            RecipientUserName = userToBeNotified,
            Title = string.Format(titleTemplate, feedbackId),
            Message = string.Format(bodyTemplate, feedbackId, feedbackTitle),
            IsGlobal = false,
            NotificationType = nameof(NotificationTypes.Push),
            CreatedBy = currentUserEmail
        };
        await notificationsService.CreateNewNotificationAsync(
            request: notificationsDomainModel,
            cancellationToken: cancellationToken
        ).ConfigureAwait(false);
    }
}
