using System.ComponentModel;
using System.Text.Json;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.SemanticKernel.Adapters.Helpers;
using Microsoft.SemanticKernel;
using static AIAgents.Laboratory.Domain.Helpers.PluginHelpers.ContentPlugins;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Plugins;

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
	[KernelFunction(name: GenerateGenreTagForStoryFunction.FunctionName)]
	[Description(description: GenerateGenreTagForStoryFunction.FunctionDescription)]
	public static async Task<string> ExecuteGenerateTagForStoryPluginAsync(Kernel kernel, [Description(GenerateGenreTagForStoryFunction.InputDescription)] string input)
	{
		var arguments = new KernelArguments
		{{
			Constants.ArgumentsConstants.KernelArgumentsInputConstant, input
		}};

		var result = await kernel.InvokePromptAsync(GenerateGenreTagForStoryFunction.FunctionInstructions, arguments).ConfigureAwait(false);
		var aiMetadata = result.Metadata;

		return JsonSerializer.Serialize(new TagResponse
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
	[KernelFunction(name: ContentModerationFunction.FunctionName)]
	[Description(description: ContentModerationFunction.FunctionDescription)]
	public static async Task<string> ExecuteContentModerationPlugin(Kernel kernel, [Description(ContentModerationFunction.InputDescription)] string input)
	{
		var arguments = new KernelArguments
		{{
			Constants.ArgumentsConstants.KernelArgumentsInputConstant, input
		}};

		var result = await kernel.InvokePromptAsync(ContentModerationFunction.FunctionInstructions, arguments).ConfigureAwait(false);
		var aiMetadata = result.Metadata;

		return JsonSerializer.Serialize(new ModerationContentResponse
		{
			ContentRating = result.GetValue<string>() ?? string.Empty,
			TotalTokensConsumed = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.TotalTokenCountConstant] ?? 0),
			CandidatesTokenCount = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.CandidatesTokenCountConstant] ?? 0),
			PromptTokenCount = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.PromptTokenCountConstant] ?? 0)
		});
	}
}
