// *********************************************************************************
//	<copyright file="FitGymToolAIHandler.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The FitGym Tool handler.</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Request.FitGymTool;
using AIAgents.Laboratory.API.Adapters.Models.Response.FitGymTool;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.FitGymTool;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The FitGym Tool AI Handler.
/// </summary>
/// <param name="fitGymToolAIService">The FitGym Tool AI service.</param>
/// <param name="mapper">The mapper service.</param>
/// <seealso cref="AIAgents.Laboratory.API.Adapters.Contracts.IFitGymToolAIHandler" />
public class FitGymToolAIHandler(IFitGymToolAIService fitGymToolAIService, IMapper mapper) : IFitGymToolAIHandler
{
	/// <summary>
	/// Gets the bug severity asynchronous.
	/// </summary>
	/// <param name="bugSeverityInput">The bug severity input.</param>
	/// <returns>
	/// The bug severity response.
	/// </returns>
	public async Task<BugSeverityResponseDTO> GetBugSeverityAsync(BugSeverityInputDTO bugSeverityInput)
	{
		var domainInput = mapper.Map<BugSeverityInput>(bugSeverityInput);
		var domainResult = await fitGymToolAIService.GetBugSeverityAsync(domainInput).ConfigureAwait(false);
		return mapper.Map<BugSeverityResponseDTO>(domainResult);
	}

	/// <summary>
	/// Gets the orchestrator response asynchronous.
	/// </summary>
	/// <param name="userQueryRequest">The user query request.</param>
	/// <returns>
	/// The AI response.
	/// </returns>
	public async Task<AIAgentResponseDTO> GetOrchestratorResponseAsync(UserQueryRequestDTO userQueryRequest)
	{
		var domainInput = mapper.Map<UserRequestDomain>(userQueryRequest);
		var domainResult = await fitGymToolAIService.GetOrchestratorResponseAsync(domainInput).ConfigureAwait(false);
		return mapper.Map<AIAgentResponseDTO>(domainResult);
	}

	/// <summary>
	/// Gets the SQL query markdown response asynchronous.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <returns>
	/// The formatted AI response.
	/// </returns>
	public async Task<string> GetSQLQueryMarkdownResponseAsync(string input)
	{
		return await fitGymToolAIService.GetSQLQueryMarkdownResponseAsync(input).ConfigureAwait(false);
	}
}
