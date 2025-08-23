// *********************************************************************************
//	<copyright file="AgentServices.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Agent Services Class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DomainEntities.FitGymTool;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.UseCases;
using AIAgents.Laboratory.SemanticKernel.Adapters.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using System.Globalization;
using System.Text.Json;
using static AIAgents.Laboratory.Domain.Helpers.PluginHelpers;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.AIServices;

/// <summary>
/// The Agent Services Class.
/// </summary>
/// <param name="kernel">The Semantic Kernel.</param>
/// <param name="logger">The Logger service.</param>
/// <seealso cref="IAIAgentServices" />
public class AgentServices(ILogger<BulletinAIServices> logger, Kernel kernel) : IAIAgentServices
{
	/// <summary>
	/// Gets the orchestrator function response asynchronous.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <returns>
	/// The AI response.
	/// </returns>
	public async Task<AIAgentResponseDomain> GetOrchestratorFunctionResponseAsync(string input)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(GetOrchestratorFunctionResponseAsync), DateTime.UtcNow));

			var aiAgentResponse = new AIAgentResponseDomain();
			var userIntent = await InvokePluginFunctionAsync(input, ChatBotPlugins.PluginName, ChatBotPlugins.DetermineUserIntentFunction.FunctionName).ConfigureAwait(false);
			if (string.IsNullOrEmpty(userIntent))
			{
				throw new Exception(ExceptionConstants.SomethingWentWrongMessage);
			}
			
			var normalizedIntent = userIntent.Trim().ToUpperInvariant();
			var aiResponse = normalizedIntent switch
			{
				IntentConstants.GreetingIntent => await InvokePluginFunctionAsync(input, ChatBotPlugins.PluginName, ChatBotPlugins.GreetingFunction.FunctionName).ConfigureAwait(false),
				IntentConstants.SQLIntent => await InvokePluginFunctionAsync(input, ChatBotPlugins.PluginName, ChatBotPlugins.NLToSqlSkillFunction.FunctionName).ConfigureAwait(false),
				IntentConstants.RAGIntent => await InvokePluginFunctionAsync(input, ChatBotPlugins.PluginName, ChatBotPlugins.RAGTextSkillFunction.FunctionName).ConfigureAwait(false),
				IntentConstants.UnclearIntent => "Cannot determine the user intent",
				_ => string.Empty
			};

			return aiAgentResponse.PrepareAgentChatbotReponse(userIntent, input, aiResponse);
		}
		catch (Exception ex)
		{
			logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(GetOrchestratorFunctionResponseAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(GetOrchestratorFunctionResponseAsync), DateTime.UtcNow));
		}
	}

	/// <summary>
	/// Gets the ai function response asynchronous.
	/// </summary>
	/// <typeparam name="TInput">The type of the input.</typeparam>
	/// <param name="input">The input.</param>
	/// <param name="pluginName">Name of the plugin.</param>
	/// <param name="functionName">Name of the function.</param>
	/// <returns>
	/// The AI response.
	/// </returns>
	public async Task<string> GetAiFunctionResponseAsync<TInput>(TInput input, string pluginName, string functionName)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(InvokePluginFunctionAsync), DateTime.UtcNow));
			return await InvokePluginFunctionAsync(input, pluginName, functionName).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(InvokePluginFunctionAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(InvokePluginFunctionAsync), DateTime.UtcNow));
		}
	}

	#region PRIVATE METHODS

	/// <summary>
	/// Invokes the plugin function asynchronous.
	/// </summary>
	/// <typeparam name="TInput">The type of the input.</typeparam>
	/// <param name="input">The input.</param>
	/// <param name="pluginName">Name of the plugin.</param>
	/// <param name="functionName">Name of the function.</param>
	/// <returns>The AI string response.</returns>
	private async Task<string> InvokePluginFunctionAsync<TInput>(TInput input, string pluginName, string functionName)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(InvokePluginFunctionAsync), DateTime.UtcNow));
			var kernelArguments = new KernelArguments()
			{
				[ArgumentsConstants.KernelArgumentsInputConstant] = JsonSerializer.Serialize(input)
			};

			var responseFromAI = await kernel.InvokeAsync(pluginName, functionName, kernelArguments).ConfigureAwait(false);
			return responseFromAI.GetValue<string>()!;
		}
		catch (Exception ex)
		{
			logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(InvokePluginFunctionAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(InvokePluginFunctionAsync), DateTime.UtcNow));
		}
	}

	#endregion
}
