using AIAgents.Laboratory.API.Adapters.Models.Base;
using AIAgents.Laboratory.API.Adapters.Models.Request;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// The AI Chatbot skills handler interface.
/// </summary>
public interface IChatbotSkillsHandler
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
	Task<IEnumerable<string>> GetFollowupQuestionsResponseAsync(FollowupQuestionsRequestDTO followupQuestionsRequest);

	/// <summary>
	/// Detects the user intent asynchronous.
	/// </summary>
	/// <param name="userQueryRequest">The user query request.</param>
	/// <returns>The intent string.</returns>
	Task<string> DetectUserIntentAsync(UserQueryRequestDTO userQueryRequest);

	/// <summary>
	/// Handles the user greeting skill asynchronous.
	/// </summary>
	/// <returns>The greeting from ai agent.</returns>
	Task<string> GetUserGreetingResponseAsync();

	/// <summary>
	/// Handles the rag text response skill asynchronous.
	/// </summary>
	/// <param name="skillsInput">The skills input.</param>
	/// <returns>The RAG ai response.</returns>
	Task<string> GetRAGTextResponseAsync(SkillsInputDTO skillsInput);

	/// <summary>
	/// Gets the nl to SQL response asynchronous.
	/// </summary>
	/// <param name="nltosqlInput">The nltosql input.</param>
	/// <returns>The nl to sql ai response.</returns>
	Task<string> GetNLToSQLResponseAsync(NltosqlInputDTO nltosqlInput);
}
