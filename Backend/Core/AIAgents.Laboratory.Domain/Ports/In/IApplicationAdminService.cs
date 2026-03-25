using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;

namespace AIAgents.Laboratory.Domain.Ports.In;

/// <summary>
/// The contract for application admin service, which is responsible for handling application administration related operations, such as retrieving bug reports and feature requests.
/// </summary>
public interface IApplicationAdminService
{
    /// <summary>
    /// Gets all bug reports data asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="BugReportData"/></returns>
    Task<IEnumerable<BugReportData>> GetAllBugReportsDataAsync(string currentLoggedinUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all submitted feature requests asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="NewFeatureRequestData"/></returns>
    Task<IEnumerable<NewFeatureRequestData>> GetAllSubmittedFeatureRequestsAsync(string currentLoggedinUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if the admin access is enabled for the current logged in user asynchronous.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <returns>The boolean for success/failure.</returns>
    bool IsAdminAccessEnabledAsync(string currentLoggedInUser);
}
