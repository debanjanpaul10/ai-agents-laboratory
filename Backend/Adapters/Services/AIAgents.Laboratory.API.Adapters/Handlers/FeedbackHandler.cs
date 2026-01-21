using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The feedback api handler adapter.
/// </summary>
/// <param name="mapper">The auto mapper.</param>
/// <param name="feedbackService">The feedback service.</param>
/// <seealso cref="IFeedbackHandler"/>
public sealed class FeedbackHandler(IMapper mapper, IFeedbackService feedbackService) : IFeedbackHandler
{
    /// <summary>
    /// Adds the new bug report data asynchronous.
    /// </summary>
    /// <param name="bugReportData">The bug report data.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> AddNewBugReportDataAsync(AddBugReportDTO bugReportData)
    {
        var domainInput = mapper.Map<BugReportData>(bugReportData);
        return await feedbackService.AddNewBugReportDataAsync(domainInput).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds the new feature request data asynchronous.
    /// </summary>
    /// <param name="featureRequestData">The feature request data.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> AddNewFeatureRequestDataAsync(NewFeatureRequestDTO featureRequestData)
    {
        var domainInput = mapper.Map<NewFeatureRequestData>(featureRequestData);
        return await feedbackService.AddNewFeatureRequestDataAsync(domainInput).ConfigureAwait(false);
    }
}
