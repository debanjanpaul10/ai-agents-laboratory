// *********************************************************************************
//	<copyright file="BulletinAIServices.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Bulletin Board AI services interface.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Core.Contracts;
using AIAgents.Laboratory.Shared.Constants;
using AIAgents.Laboratory.Shared.Models.IBBS;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;
using System.Globalization;
using static AIAgents.Laboratory.Agents.Plugins.IBBS.PluginHelpers;

namespace AIAgents.Laboratory.Core.Services;

/// <summary>
/// The Bulletin AI Services Class.
/// </summary>
/// <param name="kernel">The semantic kernel.</param>
/// <param name="logger">The logger service.</param>
/// <param name="commonAiService">The common ai service.</param>
/// <seealso cref="IBulletinAIServices" />
public class BulletinAIServices(ILogger<BulletinAIServices> logger, Kernel kernel, ICommonAiService commonAiService) : IBulletinAIServices
{
    /// <summary>
    /// Generates the tag for story asynchronous.
    /// </summary>
    /// <param name="story">The story.</param>
    /// <returns>
    /// The genre tag response dto.
    /// </returns>
    public async Task<TagResponseDTO> GenerateTagForStoryAsync(string story)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GenerateTagForStoryAsync), DateTime.UtcNow));
            if (string.IsNullOrEmpty(story))
            {
                var exception = new Exception(ExceptionConstants.StoryCannotBeEmptyMessage);
                logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GenerateTagForStoryAsync), DateTime.UtcNow, exception.Message));
                throw exception;
            }

            var kernelArguments = new KernelArguments()
            {
                [AiConstants.ArgumentsConstants.KernelArgumentsInputConstant] = story
            };

            var responseFromAI = await kernel.InvokeAsync(ContentPlugins.PluginName, ContentPlugins.GenerateGenreTagForStoryPlugin.FunctionName, kernelArguments);
            var response = JsonConvert.DeserializeObject<TagResponseDTO>(responseFromAI.GetValue<string>()!);
            if (response is not null)
            {
                response.ModelUsed = commonAiService.GetCurrentModelId();
            }

            return response ?? new TagResponseDTO { ModelUsed = commonAiService.GetCurrentModelId() };
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GenerateTagForStoryAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GenerateTagForStoryAsync), DateTime.UtcNow));
        }
    }

    /// <summary>
    /// Moderates the content data asynchronous.
    /// </summary>
    /// <param name="story">The story.</param>
    /// <returns>
    /// The moderation content response dto.
    /// </returns>
    public async Task<ModerationContentResponseDTO> ModerateContentDataAsync(string story)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(ModerateContentDataAsync), DateTime.UtcNow));
            if (string.IsNullOrEmpty(story))
            {
                var exception = new Exception(ExceptionConstants.StoryCannotBeEmptyMessage);
                logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(ModerateContentDataAsync), DateTime.UtcNow, exception.Message));
                throw exception;
            }

            var kernelArguments = new KernelArguments()
            {
                [AiConstants.ArgumentsConstants.KernelArgumentsInputConstant] = story
            };

            var responseFromAI = await kernel.InvokeAsync(ContentPlugins.PluginName, ContentPlugins.ContentModerationPlugin.FunctionName, kernelArguments);
            var response = JsonConvert.DeserializeObject<ModerationContentResponseDTO>(responseFromAI.GetValue<string>()!);
            if (response is not null)
            {
                response.ModelUsed = commonAiService.GetCurrentModelId();
            }

            return response ?? new ModerationContentResponseDTO { ModelUsed = commonAiService.GetCurrentModelId() };
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(ModerateContentDataAsync), DateTime.UtcNow, ex.Message));
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
    public async Task<RewriteResponseDTO> RewriteTextAsync(string story)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(RewriteTextAsync), DateTime.UtcNow));
            if (string.IsNullOrEmpty(story))
            {
                var exception = new Exception(ExceptionConstants.StoryCannotBeEmptyMessage);
                logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(RewriteTextAsync), DateTime.UtcNow, exception.Message));
                throw exception;
            }

            var kernelArguments = new KernelArguments()
            {
                [AiConstants.ArgumentsConstants.KernelArgumentsInputConstant] = story
            };

            var responseFromAI = await kernel.InvokeAsync(RewriteTextPlugin.PluginName, RewriteTextPlugin.RewriteUserStoryPlugin.FunctionName, kernelArguments);
            var response = JsonConvert.DeserializeObject<RewriteResponseDTO>(responseFromAI.GetValue<string>()!);
            if (response is not null)
            {
                response.ModelUsed = commonAiService.GetCurrentModelId();
            }

            return response ?? new RewriteResponseDTO { ModelUsed = commonAiService.GetCurrentModelId() };
        }
        catch (Exception ex)
        {
            logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(RewriteTextAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(RewriteTextAsync), DateTime.UtcNow));
        }
    }
}
