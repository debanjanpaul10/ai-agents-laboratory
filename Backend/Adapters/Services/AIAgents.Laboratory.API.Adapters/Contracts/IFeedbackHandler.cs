using AIAgents.Laboratory.API.Adapters.Models.Request;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// The contract for feedback api handler.
/// </summary>
public interface IFeedbackHandler
{
    /// <summary>
    /// Adds the new bug report data asynchronous.
    /// </summary>
    /// <param name="bugReportData">The bug report data.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> AddNewBugReportDataAsync(AddBugReportDTO bugReportData);

    /// <summary>
    /// Adds the new feature request data asynchronous.
    /// </summary>
    /// <param name="featureRequestData">The feature request data.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> AddNewFeatureRequestDataAsync(NewFeatureRequestDTO featureRequestData);
}
