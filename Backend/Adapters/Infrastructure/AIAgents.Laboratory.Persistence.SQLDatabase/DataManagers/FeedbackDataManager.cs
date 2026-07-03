using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Models.Feedback;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Persistence.SQLDatabase.Contracts;
using AIAgents.Laboratory.Persistence.SQLDatabase.Mapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Persistence.SQLDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.DataManagers;

/// <summary>
/// Provides operations for managing bug reports and feature requests, including adding new entries and retrieving submitted data.
/// </summary>
/// <remarks>This class is intended for use in scenarios where feedback data, such as bug reports and feature requests, must be managed in a consistent and auditable manner. 
/// All operations are performed asynchronously and are logged for traceability. Thread safety is ensured by the stateless nature of the manager and its reliance on injected dependencies.</remarks>
/// <param name="logger">The logger used for recording informational and error messages during operations.</param>
/// <param name="correlationContext">The correlation context used to track request correlation identifiers for logging and exception handling.</param>
/// <param name="feedbackRepository">The feedback data repository service used to manipulate the data from database for the feedback entities.</param>
/// <seealso cref="IFeedbackDataManager"/>
public sealed class FeedbackDataManager(
    ILogger<FeedbackDataManager> logger,
    ICorrelationContext correlationContext,
    IFeedbackRepository feedbackRepository) : IFeedbackDataManager
{
    /// <inheritdoc/>
    public async Task<bool> AddNewBugReportDataAsync(
        BugReportData bugReportData,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(AddNewBugReportDataAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, bugReportData })
            );

            var entityData = DataMapperProfile.MapToEntity(
                domainInput: bugReportData
            );
            response = await feedbackRepository.AddNewBugReportDataAsync(
                bugReportData: entityData,
                cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(AddNewBugReportDataAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(AddNewBugReportDataAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, bugReportData, response })
            );
        }
    }

    /// <inheritdoc/>
    public async Task<bool> AddNewFeatureRequestDataAsync(
        NewFeatureRequestData featureRequestData,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, featureRequestData })
            );

            var entityData = DataMapperProfile.MapToEntity(
                domainInput: featureRequestData
            );
            response = await feedbackRepository.AddNewFeatureRequestDataAsync(
                featureRequestData: entityData,
                cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(AddNewFeatureRequestDataAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, featureRequestData, response })
            );
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<BugReportData>> GetAllBugReportsDataAsync(
        string currentLoggedinUser,
        CancellationToken cancellationToken = default
    )
    {
        IEnumerable<BugReportData> response = [];
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(GetAllBugReportsDataAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser })
            );

            var dbResponse = await feedbackRepository.GetAllBugReportsDataAsync(
                currentLoggedinUser,
                cancellationToken
            ).ConfigureAwait(false);
            response = [.. dbResponse.Select(DataMapperProfile.MapToDomain)];
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(GetAllBugReportsDataAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser, response })
            );
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NewFeatureRequestData>> GetAllSubmittedFeatureRequestsAsync(
        string currentLoggedinUser,
        CancellationToken cancellationToken = default
    )
    {
        IEnumerable<NewFeatureRequestData> response = [];
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser })
            );

            var dbResponse = await feedbackRepository.GetAllSubmittedFeatureRequestsAsync(
                currentLoggedinUser,
                cancellationToken
            ).ConfigureAwait(false);
            response = [.. dbResponse.Select(DataMapperProfile.MapToDomain)];
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser, response })
            );
        }
    }
}
