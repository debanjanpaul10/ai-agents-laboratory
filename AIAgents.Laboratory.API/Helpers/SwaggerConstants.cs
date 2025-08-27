namespace AIAgents.Laboratory.API.Helpers;

/// <summary>
/// The Swagger Constants class containing swagger documentation.
/// </summary>
internal static class SwaggerConstants
{
	/// <summary>
	/// The Skills Controller.
	/// </summary>
	internal static class SkillsController
	{
		/// <summary>
		/// Swagger documentation for GetSQLQueryMarkdownResponseAsync
		/// </summary>
		internal static class GetSQLQueryMarkdownResponseAction
		{
			internal const string Summary = "Gets the markdown response for sql query json.";
			internal const string Description = "Gets a properly formatted markdown table and response format from a json sql request.";
			internal const string OperationId = nameof(GetSQLQueryMarkdownResponseAction);
		}

		/// <summary>
		/// Swagger documentation for GetFollowupQuestionsResponseAsync.
		/// </summary>
		internal static class GetFollowupQuestionsResponseAction
		{
			internal const string Summary = "Gets the list of followup questions.";
			internal const string Description = "Gets a list of strings that contain followup questions based on last user query and ai responses.";
			internal const string OperationId = nameof(GetFollowupQuestionsResponseAction);
		}

		/// <summary>
		/// Swagger documentation for DetectUserIntentAsync.
		/// </summary>
		internal static class DetectUserIntentAction
		{
			internal const string Summary = "Detects the user intent based on the user's query.";
			internal const string Description = "Detects the user intent and classifies them into SQL, RAG, GREETING, etc based on the user's asked question and query.";
			internal const string OperationId = nameof(DetectUserIntentAction);
		}

		/// <summary>
		/// Swagger documentation for GetUserGreetingResponseAsync.
		/// </summary>
		internal static class GetUserGreetingResponseAction
		{
			internal const string Summary = "Gives a greeting message for the user's reply.";
			internal const string Description = "Based on the user's greeting message, returns an AI generated greeting response.";
			internal const string OperationId = nameof(GetUserGreetingResponseAction);
		}

		/// <summary>
		/// Swagger documentation for GetRAGTextResponseAsync.
		/// </summary>
		internal static class GetRAGTextResponseAction
		{
			internal const string Summary = "Gives the RAG text response based on user query.";
			internal const string Description = "Gives the RAG text response based on user query and knowledge base passed on.";
			internal const string OperationId = nameof(GetRAGTextResponseAction);
		}

		/// <summary>
		/// Swagger documentation for GetNLToSQLResponseAsync.
		/// </summary>
		internal static class GetNLToSQLResponseAction
		{
			internal const string Summary = "Gives the Nl to sql response based on user query.";
			internal const string Description = "Creates a SQL query based on user's query, knowledge base and database schema.";
			internal const string OperationId = nameof(GetNLToSQLResponseAction);
		}
	}

	/// <summary>
	/// The Plugins Controller.
	/// </summary>
	internal static class PluginsController
	{
		/// <summary>
		/// Swagger documentation for GetBugSeverityAsync
		/// </summary>
		internal static class GetBugSeverityAction
		{
			internal const string Summary = "Gets the bug severity string data asynchronous.";
			internal const string Description = "Gets the bug severity for the given user bug based on the bug title and bug description.";
			internal const string OperationId = nameof(GetBugSeverityAction);
		}

		/// <summary>
		/// Swagger documentation for RewriteTextAsync
		/// </summary>
		internal static class RewriteTextAction
		{
			internal const string Summary = "Rewrites the text asynchronous.";
			internal const string Description = "Rewrites and improves the provided user story text using AI to enhance readability, grammar, and overall quality while maintaining the original meaning and context.";
			internal const string OperationId = nameof(RewriteTextAction);
		}

		/// <summary>
		/// Swagger documentation for GenerateTagForStoryAsync
		/// </summary>
		internal static class GenerateTagForStoryAction
		{
			internal const string Summary = "Generates the tag for story asynchronous.";
			internal const string Description = "Analyzes the provided user story content and generates an appropriate genre tag or category classification using AI to help with content organization and discovery.";
			internal const string OperationId = nameof(GenerateTagForStoryAction);
		}

		/// <summary>
		/// Swagger documentation for ModerateContentDataAsync
		/// </summary>
		internal static class ModerateContentDataAction
		{
			internal const string Summary = "Moderates the content data asynchronous.";
			internal const string Description = "Analyzes the provided user story content for inappropriate material and assigns a content rating to ensure compliance with community guidelines and content policies.";
			internal const string OperationId = nameof(ModerateContentDataAction);
		}
	}

	/// <summary>
	/// The Health Check Controller.
	/// </summary>
	internal static class HealthCheckController
	{
		/// <summary>
		/// Swagger documentation for GetAgentStatus
		/// </summary>
		internal static class GetAgentStatusAction
		{
			internal const string Summary = "Gets the agent status.";
			internal const string Description = "Gets the agent status from the app configuration via SignalR.";
			internal const string OperationId = nameof(GetAgentStatusAction);
		}
	}

}
