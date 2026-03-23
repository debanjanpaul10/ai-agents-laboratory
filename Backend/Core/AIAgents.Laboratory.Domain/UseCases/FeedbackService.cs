using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
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
/// <param name="emailNotificationService">The email notification service.</param>
/// <seealso cref="IFeedbackService"/>
public sealed class FeedbackService(ILogger<FeedbackService> logger, IConfiguration configuration, ICorrelationContext correlationContext,
    IFeedbackDataManager feedbackDataManager, IEmailNotificationService emailNotificationService) : IFeedbackService
{
    /// <summary>
    /// The admin email address from configuration.
    /// </summary>
    private readonly string ADMIN_EMAIL_ADDRESS = configuration[AzureAppConfigurationConstants.AdminEmailAddressConstant] ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// Adds the new bug report data asynchronous.
    /// </summary>
    /// <param name="bugReportData">The bug report data.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> AddNewBugReportDataAsync(BugReportData bugReportData)
    {
        ArgumentNullException.ThrowIfNull(bugReportData);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, bugReportData }));

            bugReportData.PrepareAuditEntityData(bugReportData.CreatedBy);
            var feedbackSaveResult = await feedbackDataManager.AddNewBugReportDataAsync(bugReportData).ConfigureAwait(false);

            var template = await File.ReadAllTextAsync(FeedbackTemplateConstants.FileName).ConfigureAwait(false);
            var emailSendResult = await emailNotificationService.SendEmailNotificationAsync(
                subject: bugReportData.Title,
                content: string.Format(template, bugReportData.Title, bugReportData.Description, bugReportData.CreatedBy),
                recipient: ADMIN_EMAIL_ADDRESS).ConfigureAwait(false);

            return feedbackSaveResult && emailSendResult;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, bugReportData }));
        }
    }

    /// <summary>
    /// Adds the new feature request data asynchronous.
    /// </summary>
    /// <param name="featureRequestData">The feature request data.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> AddNewFeatureRequestDataAsync(NewFeatureRequestData featureRequestData)
    {
        ArgumentNullException.ThrowIfNull(featureRequestData);

        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, featureRequestData }));

            featureRequestData.PrepareAuditEntityData(featureRequestData.CreatedBy);
            var feedbackSaveResult = await feedbackDataManager.AddNewFeatureRequestDataAsync(featureRequestData).ConfigureAwait(false);

            var template = await File.ReadAllTextAsync(FeedbackTemplateConstants.FileName).ConfigureAwait(false);
            var emailSendResult = await emailNotificationService.SendEmailNotificationAsync(
                subject: featureRequestData.Title,
                content: string.Format(template, featureRequestData.Title, featureRequestData.Description, featureRequestData.CreatedBy),
                recipient: ADMIN_EMAIL_ADDRESS).ConfigureAwait(false);

            return feedbackSaveResult && emailSendResult;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, featureRequestData }));
        }
    }

    /// <summary>
    /// Gets all bug reports data asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <returns>A list of <see cref="BugReportData"/></returns>
    public async Task<IEnumerable<BugReportData>> GetAllBugReportsDataAsync(string currentLoggedinUser)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));

            return await this.GetRespectiveFeedbackResponseAsync(
                currentUserEmail: currentLoggedinUser,
                dataManagerMethod: feedbackDataManager.GetAllBugReportsDataAsync).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));
        }
    }

    /// <summary>
    /// Gets all submitted feature requests asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <returns>A list of <see cref="NewFeatureRequestData"/></returns>
    public async Task<IEnumerable<NewFeatureRequestData>> GetAllSubmittedFeatureRequestsAsync(string currentLoggedinUser)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));

            return await this.GetRespectiveFeedbackResponseAsync(
                currentUserEmail: currentLoggedinUser,
                dataManagerMethod: feedbackDataManager.GetAllSubmittedFeatureRequestsAsync).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));
        }
    }

    #region PRIVATE METHODS

    /// <summary>
    /// Gets the respective feedback response based on the user email. If the user is admin, it will return the data from data manager, otherwise it will throw unauthorized access exception.
    /// </summary>
    /// <typeparam name="TResponse">The response type inferred.</typeparam>
    /// <param name="currentUserEmail">The current logged in user email address.</param>
    /// <param name="dataManagerMethod">The data manager method to perform the respective action.</param>
    /// <returns>The response type inferred from <see cref="TResponse"/>.</returns>
    /// <exception cref="UnauthorizedAccessException">If user is not admin, return unauthentication error.</exception>
    private async Task<TResponse> GetRespectiveFeedbackResponseAsync<TResponse>(string currentUserEmail, Func<string, Task<TResponse>> dataManagerMethod)
    {
        if (currentUserEmail == ADMIN_EMAIL_ADDRESS)
            return await dataManagerMethod(currentUserEmail).ConfigureAwait(false);
        else
            throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);
    }

    #endregion
}
