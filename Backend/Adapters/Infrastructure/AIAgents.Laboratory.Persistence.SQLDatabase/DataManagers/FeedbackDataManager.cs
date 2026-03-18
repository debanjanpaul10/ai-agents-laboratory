using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Persistence.SQLDatabase.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Persistence.SQLDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.DataManagers;

/// <summary>
/// Provides operations for managing bug reports and feature requests, including adding new entries and retrieving submitted data.
/// </summary>
/// <remarks>This class is intended for use in scenarios where feedback data, such as bug reports and feature requests, must be managed in a consistent and auditable manner. 
/// All operations are performed asynchronously and are logged for traceability. Thread safety is ensured by the stateless nature of the manager and its reliance on injected dependencies.</remarks>
/// <param name="mapper">The mapper used to convert between domain models and data entities.</param>
/// <param name="unitOfWork">The unit of work used to coordinate repository operations and persist changes.</param>
/// <param name="logger">The logger used for recording informational and error messages during operations.</param>
/// <param name="correlationContext">The correlation context used to track request correlation identifiers for logging and exception handling.</param>
/// <seealso cref="IFeedbackDataManager"/>
public sealed class FeedbackDataManager(IMapper mapper, IUnitOfWork unitOfWork, ILogger<FeedbackDataManager> logger, ICorrelationContext correlationContext) : IFeedbackDataManager
{
    /// <summary>
    /// Adds the new bug report data asynchronous.
    /// </summary>
    /// <param name="bugReportData">The bug report data.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> AddNewBugReportDataAsync(BugReportData bugReportData)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.MethodStartedMessageConstant, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, bugReportData }));

            var entityData = mapper.Map<BugReportDataEntity>(bugReportData);
            var bugStatusEntity = await unitOfWork.Repository<BugItemStatusMappingEntity>().FirstOrDefaultAsync(status => status.StatusName == DatabaseConstants.NotStartedConstant && status.IsActive);
            bugReportData.BugStatusId = bugStatusEntity?.Id ?? 0;

            await unitOfWork.Repository<BugReportDataEntity>().AddAsync(entityData).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.MethodEndedMessageConstant, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, bugReportData }));
        }
    }

    /// <summary>
    /// Adds the new feature request data asynchronous.
    /// </summary>
    /// <param name="featureRequestData">The feature request data.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> AddNewFeatureRequestDataAsync(NewFeatureRequestData featureRequestData)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.MethodStartedMessageConstant, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, featureRequestData }));

            var entityData = mapper.Map<NewFeatureRequestDataEntity>(featureRequestData);

            await unitOfWork.Repository<NewFeatureRequestDataEntity>().AddAsync(entityData).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.MethodEndedMessageConstant, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, featureRequestData }));
        }
    }

    /// <summary>
    /// Gets all bug reports data asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <returns>A list of <see cref="BugReportData"/></returns>
    public async Task<IEnumerable<BugReportData>> GetAllBugReportsDataAsync(string currentLoggedinUser)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.MethodStartedMessageConstant, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));

            var result = await unitOfWork.Repository<BugReportDataEntity>().GetAllAsync(x => x.IsActive).ConfigureAwait(false);
            return mapper.Map<IEnumerable<BugReportData>>(result);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.MethodEndedMessageConstant, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));
        }
    }

    /// <summary>
    /// Gets all submitted feature requests asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <returns>A list of <see cref="NewFeatureRequestData"/></returns>
    public async Task<IEnumerable<NewFeatureRequestData>> GetAllSubmittedFeatureRequestsAsync(string currentLoggedinUser)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.MethodStartedMessageConstant, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));

            var result = await unitOfWork.Repository<NewFeatureRequestDataEntity>().GetAllAsync(x => x.IsActive).ConfigureAwait(false);
            return mapper.Map<IEnumerable<NewFeatureRequestData>>(result);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.MethodEndedMessageConstant, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));
        }
    }
}
