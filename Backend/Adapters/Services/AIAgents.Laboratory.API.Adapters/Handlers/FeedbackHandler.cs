using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.Domain.Ports.In;
using static AIAgents.Laboratory.API.Adapters.Mapper.RequestToDomainMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The feedback api handler adapter.
/// </summary>
/// <param name="feedbackService">The feedback service.</param>
/// <seealso cref="IFeedbackHandler"/>
public sealed class FeedbackHandler(IFeedbackService feedbackService) : IFeedbackHandler
{
    /// <inheritdoc/>
    public async Task<bool> AddNewBugReportDataAsync(
        AddBugReportDTO bugReportData,
        CancellationToken cancellationToken = default
    )
    {
        var domainInput = MapToDomain(bugReportData);
        return await feedbackService.AddNewBugReportDataAsync(
            bugReportData: domainInput,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> AddNewFeatureRequestDataAsync(
        NewFeatureRequestDTO featureRequestData,
        CancellationToken cancellationToken = default
    )
    {
        var domainInput = MapToDomain(featureRequestData);
        return await feedbackService.AddNewFeatureRequestDataAsync(
            featureRequestData: domainInput,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
