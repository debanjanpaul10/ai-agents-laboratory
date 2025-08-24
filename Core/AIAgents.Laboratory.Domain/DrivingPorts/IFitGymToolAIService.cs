// *********************************************************************************
//	<copyright file="IFitGymToolAIService.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The FitGym Tool AI Service Interface.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.FitGymTool;

namespace AIAgents.Laboratory.Domain.DrivingPorts;

/// <summary>
/// The FitGym Tool AI Service Interface.
/// </summary>
public interface IFitGymToolAIService
{
	/// <summary>
	/// Gets the bug severity asynchronous.
	/// </summary>
	/// <param name="bugSeverityInput">The bug severity input.</param>
	/// <returns>The bug severity response.</returns>
	Task<BugSeverityResponse> GetBugSeverityAsync(BugSeverityInput bugSeverityInput);

	/// <summary>
	/// Gets the orchestrator response asynchronous.
	/// </summary>
	/// <param name="userQueryRequest">The user query request.</param>
	/// <returns>The AI response.</returns>
	Task<AIAgentResponseDomain> GetOrchestratorResponseAsync(UserRequestDomain userQueryRequest);

	/// <summary>
	/// Gets the SQL query markdown response asynchronous.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <returns>The formatted AI response.</returns>
	Task<string> GetSQLQueryMarkdownResponseAsync(string input);

	/// <summary>
	/// Gets the list of followup questions.
	/// </summary>
	/// <param name="followupQuestionsRequest">The followup questions request.</param>
	/// <returns>The list of followup questions.</returns>
	Task<IEnumerable<string>> GetFollowupQuestionsResponseAsync(FollowupQuestionsRequestDomain followupQuestionsRequest);
}
