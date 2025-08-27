using System.Globalization;
using System.Text.Json;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.Extensions.Logging;
using static AIAgents.Laboratory.Domain.Helpers.Constants;
using static AIAgents.Laboratory.Domain.Helpers.PluginHelpers;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The plugins ai services.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="aiAgentServices">The AI agent services.</param>
/// <param name="commonAiService">The common ai services.</param>
public class PluginsAiServices(ILogger<PluginsAiServices> logger, IAIAgentServices aiAgentServices, ICommonAiService commonAiService) : IPluginsAiServices
{
    /// <summary>
	/// Gets the bug severity asynchronous.
	/// </summary>
	/// <param name="bugSeverityInput">The bug severity input.</param>
	/// <returns>
	/// The bug severity response.
	/// </returns>
	public async Task<BugSeverityResponse> GetBugSeverityAsync(BugSeverityInput bugSeverityInput)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetBugSeverityAsync), DateTime.UtcNow, string.Empty));
            if (bugSeverityInput is null)
            {
                var exception = new Exception(ExceptionConstants.InputParametersCannotBeEmptyMessage);
                logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetBugSeverityAsync), DateTime.UtcNow, exception.Message));
                throw exception;
            }

            var response = await aiAgentServices.GetAiFunctionResponseAsync(bugSeverityInput, PluginHelpers.UtilityPlugins.PluginName, PluginHelpers.UtilityPlugins.DetermineBugSeverityFunction.FunctionName).ConfigureAwait(false);
            var bugSeverityResponse = JsonSerializer.Deserialize<BugSeverityResponse>(response);
            if (bugSeverityResponse is not null)
            {
                bugSeverityResponse.ModelUsed = commonAiService.GetCurrentModelId();
            }

            return bugSeverityResponse ?? new BugSeverityResponse { ModelUsed = commonAiService.GetCurrentModelId() };
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
	public async Task<TagResponse> GenerateTagForStoryAsync(string story)
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

            var response = await aiAgentServices.GetAiFunctionResponseAsync(story, ContentPlugins.PluginName, ContentPlugins.GenerateGenreTagForStoryFunction.FunctionName).ConfigureAwait(false);
            var tagResponse = JsonSerializer.Deserialize<TagResponse>(response);
            if (tagResponse is not null)
            {
                tagResponse.ModelUsed = commonAiService.GetCurrentModelId();
            }

            return tagResponse ?? new TagResponse { ModelUsed = commonAiService.GetCurrentModelId() };
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
    public async Task<ModerationContentResponse> ModerateContentDataAsync(string story)
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

            var response = await aiAgentServices.GetAiFunctionResponseAsync(story, ContentPlugins.PluginName, ContentPlugins.ContentModerationFunction.FunctionName).ConfigureAwait(false);
            var moderationResponse = JsonSerializer.Deserialize<ModerationContentResponse>(response);
            if (moderationResponse is not null)
            {
                moderationResponse.ModelUsed = commonAiService.GetCurrentModelId();
            }

            return moderationResponse ?? new ModerationContentResponse { ModelUsed = commonAiService.GetCurrentModelId() };
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
    public async Task<RewriteResponse> RewriteTextAsync(string story)
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

            var response = await aiAgentServices.GetAiFunctionResponseAsync(story, RewriteTextPlugin.PluginName, RewriteTextPlugin.RewriteUserStoryFunction.FunctionName).ConfigureAwait(false);
            var rewriteResponse = JsonSerializer.Deserialize<RewriteResponse>(response);
            if (rewriteResponse is not null)
            {
                rewriteResponse.ModelUsed = commonAiService.GetCurrentModelId();
            }

            return rewriteResponse ?? new RewriteResponse { ModelUsed = commonAiService.GetCurrentModelId() };
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