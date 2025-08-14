// *********************************************************************************
//	<copyright file="UtilityPlugins.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Utility Plugins.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DomainEntities.FitGymTool;
using AIAgents.Laboratory.SemanticKernel.Adapters.Helpers;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;
using System.ComponentModel;
using static AIAgents.Laboratory.Domain.Helpers.PluginHelpers.UtilityPlugins;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Plugins.FitGymTool;

/// <summary>
/// The Utility Plugins.
/// </summary>
public class UtilityPlugins
{
	/// <summary>
	/// Determines the bug severity asynchronous.
	/// </summary>
	/// <param name="kernel">The kernel.</param>
	/// <param name="input">The input.</param>
	/// <returns>The ai prompt response.</returns>
	[KernelFunction(DetermineBugSeverityFunction.FunctionName)]
	[Description(DetermineBugSeverityFunction.FunctionDescription)]
	public static async Task<string> DetermineBugSeverityAsync(Kernel kernel, [Description(DetermineBugSeverityFunction.InputDescription)] string input)
	{
		var arguments = new KernelArguments()
		{{
				Constants.ArgumentsConstants.KernelArgumentsInputConstant, input
		}};

		var result = await kernel.InvokePromptAsync(DetermineBugSeverityFunction.FunctionInstructions, arguments).ConfigureAwait(false);
		var aiMetadata = result.Metadata;

		return JsonConvert.SerializeObject(new BugSeverityResponse
		{
			BugSeverity = result.GetValue<string>() ?? string.Empty,
			TotalTokensConsumed = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.TotalTokenCountConstant] ?? 0),
			CandidatesTokenCount = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.CandidatesTokenCountConstant] ?? 0),
			PromptTokenCount = Convert.ToInt32(aiMetadata?[Constants.ArgumentsConstants.PromptTokenCountConstant] ?? 0)
		});
	}
}
