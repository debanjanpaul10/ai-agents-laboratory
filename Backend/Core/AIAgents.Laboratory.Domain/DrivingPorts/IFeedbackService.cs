using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;

namespace AIAgents.Laboratory.Domain.DrivingPorts;

/// <summary>
/// The contract for feedback service.
/// </summary>
public interface IFeedbackService
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
