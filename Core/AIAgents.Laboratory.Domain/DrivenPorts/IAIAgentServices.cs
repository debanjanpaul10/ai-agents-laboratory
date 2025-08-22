// *********************************************************************************
//	<copyright file="IAIAgentServices.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Agent Services Interface.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DomainEntities.FitGymTool;

namespace AIAgents.Laboratory.Domain.DrivenPorts;

/// <summary>
/// The Agent Services Interface.
/// </summary>
public interface IAIAgentServices
{
	/// <summary>
	/// Gets the ai function response asynchronous.
	/// </summary>
	/// <typeparam name="TInput">The type of the input.</typeparam>
	/// <typeparam name="TResponse">The type of the response.</typeparam>
	/// <param name="input">The input.</param>
	/// <param name="pluginName">Name of the plugin.</param>
	/// <param name="functionName">Name of the function.</param>
	/// <returns>The AI response.</returns>
	Task<TResponse> GetAiFunctionResponseAsync<TInput, TResponse>(TInput input, string pluginName, string functionName);

	/// <summary>
	/// Gets the orchestrator function response asynchronous.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <returns>The AI response.</returns>
	Task<AIAgentResponseDomain> GetOrchestratorFunctionResponseAsync(string input);
}
