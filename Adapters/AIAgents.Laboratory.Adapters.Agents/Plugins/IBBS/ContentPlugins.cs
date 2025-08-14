// *********************************************************************************
//	<copyright file="ContentPlugins.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Content Plugins.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DomainEntities.IBBS;
using AIAgents.Laboratory.SemanticKernel.Adapters.Helpers;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;
using System.ComponentModel;
using static AIAgents.Laboratory.Adapters.Agents.Plugins.IBBS.PluginHelpers.ContentPlugins;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Plugins.IBBS;

/// <summary>
/// The Content Plugins.
/// </summary>
public class ContentPlugins
{
	/// <summary>
	/// Executes the generate tag for story plugin asynchronous.
	/// </summary>
	/// <param name="kernel">The kernel.</param>
	/// <param name="input">The input.</param>
	/// <returns>The AI response.</returns>
	[KernelFunction(name: GenerateGenreTagForStoryPlugin.FunctionName)]
	[Description(description: GenerateGenreTagForStoryPlugin.FunctionDescription)]
	public static async Task<string> ExecuteGenerateTagForStoryPluginAsync(
		Kernel kernel, [Description(GenerateGenreTagForStoryPlugin.InputDescription)]string input)
	{
		var arguments = new KernelArguments
		{{
			Constants.ArgumentsConstants.KernelArgumentsInputConstant, input
		}};

		var result = await kernel.InvokePromptAsync(GenerateGenreTagForStoryPlugin.FunctionInstructions, arguments).ConfigureAwait(false);
		var aiMetadata = result.Metadata;

		return JsonConvert.SerializeObject(new TagResponse
		{
			UserStoryTag = result.GetValue<string>() ?? string.Empty,
			TotalTokensConsumed = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.TotalTokenCountConstant] ?? 0),
			CandidatesTokenCount = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.CandidatesTokenCountConstant] ?? 0),
			PromptTokenCount = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.PromptTokenCountConstant] ?? 0)
		});
	}

	/// <summary>
	/// Executes the content moderation plugin.
	/// </summary>
	/// <param name="kernel">The kernel.</param>
	/// <param name="input">The input.</param>
	/// <returns>The AI response.</returns>
	[KernelFunction(name: ContentModerationPlugin.FunctionName)]
	[Description(description: ContentModerationPlugin.FunctionDescription)]
	public static async Task<string> ExecuteContentModerationPlugin(
		Kernel kernel, [Description(ContentModerationPlugin.InputDescription)] string input)
	{
		var arguments = new KernelArguments
		{{
			Constants.ArgumentsConstants.KernelArgumentsInputConstant, input
		}};

		var result = await kernel.InvokePromptAsync(ContentModerationPlugin.FunctionInstructions, arguments).ConfigureAwait(false);
		var aiMetadata = result.Metadata;

		return JsonConvert.SerializeObject(new ModerationContentResponse
		{
			ContentRating = result.GetValue<string>() ?? string.Empty,
			TotalTokensConsumed = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.TotalTokenCountConstant] ?? 0),
			CandidatesTokenCount = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.CandidatesTokenCountConstant] ?? 0),
			PromptTokenCount = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.PromptTokenCountConstant] ?? 0)
		});
	}
}
