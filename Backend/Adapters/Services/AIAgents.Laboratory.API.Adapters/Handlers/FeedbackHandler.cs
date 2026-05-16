using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Mapper;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.Domain.Ports.In;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The feedback api handler adapter.
/// </summary>
/// <param name="feedbackService">The feedback service.</param>
/// <seealso cref="IFeedbackHandler"/>
public sealed class FeedbackHandler(IFeedbackService feedbackService) : IFeedbackHandler
{
    /// <inheritdoc/>
    public async Task<bool> AddNewBugReportDataAsync(AddBugReportDTO bugReportData, CancellationToken cancellationToken = default)
    {
        var domainInput = DomainMapperProfile.MapToDomain(bugReportData);
        return await feedbackService.AddNewBugReportDataAsync(bugReportData: domainInput).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> AddNewFeatureRequestDataAsync(NewFeatureRequestDTO featureRequestData, CancellationToken cancellationToken = default)
    {
        var domainInput = DomainMapperProfile.MapToDomain(featureRequestData);
        return await feedbackService.AddNewFeatureRequestDataAsync(featureRequestData: domainInput).ConfigureAwait(false);
    }
}
