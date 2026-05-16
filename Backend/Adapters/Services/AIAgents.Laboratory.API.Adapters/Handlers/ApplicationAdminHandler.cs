using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Mapper;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.Ports.In;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The <c>ApplicationAdminHandler</c> class is responsible for handling application administration related operations, such as retrieving bug reports and feature requests. 
/// <remarks>It implements the <see cref="IApplicationAdminHandler"/> interface and uses an instance of <see cref="IApplicationAdminService"/> to perform the necessary operations. 
/// The results are then mapped to the appropriate DTOs using <see cref="DomainMapperProfile"/> before being returned to the caller.</remarks>
/// </summary>
/// <param name="applicationAdminService">The application admin service.</param>
/// <seealso cref="IApplicationAdminHandler"/>
public sealed class ApplicationAdminHandler(IApplicationAdminService applicationAdminService) : IApplicationAdminHandler
{
    /// <inheritdoc/>
    public async Task<IEnumerable<BugReportDataDto>> GetAllBugReportsDataAsync(string currentLoggedinUser, CancellationToken cancellationToken = default)
    {
        var domainResult = await applicationAdminService.GetAllBugReportsDataAsync(
            currentLoggedinUser,
            cancellationToken
        ).ConfigureAwait(false);
        return [.. domainResult.Select(DomainMapperProfile.MapToDto)];
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NewFeatureRequestDataDto>> GetAllSubmittedFeatureRequestsAsync(string currentLoggedinUser, CancellationToken cancellationToken = default)
    {
        var domainResult = await applicationAdminService.GetAllSubmittedFeatureRequestsAsync(
            currentLoggedinUser,
            cancellationToken
        ).ConfigureAwait(false);
        return [.. domainResult.Select(DomainMapperProfile.MapToDto)];
    }

    /// <inheritdoc/>
    public bool IsAdminAccessEnabledAsync(string currentLoggedInUser)
    {
        return applicationAdminService.IsAdminAccessEnabledAsync(currentLoggedInUser);
    }
}
