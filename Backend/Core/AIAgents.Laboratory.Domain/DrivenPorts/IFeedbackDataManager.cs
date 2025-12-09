using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;

namespace AIAgents.Laboratory.Domain.DrivenPorts;

/// <summary>
/// The contract for Feedback data manager service.
/// </summary>
public interface IFeedbackDataManager
{
    /// <summary>
    /// Adds the new bug report data asynchronous.
    /// </summary>
    /// <param name="bugReportData">The bug report data.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> AddNewBugReportDataAsync(BugReportData bugReportData);

    /// <summary>
    /// Adds the new feature request data asynchronous.
    /// </summary>
    /// <param name="featureRequestData">The feature request data.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> AddNewFeatureRequestDataAsync(NewFeatureRequestData featureRequestData);
}
