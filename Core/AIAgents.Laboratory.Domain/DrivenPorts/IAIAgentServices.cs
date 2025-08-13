namespace AIAgents.Laboratory.Domain.DrivenPorts;

/// <summary>
/// The Agent Services Interface.
/// </summary>
public interface IAIAgentServices
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
	Task<TR> InvokeBulletinAIAgentsAsync<T, TR>(T input, string pluginName, string functionName);
}
