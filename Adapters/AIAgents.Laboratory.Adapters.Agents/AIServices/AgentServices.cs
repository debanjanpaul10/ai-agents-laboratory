using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.UseCases;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;
using System.Globalization;
using static AIAgents.Laboratory.Agents.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.Agents.Adapters.AIServices;

/// <summary>
/// The Agent Services Class.
/// </summary>
/// <param name="kernel">The Semantic Kernel.</param>
/// <param name="logger">The Logger service.</param>
/// <seealso cref="IAIAgentServices" />
public class AgentServices(ILogger<BulletinAIServices> logger, Kernel kernel) : IAIAgentServices
{
	/// <summary>
	/// Invokes the bulletin ai agents asynchronous.
	/// </summary>
	/// <typeparam name="T">The input type.</typeparam>
	/// <typeparam name="TR">The type of the response.</typeparam>
	/// <param name="input">The input.</param>
	/// <param name="pluginName">Name of the plugin.</param>
	/// <param name="functionName">Name of the function.</param>
	/// <returns>The response from AI agent.</returns>
	public async Task<TR> InvokeBulletinAIAgentsAsync<T, TR>(T input, string pluginName, string functionName)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodStart, nameof(InvokeBulletinAIAgentsAsync), DateTime.UtcNow));
			
			var kernelArguments = new KernelArguments()
			{
				[ArgumentsConstants.KernelArgumentsInputConstant] = input
			};

			var responseFromAI = await kernel.InvokeAsync(pluginName, functionName, kernelArguments);
			return JsonConvert.DeserializeObject<TR>(responseFromAI.GetValue<string>()!) ?? default!;
		}
		catch (Exception ex)
		{
			logger.LogError(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodFailed, nameof(InvokeBulletinAIAgentsAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.LogHelperMethodEnd, nameof(InvokeBulletinAIAgentsAsync), DateTime.UtcNow));
		}
	}
}
