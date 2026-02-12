using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The class is the implementation of <see cref="IApplicationAdminService"/> contract, 
/// which is responsible for handling application administration related operations, such as retrieving bug reports and feature requests.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="feedbackService">The feedback service.</param>
/// <param name="commonAiService">The common ai service.</param>
public sealed class ApplicationAdminService(ILogger<ApplicationAdminService> logger, IFeedbackService feedbackService, ICommonAiService commonAiService) : IApplicationAdminService
{
    /// <summary>
    /// Gets all bug reports data asynchronous.
    /// </summary>
    /// <param name="currentLoggedinUser">The current logged in user.</param>
    /// <returns>A list of <see cref="BugReportData"/></returns>
    public async Task<IEnumerable<BugReportData>> GetAllBugReportsDataAsync(string currentLoggedinUser)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, currentLoggedinUser);

            ArgumentException.ThrowIfNullOrWhiteSpace(currentLoggedinUser);
            return await feedbackService.GetAllBugReportsDataAsync(currentLoggedinUser).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllBugReportsDataAsync), DateTime.UtcNow, currentLoggedinUser);
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
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, currentLoggedinUser);

            ArgumentException.ThrowIfNullOrWhiteSpace(currentLoggedinUser);
            return await feedbackService.GetAllSubmittedFeatureRequestsAsync(currentLoggedinUser).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllSubmittedFeatureRequestsAsync), DateTime.UtcNow, currentLoggedinUser);
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
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(IsAdminAccessEnabledAsync), DateTime.UtcNow, currentLoggedInUser);

            ArgumentException.ThrowIfNullOrWhiteSpace(currentLoggedInUser);
            var keyValue = commonAiService.GetConfigurationByKeyName(AzureAppConfigurationConstants.AdminEmailAddressConstant).Values.First()
                ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

            return keyValue == currentLoggedInUser;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(IsAdminAccessEnabledAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message);
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(IsAdminAccessEnabledAsync), DateTime.UtcNow, currentLoggedInUser);
        }
    }
}
