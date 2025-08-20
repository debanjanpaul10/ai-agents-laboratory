// *********************************************************************************
//	<copyright file="ChatbotPlugins.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Chatbot Plugins.</summary>
// *********************************************************************************

using AIAgents.Laboratory.SemanticKernel.Adapters.Helpers;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using static AIAgents.Laboratory.Domain.Helpers.PluginHelpers.ChatBotPlugins;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Plugins.FitGymTool;

/// <summary>
/// The Chatbot Plugins.
/// </summary>
public class ChatbotPlugins
{
	/// <summary>
	/// Determines the user intent asynchronous.
	/// </summary>
	/// <param name="kernel">The kernel.</param>
	/// <param name="input">The input.</param>
	/// <returns>The AI response.</returns>
	[KernelFunction(DetermineUserIntentFunction.FunctionName)]
	[Description(DetermineUserIntentFunction.FunctionDescription)]
	public async Task<string> DetermineUserIntentAsync(Kernel kernel, [Description(DetermineUserIntentFunction.InputDescription)] string input)
	{
		var arguments = new KernelArguments()
		{{
				Constants.ArgumentsConstants.KernelArgumentsInputConstant, input
		}};

		var result = await kernel.InvokePromptAsync(DetermineUserIntentFunction.FunctionInstructions, arguments).ConfigureAwait(false);
		return result.GetValue<string>() ?? string.Empty;
	}

	/// <summary>
	/// Handles the user greeting asynchronous.
	/// </summary>
	/// <param name="kernel">The kernel.</param>
	/// <param name="input">The input.</param>
	/// <returns>The AI response.</returns>
	[KernelFunction(GreetingFunction.FunctionName)]
	[Description(GreetingFunction.FunctionDescription)]
	public static async Task<string> HandleUserGreetingAsync(Kernel kernel, [Description(GreetingFunction.InputDescription)] string input)
	{
		var arguments = new KernelArguments()
		{{
				Constants.ArgumentsConstants.KernelArgumentsInputConstant, input
		}};

		var result = await kernel.InvokePromptAsync(GreetingFunction.FunctionInstructions, arguments).ConfigureAwait(false);
		return result.GetValue<string>() ?? string.Empty;
	}
}
