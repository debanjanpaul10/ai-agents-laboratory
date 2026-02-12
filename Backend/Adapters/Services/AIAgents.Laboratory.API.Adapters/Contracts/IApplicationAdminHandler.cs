using AIAgents.Laboratory.API.Adapters.Models.Response;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// The interface defines the contract for handling application administration related operations, such as retrieving bug reports and feature requests.
/// </summary>
public interface IApplicationAdminHandler
{
    /// <summary>
    /// Gets all bug reports data asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <returns>A list of <see cref="BugReportDataDto"/></returns>
    Task<IEnumerable<BugReportDataDto>> GetAllBugReportsDataAsync(string currentLoggedinUser);

    /// <summary>
    /// Gets all submitted feature requests asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <returns>A list of <see cref="NewFeatureRequestDataDto"/></returns>
    Task<IEnumerable<NewFeatureRequestDataDto>> GetAllSubmittedFeatureRequestsAsync(string currentLoggedinUser);

    /// <summary>
    /// Checks if the admin access is enabled for the current logged in user asynchronous.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <returns>The boolean for success/failure.</returns>
    bool IsAdminAccessEnabledAsync(string currentLoggedInUser);
}
