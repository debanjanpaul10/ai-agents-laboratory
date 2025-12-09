using System.Globalization;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The plugins ai services.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="aiAgentServices">The AI agent services.</param>
public class PluginsAiService(ILogger<PluginsAiService> logger, IAiServices aiAgentServices) : IPluginsAiService
{
    /// <summary>
    /// The application plugin name.
    /// </summary>
    private static readonly string ApplicationPluginName = ApplicationPluginsHelpers.PluginName;

    /// <summary>
    /// Gets the bug severity asynchronous.
    /// </summary>
    /// <param name="bugSeverityInput">The bug severity input.</param>
    /// <returns>
    /// The bug severity response.
    /// </returns>
    public async Task<string> GetBugSeverityAsync(BugSeverityInput bugSeverityInput)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetBugSeverityAsync), DateTime.UtcNow, string.Empty));
            if (string.IsNullOrEmpty(bugSeverityInput.BugTitle) || string.IsNullOrEmpty(bugSeverityInput.BugDescription))
                throw new Exception(ExceptionConstants.InputParametersCannotBeEmptyMessage);

            return await aiAgentServices.GetAiFunctionResponseAsync(bugSeverityInput, ApplicationPluginName, ApplicationPluginsHelpers.DetermineBugSeverityFunction.FunctionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetBugSeverityAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetBugSeverityAsync), DateTime.UtcNow, string.Empty));
        }
    }

    /// <summary>
    /// Generates the tag for story asynchronous.
    /// </summary>
    /// <param name="story">The story.</param>
    /// <returns>
    /// The genre tag response dto.
    /// </returns>
    public async Task<string> GenerateTagForStoryAsync(string story)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GenerateTagForStoryAsync), DateTime.UtcNow, string.Empty));
            if (string.IsNullOrEmpty(story))
            {
                var exception = new Exception(ExceptionConstants.StoryCannotBeEmptyMessage);
                logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GenerateTagForStoryAsync), DateTime.UtcNow, exception.Message));
                throw exception;
            }

            return await aiAgentServices.GetAiFunctionResponseAsync(story, ApplicationPluginName, ApplicationPluginsHelpers.GenerateGenreTagForStoryFunction.FunctionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GenerateTagForStoryAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GenerateTagForStoryAsync), DateTime.UtcNow, string.Empty));
        }
    }

    /// <summary>
    /// Moderates the content data asynchronous.
    /// </summary>
    /// <param name="story">The story.</param>
    /// <returns>
    /// The moderation content response dto.
    /// </returns>
    public async Task<string> ModerateContentDataAsync(string story)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(ModerateContentDataAsync), DateTime.UtcNow, string.Empty));
            if (string.IsNullOrEmpty(story))
            {
                var exception = new Exception(ExceptionConstants.StoryCannotBeEmptyMessage);
                logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(ModerateContentDataAsync), DateTime.UtcNow, exception.Message));
                throw exception;
            }

            return await aiAgentServices.GetAiFunctionResponseAsync(story, ApplicationPluginName, ApplicationPluginsHelpers.ContentModerationFunction.FunctionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(ModerateContentDataAsync), DateTime.UtcNow, ex.Message, string.Empty));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(ModerateContentDataAsync), DateTime.UtcNow));
        }
    }

    /// <summary>
    /// Rewrites text async.
    /// </summary>
    /// <param name="story">The story.</param>
    /// <returns>
    /// The rewrite response dto.
    /// </returns>
    public async Task<string> RewriteTextAsync(string story)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(RewriteTextAsync), DateTime.UtcNow, string.Empty));
            if (string.IsNullOrEmpty(story))
            {
                var exception = new Exception(ExceptionConstants.StoryCannotBeEmptyMessage);
                logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(RewriteTextAsync), DateTime.UtcNow, exception.Message));
                throw exception;
            }

            return await aiAgentServices.GetAiFunctionResponseAsync(story, ApplicationPluginName, ApplicationPluginsHelpers.RewriteUserStoryFunction.FunctionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(RewriteTextAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(RewriteTextAsync), DateTime.UtcNow, string.Empty));
        }
    }
}