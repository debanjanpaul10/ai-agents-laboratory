using System.Globalization;
using System.Text.Json;
using AIAgents.Laboratory.Domain.DrivenPorts;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.AIServices;

/// <summary>
/// The AI services Class.
/// </summary>
/// <param name="kernel">The Semantic Kernel.</param>
/// <param name="logger">The Logger service.</param>
/// <seealso cref="IAiServices" />
public class AiServices(ILogger<AiServices> logger, Kernel kernel) : IAiServices
{
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
