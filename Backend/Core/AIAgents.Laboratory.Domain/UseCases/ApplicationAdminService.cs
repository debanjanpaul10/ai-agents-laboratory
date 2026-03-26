using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.In;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The class is the implementation of <see cref="IApplicationAdminService"/> contract, 
/// which is responsible for handling application administration related operations, such as retrieving bug reports and feature requests.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="correlationContext">The correlation context for logging.</param>
/// <param name="feedbackService">The feedback service.</param>
/// <param name="commonAiService">The common ai service.</param>
public sealed class ApplicationAdminService(ILogger<ApplicationAdminService> logger, ICorrelationContext correlationContext, IFeedbackService feedbackService, ICommonAiService commonAiService) : IApplicationAdminService
{
    /// <summary>
    /// Gets all bug reports data asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="BugReportData"/></returns>
    public async Task<IEnumerable<BugReportData>> GetAllBugReportsDataAsync(string currentLoggedinUser, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));

            ArgumentException.ThrowIfNullOrWhiteSpace(currentLoggedinUser);
            return await feedbackService.GetAllBugReportsDataAsync(
                currentLoggedinUser,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));
        }
    }

    /// <summary>
    /// Gets all submitted feature requests asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="NewFeatureRequestData"/></returns>
    public async Task<IEnumerable<NewFeatureRequestData>> GetAllSubmittedFeatureRequestsAsync(string currentLoggedinUser, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));

            ArgumentException.ThrowIfNullOrWhiteSpace(currentLoggedinUser);
            return await feedbackService.GetAllSubmittedFeatureRequestsAsync(
                currentLoggedinUser,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedinUser }));
        }
    }

    /// <summary>
    /// Checks if the admin access is enabled for the current logged in user asynchronous.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <returns>The boolean for success/failure.</returns>
    public bool IsAdminAccessEnabledAsync(string currentLoggedInUser)
    {
        try
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodStart, nameof(IsAdminAccessEnabledAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser }));

            ArgumentException.ThrowIfNullOrWhiteSpace(currentLoggedInUser);
            var keyValue = commonAiService.GetConfigurationByKeyName(
                key: AzureAppConfigurationConstants.AdminEmailAddressConstant
            ).Values.First() ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

            return keyValue == currentLoggedInUser;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggingConstants.LogHelperMethodFailed, nameof(IsAdminAccessEnabledAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggingConstants.LogHelperMethodEnd, nameof(IsAdminAccessEnabledAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, currentLoggedInUser }));
        }
    }
}
