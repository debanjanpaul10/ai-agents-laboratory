// *********************************************************************************
//	<copyright file="AgentServices.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Agent Services Class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.UseCases;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;
using System.Globalization;
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
	/// Invokes the plugin function with dynamic plugin loading.
	/// </summary>
	/// <typeparam name="TInput">The input type.</typeparam>
	/// <typeparam name="TResponse">The type of the response.</typeparam>
	/// <param name="input">The input.</param>
	/// <param name="pluginName">Name of the plugin.</param>
	/// <param name="functionName">Name of the function.</param>
	/// <returns>The AI Response.</returns>
	public async Task<TResponse> InvokePluginFunctionAsync<TInput, TResponse>(TInput input, string pluginName, string functionName)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(InvokePluginFunctionAsync), DateTime.UtcNow));
			var kernelArguments = new KernelArguments()
			{
				[ArgumentsConstants.KernelArgumentsInputConstant] = JsonConvert.SerializeObject(input)
			};

			var responseFromAI = await kernel.InvokeAsync(pluginName, functionName, kernelArguments);
			return JsonConvert.DeserializeObject<TResponse>(responseFromAI.GetValue<string>()!) ?? default!;
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
}
