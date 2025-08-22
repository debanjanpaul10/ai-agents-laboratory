// *********************************************************************************
//	<copyright file="IFitGymToolAIHandler.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The interface for FitGym Tool handler.</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Request.FitGymTool;
using AIAgents.Laboratory.API.Adapters.Models.Response.FitGymTool;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// The interface for FitGym Tool handler.
/// </summary>
public interface IFitGymToolAIHandler
{
	/// <summary>
	/// Gets the bug severity asynchronous.
	/// </summary>
	/// <param name="bugSeverityInput">The bug severity input.</param>
	/// <returns>The bug severity response.</returns>
	Task<BugSeverityResponseDTO> GetBugSeverityAsync(BugSeverityInputDTO bugSeverityInput);

	/// <summary>
	/// Gets the orchestrator response asynchronous.
	/// </summary>
	/// <param name="userQueryRequest">The user query request.</param>
	/// <returns>The AI response.</returns>
	Task<AIAgentResponseDTO> GetOrchestratorResponseAsync(UserQueryRequestDTO userQueryRequest);
}
