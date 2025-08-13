// *********************************************************************************
//	<copyright file="RewriteTextPlugin.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Rewrite text plugin.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Shared.Constants;
using AIAgents.Laboratory.Shared.Models.IBBS;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;
using System.ComponentModel;
using static AIAgents.Laboratory.Agents.Plugins.IBBS.PluginHelpers.RewriteTextPlugin;

namespace AIAgents.Laboratory.Agents.Plugins.IBBS;

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
	[KernelFunction(name: RewriteUserStoryPlugin.FunctionName)]
	[Description(RewriteUserStoryPlugin.FunctionDescription)]
	public static async Task<string> ExecuteRewriteUserStoryAsync(Kernel kernel, [Description(RewriteUserStoryPlugin.InputDescription)] string input)
	{
		var arguments = new KernelArguments
		{{
			AiConstants.ArgumentsConstants.KernelArgumentsInputConstant, input
		}};

		var result = await kernel.InvokePromptAsync(RewriteUserStoryPlugin.FunctionInstructions, arguments).ConfigureAwait(false);
		var aiMetadata = result.Metadata;

		return JsonConvert.SerializeObject(new RewriteResponseDTO
		{
			RewrittenStory = result.GetValue<string>() ?? string.Empty,
			TotalTokensConsumed = Convert.ToInt32(aiMetadata?[AiConstants.ArgumentsConstants.TotalTokenCountConstant] ?? 0),
			CandidatesTokenCount = Convert.ToInt32(aiMetadata?[AiConstants.ArgumentsConstants.CandidatesTokenCountConstant] ?? 0),
			PromptTokenCount = Convert.ToInt32(aiMetadata?[AiConstants.ArgumentsConstants.PromptTokenCountConstant] ?? 0)
		});
	}
}


