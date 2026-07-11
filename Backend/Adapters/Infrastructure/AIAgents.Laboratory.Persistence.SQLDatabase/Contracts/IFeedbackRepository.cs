using AIAgents.Laboratory.Persistence.SQLDatabase.Models;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.Contracts;

/// <summary>
/// Defines the contract for the Feedback repository, providing methods for adding and retrieving bug reports and feature requests from the SQL database.
/// </summary>
public interface IFeedbackRepository
{
    /// <summary>
    /// Adds the new bug report data asynchronous.
    /// </summary>
    /// <param name="bugReportData">The bug report data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> AddNewBugReportDataAsync(
        BugReportDataEntity bugReportData,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Adds the new feature request data asynchronous.
    /// </summary>
    /// <param name="featureRequestData">The feature request data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> AddNewFeatureRequestDataAsync(
        NewFeatureRequestDataEntity featureRequestData,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets all bug reports data asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="BugReportData"/></returns>
    Task<IEnumerable<BugReportDataEntity>> GetAllBugReportsDataAsync(
        string currentLoggedinUser,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets all submitted feature requests asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="NewFeatureRequestData"/></returns>
    Task<IEnumerable<NewFeatureRequestDataEntity>> GetAllSubmittedFeatureRequestsAsync(
        string currentLoggedinUser,
        CancellationToken cancellationToken = default
    );
}