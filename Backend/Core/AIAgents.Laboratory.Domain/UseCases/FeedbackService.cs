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
/// The Feedback Service class.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="configuration">The configuration.</param>
/// <param name="feedbackDataManager">The feedback data manager.</param>
/// <param name="emailNotificationService">The email notification service.</param>
/// <seealso cref="IFeedbackService"/>
public sealed class FeedbackService(ILogger<FeedbackService> logger, IConfiguration configuration, IFeedbackDataManager feedbackDataManager, IEmailNotificationService emailNotificationService) : IFeedbackService
{
    /// <summary>
    /// The boolean flag to check if feedback feature is enabled.
    /// </summary>
    private readonly bool IS_FEEDBACK_FEATURE_ENABLED = bool.TryParse(configuration[AzureAppConfigurationConstants.IsFeedbackFeatureEnabled], out var isEnabled) && isEnabled;

    /// <summary>
    /// The admin email address from configuration.
    /// </summary>
    private readonly string ADMIN_EMAIL_ADDRESS = configuration[AzureAppConfigurationConstants.AdminEmailAddressConstant] ?? throw new Exception(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

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
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(bugReportData));
            if (IS_FEEDBACK_FEATURE_ENABLED)
            {
                bugReportData.PrepareBugReportDataDomain();
                await feedbackDataManager.AddNewBugReportDataAsync(bugReportData).ConfigureAwait(false);
                await emailNotificationService.SendEmailNotificationAsync(
                    subject: bugReportData.Title,
                    content: string.Format(FeedbackTemplateConstants.EmailTemplateHtml, bugReportData.Title, bugReportData.Description, bugReportData.CreatedBy),
                    recipient: ADMIN_EMAIL_ADDRESS).ConfigureAwait(false);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(bugReportData));
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
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(featureRequestData));
            if (IS_FEEDBACK_FEATURE_ENABLED)
            {
                featureRequestData.PrepareNewFeatureRequestDataDomain();
                await feedbackDataManager.AddNewFeatureRequestDataAsync(featureRequestData).ConfigureAwait(false);
                await emailNotificationService.SendEmailNotificationAsync(
                    subject: featureRequestData.Title,
                    content: string.Format(FeedbackTemplateConstants.EmailTemplateHtml, featureRequestData.Title, featureRequestData.Description, featureRequestData.CreatedBy),
                    recipient: ADMIN_EMAIL_ADDRESS).ConfigureAwait(false);

                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(featureRequestData));
        }
    }
}
