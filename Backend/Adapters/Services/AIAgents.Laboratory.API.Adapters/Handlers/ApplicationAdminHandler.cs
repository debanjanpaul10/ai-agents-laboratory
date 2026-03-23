using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The <c>ApplicationAdminHandler</c> class is responsible for handling application administration related operations, such as retrieving bug reports and feature requests. 
/// <remarks>It implements the <see cref="IApplicationAdminHandler"/> interface and uses an instance of <see cref="IApplicationAdminService"/> to perform the necessary operations. 
/// The results are then mapped to the appropriate DTOs using AutoMapper before being returned to the caller.</remarks>
/// </summary>
/// <param name="mapper">The auto mapper service.</param>
/// <param name="applicationAdminService">The application admin service.</param>
/// <seealso cref="IApplicationAdminHandler"/>
public sealed class ApplicationAdminHandler(IMapper mapper, IApplicationAdminService applicationAdminService) : IApplicationAdminHandler
{
    /// <summary>
    /// Gets all bug reports data asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <returns>A list of <see cref="BugReportDataDto"/></returns>
    public async Task<IEnumerable<BugReportDataDto>> GetAllBugReportsDataAsync(string currentLoggedinUser)
    {
        var domainResult = await applicationAdminService.GetAllBugReportsDataAsync(currentLoggedinUser).ConfigureAwait(false);
        return mapper.Map<IEnumerable<BugReportDataDto>>(domainResult);
    }

    /// <summary>
    /// Gets all submitted feature requests asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <returns>A list of <see cref="NewFeatureRequestDataDto"/></returns>
    public async Task<IEnumerable<NewFeatureRequestDataDto>> GetAllSubmittedFeatureRequestsAsync(string currentLoggedinUser)
    {
        var domainResult = await applicationAdminService.GetAllSubmittedFeatureRequestsAsync(currentLoggedinUser).ConfigureAwait(false);
        return mapper.Map<IEnumerable<NewFeatureRequestDataDto>>(domainResult);
    }

    /// <summary>
    /// Checks if the admin access is enabled for the current logged in user asynchronous.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <returns>The boolean for success/failure.</returns>
    public bool IsAdminAccessEnabledAsync(string currentLoggedInUser)
    {
        return applicationAdminService.IsAdminAccessEnabledAsync(currentLoggedInUser);
    }
}
