using System.Globalization;
using System.Text.Json;
using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The Feedback Service class.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="configuration">The configuration.</param>
/// <param name="feedbackDataManager">The feedback data manager.</param>
/// <seealso cref="IFeedbackService"/>
public sealed class FeedbackService(ILogger<FeedbackService> logger, IConfiguration configuration, IFeedbackDataManager feedbackDataManager) : IFeedbackService
{
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
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, JsonSerializer.Serialize(bugReportData)));

            var isFeedbackFeatureEnabled = bool.TryParse(configuration[AzureAppConfigurationConstants.IsFeedbackFeatureEnabled], out var isEnabled) && isEnabled;
            if (isFeedbackFeatureEnabled)
            {
                bugReportData.PrepareBugReportDataDomain();
                return await feedbackDataManager.AddNewBugReportDataAsync(bugReportData).ConfigureAwait(false);
            }

            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, JsonSerializer.Serialize(bugReportData)));
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
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, JsonSerializer.Serialize(featureRequestData)));
            var isFeedbackFeatureEnabled = bool.TryParse(configuration[AzureAppConfigurationConstants.IsFeedbackFeatureEnabled], out var isEnabled) && isEnabled;
            if (!isFeedbackFeatureEnabled)
            {
                return false;
            }
            else
            {
                featureRequestData.PrepareNewFeatureRequestDataDomain();
                return await feedbackDataManager.AddNewFeatureRequestDataAsync(featureRequestData).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, JsonSerializer.Serialize(featureRequestData)));
        }
    }
}
