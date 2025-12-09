using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DomainEntities.SkillsEntities;

namespace AIAgents.Laboratory.Domain.DrivingPorts;

/// <summary>
/// The AI Skills service interface.
/// </summary>
public interface ISkillsService
{
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

	/// <summary>
	/// Detects the user intent asynchronous.
	/// </summary>
	/// <param name="userQueryRequest">The user query request.</param>
	/// <returns>The intent string.</returns>
	Task<string> DetectUserIntentAsync(UserRequestDomain userQueryRequest);

	/// <summary>
	/// Handles the user greeting intent asynchronous.
	/// </summary>
	/// <returns>The greeting from ai agent.</returns>
	Task<string> HandleUserGreetingIntentAsync();

	/// <summary>
	/// Handles the rag text response asynchronous.
	/// </summary>
	/// <param name="skillsInputDomain">The skills input domain.</param>
	/// <returns>The ai generated response.</returns>
	Task<string> HandleRAGTextResponseAsync(SkillsInputDomain skillsInputDomain);

	/// <summary>
	/// Handles the nl to SQL response asynchronous.
	/// </summary>
	/// <param name="nltosqlInput">The nltosql input.</param>
	/// <returns>The ai generated response.</returns>
	Task<string> HandleNLToSQLResponseAsync(NltosqlInputDomain nltosqlInput);
}
