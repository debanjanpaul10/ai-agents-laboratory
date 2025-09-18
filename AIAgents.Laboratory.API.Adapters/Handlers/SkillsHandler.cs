﻿using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.SkillsEntities;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The Skills handler adapter.
/// </summary>
/// <param name="aiSkillsService">The ai skills service.</param>
/// <param name="mapper">The mapper service.</param>
/// <seealso cref="AIAgents.Laboratory.API.Adapters.Contracts.ISkillsHandler" />
public class SkillsHandler(IMapper mapper, ISkillsService aiSkillsService) : ISkillsHandler
{
	/// <summary>
	/// Detects the user intent asynchronous.
	/// </summary>
	/// <param name="userQueryRequest">The user query request.</param>
	/// <returns>
	/// The intent string.
	/// </returns>
	public async Task<string> DetectUserIntentAsync(UserQueryRequestDTO userQueryRequest)
	{
		var domainRequest = mapper.Map<UserRequestDomain>(userQueryRequest);
		return await aiSkillsService.DetectUserIntentAsync(domainRequest).ConfigureAwait(false);
	}

	/// <summary>
	/// Gets the list of followup questions.
	/// </summary>
	/// <param name="followupQuestionsRequest">The followup questions request.</param>
	/// <returns>The list of followup questions.</returns>
	public async Task<IEnumerable<string>> GetFollowupQuestionsResponseAsync(FollowupQuestionsRequestDTO followupQuestionsRequest)
	{
		var domainInput = mapper.Map<FollowupQuestionsRequestDomain>(followupQuestionsRequest);
		return await aiSkillsService.GetFollowupQuestionsResponseAsync(domainInput).ConfigureAwait(false);
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
		return await aiSkillsService.GetSQLQueryMarkdownResponseAsync(input).ConfigureAwait(false);
	}

	/// <summary>
	/// Gets the nl to SQL response asynchronous.
	/// </summary>
	/// <param name="nltosqlInput">The nltosql input.</param>
	/// <returns>
	/// The nl to sql ai response.
	/// </returns>
	public async Task<string> GetNLToSQLResponseAsync(NltosqlInputDTO nltosqlInput)
	{
		var domainInput = mapper.Map<NltosqlInputDomain>(nltosqlInput);
		return await aiSkillsService.HandleNLToSQLResponseAsync(domainInput).ConfigureAwait(false);
	}

	/// <summary>
	/// Handles the rag text response skill asynchronous.
	/// </summary>
	/// <param name="skillsInput">The skills input.</param>
	/// <returns>
	/// The RAG ai response.
	/// </returns>
	public async Task<string> GetRAGTextResponseAsync(SkillsInputDTO skillsInput)
	{
		var domainInput = mapper.Map<SkillsInputDomain>(skillsInput);
		return await aiSkillsService.HandleRAGTextResponseAsync(domainInput).ConfigureAwait(false);
	}

	/// <summary>
	/// Handles the user greeting intent asynchronous.
	/// </summary>
	/// <returns>
	/// The greeting from ai agent.
	/// </returns>
	public async Task<string> GetUserGreetingResponseAsync()
	{
		return await aiSkillsService.HandleUserGreetingIntentAsync().ConfigureAwait(false);
	}
}
