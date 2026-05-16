using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Persistence.SQLDatabase.Mapper;
using AIAgents.Laboratory.Persistence.SQLDatabase.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Persistence.SQLDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.DataManagers;

/// <summary>
/// Provides operations for managing bug reports and feature requests, including adding new entries and retrieving submitted data.
/// </summary>
/// <remarks>This class is intended for use in scenarios where feedback data, such as bug reports and feature requests, must be managed in a consistent and auditable manner. 
/// All operations are performed asynchronously and are logged for traceability. Thread safety is ensured by the stateless nature of the manager and its reliance on injected dependencies.</remarks>
/// <param name="unitOfWork">The unit of work used to coordinate repository operations and persist changes.</param>
/// <param name="logger">The logger used for recording informational and error messages during operations.</param>
/// <param name="correlationContext">The correlation context used to track request correlation identifiers for logging and exception handling.</param>
/// <seealso cref="IFeedbackDataManager"/>
public sealed class FeedbackDataManager(
    IUnitOfWork unitOfWork,
    ILogger<FeedbackDataManager> logger,
    ICorrelationContext correlationContext) : IFeedbackDataManager
{
    /// <inheritdoc/>
    public async Task<bool> AddNewBugReportDataAsync(
        BugReportData bugReportData,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.MethodStartedMessageConstant, nameof(AddNewBugReportDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, bugReportData }));

            var entityData = DataMapperProfile.MapToEntity(domainInput: bugReportData);
            var bugStatusEntity = await unitOfWork.Repository<BugItemStatusMappingEntity>()
                .FirstOrDefaultAsync(status => status.StatusName == DatabaseConstants.NotStartedConstant && status.IsActive, cancellationToken)
                .ConfigureAwait(false);

            bugReportData.BugStatusId = bugStatusEntity?.Id ?? 0;

            await unitOfWork.Repository<BugReportDataEntity>()
                .AddAsync(entityData, cancellationToken)
                .ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.MethodEndedMessageConstant, nameof(AddNewBugReportDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, bugReportData }));
        }
    }

    /// <inheritdoc/>
    public async Task<bool> AddNewFeatureRequestDataAsync(
        NewFeatureRequestData featureRequestData,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.MethodStartedMessageConstant, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, featureRequestData }));

            var entityData = DataMapperProfile.MapToEntity(domainInput: featureRequestData);

            await unitOfWork.Repository<NewFeatureRequestDataEntity>()
                .AddAsync(entityData, cancellationToken)
                .ConfigureAwait(false);

            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.MethodEndedMessageConstant, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, featureRequestData }));
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<BugReportData>> GetAllBugReportsDataAsync(
        string currentLoggedinUser,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.MethodStartedMessageConstant, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));

            var result = await unitOfWork.Repository<BugReportDataEntity>()
                .GetAllAsync(x => x.IsActive, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            return [.. result.Select(DataMapperProfile.MapToDomain)];
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.MethodEndedMessageConstant, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NewFeatureRequestData>> GetAllSubmittedFeatureRequestsAsync(
        string currentLoggedinUser,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.MethodStartedMessageConstant, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));

            var result = await unitOfWork.Repository<NewFeatureRequestDataEntity>()
                .GetAllAsync(x => x.IsActive, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            return [.. result.Select(DataMapperProfile.MapToDomain)];
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.MethodEndedMessageConstant, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));
        }
    }
}
