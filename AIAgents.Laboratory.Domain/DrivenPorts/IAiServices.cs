namespace AIAgents.Laboratory.Domain.DrivenPorts;

/// <summary>
/// The AI Services interface.
/// </summary>
public interface IAiServices
{
	/// <summary>
	/// Gets the ai function response asynchronous.
	/// </summary>
	/// <typeparam name="TInput">The type of the input.</typeparam>
	/// <param name="input">The input.</param>
	/// <param name="pluginName">Name of the plugin.</param>
	/// <param name="functionName">Name of the function.</param>
	/// <returns>The AI response.</returns>
	Task<string> GetAiFunctionResponseAsync<TInput>(TInput input, string pluginName, string functionName);
}