using AIAgents.Laboratory.SemanticKernel.Adapters.Helpers;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using static AIAgents.Laboratory.Domain.Helpers.PluginHelpers.UtilityPlugins;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Plugins;

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
		return result.GetValue<string>() ?? string.Empty;
	}
}
