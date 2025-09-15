using System.ComponentModel;
using System.Text.Json;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.SemanticKernel.Adapters.Helpers;
using Microsoft.SemanticKernel;
using static AIAgents.Laboratory.Domain.Helpers.PluginHelpers.RewriteTextPlugin;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Plugins;

/// <summary>
/// Rewrite text plugin.
/// </summary>
public class RewriteTextPlugin
{
	/// <summary>
	/// Executes rewrite user story async.
	/// </summary>
	/// <param name="kernel">The kernel.</param>
	/// <param name="input">The input.</param>
	[KernelFunction(RewriteUserStoryFunction.FunctionName)]
	[Description(RewriteUserStoryFunction.FunctionDescription)]
	public static async Task<string> ExecuteRewriteUserStoryAsync(Kernel kernel, [Description(RewriteUserStoryFunction.InputDescription)] string input)
	{
		var arguments = new KernelArguments
		{{
			Constants.ArgumentsConstants.KernelArgumentsInputConstant, input
		}};

		var result = await kernel.InvokePromptAsync(RewriteUserStoryFunction.FunctionInstructions, arguments).ConfigureAwait(false);
		var aiMetadata = result.Metadata;

		return JsonSerializer.Serialize(new RewriteResponse
		{
			RewrittenStory = result.GetValue<string>() ?? string.Empty,
			TotalTokensConsumed = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.TotalTokenCountConstant] ?? 0),
			CandidatesTokenCount = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.CandidatesTokenCountConstant] ?? 0),
			PromptTokenCount = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.PromptTokenCountConstant] ?? 0)
		});
	}
}


