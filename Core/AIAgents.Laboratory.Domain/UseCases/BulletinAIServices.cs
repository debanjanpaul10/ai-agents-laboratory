// *********************************************************************************
//	<copyright file="BulletinAIServices.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Bulletin Board AI services class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DomainEntities.IBBS;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using Microsoft.Extensions.Logging;
using System.Globalization;
using static AIAgents.Laboratory.Domain.Helpers.PluginHelpers;
using static AIAgents.Laboratory.Domain.Helpers.Constants;
using System.Text.Json;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The Bulletin AI Services Class.
/// </summary>
/// <param name="aiAgentServices">The AI Agent Services.</param>
/// <param name="commonAiService">The Common AI Services.</param>
/// <param name="logger">The Logger service.</param>
/// <seealso cref="AIAgents.Laboratory.Domain.DrivingPorts.IBulletinAIServices" />
public class BulletinAIServices(ILogger<BulletinAIServices> logger, IAIAgentServices aiAgentServices, ICommonAiService commonAiService) : IBulletinAIServices
{
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
