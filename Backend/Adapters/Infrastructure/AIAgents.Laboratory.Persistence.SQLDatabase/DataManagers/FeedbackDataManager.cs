using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Persistence.SQLDatabase.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Persistence.SQLDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.DataManagers;

/// <summary>
/// The feedback data manager service implementation.
/// </summary>
/// <param name="unitOfWork">The unit of work.</param>
/// <param name="logger">The logger service.</param>
/// <seealso cref="IFeedbackDataManager" />
public sealed class FeedbackDataManager(IUnitOfWork unitOfWork, ILogger<FeedbackDataManager> logger) : IFeedbackDataManager
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
            logger.LogInformation(LoggingConstants.MethodStartedMessageConstant, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(bugReportData));

            var bugStatusEntity = await unitOfWork.Repository<BugItemStatusMapping>().FirstOrDefaultAsync(status => status.StatusName == DatabaseConstants.NotStartedConstant && status.IsActive);
            bugReportData.BugStatusId = bugStatusEntity?.Id ?? 0;

            await unitOfWork.Repository<BugReportData>().AddAsync(bugReportData).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.MethodEndedMessageConstant, nameof(AddNewBugReportDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(bugReportData));
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
            logger.LogInformation(LoggingConstants.MethodStartedMessageConstant, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, featureRequestData.CreatedBy);

            await unitOfWork.Repository<NewFeatureRequestData>().AddAsync(featureRequestData).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.MethodEndedMessageConstant, nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, featureRequestData.CreatedBy);
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
            logger.LogInformation(LoggingConstants.MethodStartedMessageConstant, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, currentLoggedinUser);
            return await unitOfWork.Repository<BugReportData>().GetAllAsync(x => x.IsActive).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.MethodEndedMessageConstant, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, currentLoggedinUser);
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
            logger.LogInformation(LoggingConstants.MethodStartedMessageConstant, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, currentLoggedinUser);
            return await unitOfWork.Repository<NewFeatureRequestData>().GetAllAsync(x => x.IsActive).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.MethodFailedWithMessageConstant, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.MethodEndedMessageConstant, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, currentLoggedinUser);
        }
    }
}
