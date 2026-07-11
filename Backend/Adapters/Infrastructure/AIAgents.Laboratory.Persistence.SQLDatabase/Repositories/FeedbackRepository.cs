using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Persistence.SQLDatabase.Contracts;
using AIAgents.Laboratory.Persistence.SQLDatabase.Models;
using static AIAgents.Laboratory.Persistence.SQLDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.Repositories;

/// <summary>
/// Implements the <see cref="IFeedbackRepository"/> interface, providing methods for adding and retrieving bug reports and feature requests from the SQL database.
/// </summary>
/// <param name="unitOfWork">The unit of work.</param>
/// <seealso cref="IFeedbackRepository"/>
public sealed class FeedbackRepository(IUnitOfWork unitOfWork) : IFeedbackRepository
{
    /// <inheritdoc/>
    public async Task<bool> AddNewBugReportDataAsync(
        BugReportDataEntity bugReportData,
        CancellationToken cancellationToken = default
    )
    {
        var bugStatusEntity = await unitOfWork.Repository<BugItemStatusMappingEntity>().FirstOrDefaultAsync(
                predicate: status => status.StatusName == DatabaseConstants.NotStartedConstant && status.IsActive,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

        bugReportData.BugStatusId = bugStatusEntity?.Id ?? 0;

        await unitOfWork.Repository<BugReportDataEntity>()
            .AddAsync(entity: bugReportData, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        await unitOfWork.SaveChangesAsync(
            cancellationToken
        ).ConfigureAwait(false);

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> AddNewFeatureRequestDataAsync(
        NewFeatureRequestDataEntity featureRequestData,
        CancellationToken cancellationToken = default
    )
    {
        await unitOfWork.Repository<NewFeatureRequestDataEntity>()
            .AddAsync(entity: featureRequestData, cancellationToken)
            .ConfigureAwait(false);

        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<BugReportDataEntity>> GetAllBugReportsDataAsync(
        string currentLoggedinUser,
        CancellationToken cancellationToken = default
    ) =>
        await unitOfWork.Repository<BugReportDataEntity>()
            .GetAllAsync(filter: x => x.IsActive, cancellationToken: cancellationToken)
            .ConfigureAwait(false);


    /// <inheritdoc/>
    public async Task<IEnumerable<NewFeatureRequestDataEntity>> GetAllSubmittedFeatureRequestsAsync(
        string currentLoggedinUser,
        CancellationToken cancellationToken = default
    ) =>
        await unitOfWork.Repository<NewFeatureRequestDataEntity>()
            .GetAllAsync(filter: x => x.IsActive, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
}