// *********************************************************************************
//	<copyright file="RouteConstants.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Route Constants Class.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.API.Helpers;

/// <summary>
/// The Route Constants Class.
/// </summary>
public static class RouteConstants
{
	/// <summary>
	/// The ai base route prefix
	/// </summary>
	public const string AiBase_RoutePrefix = "aiagentsapi";

	/// <summary>
	/// The agent status hub route
	/// </summary>
	public const string AgentStatusHub_Route = "/hubs/agent-status";

	/// <summary>
	/// The route constants for plugins.
	/// </summary>
	internal static class Plugins
	{
		/// <summary>
		/// The rewrite text route
		/// </summary>
		public const string RewriteText_Route = "rewritetext";

		/// <summary>
		/// The generate tag route
		/// </summary>
		public const string GenerateTag_Route = "generatetag";

		/// <summary>
		/// The moderate content route
		/// </summary>
		public const string ModerateContent_Route = "moderatecontent";

		/// <summary>
		/// The get bug severity route
		/// </summary>
		internal const string GetBugSeverity_Route = "getbugseverity";
	}

	/// <summary>
	/// The AI skills routes.
	/// </summary>
	internal static class AISkillsRoutes
	{

		/// <summary>
		/// The get SQL query markdown response route
		/// </summary>
		internal const string GetSQLQueryMarkdownResponse_Route = "getsqlquerymarkdownresponse";

		/// <summary>
		/// The get followup questions api route.
		/// </summary>
		internal const string GetFollowupQuestionsResponse_Route = "getfollowupquestionsresponse";

		/// <summary>
		/// The detect user intent route
		/// </summary>
		internal const string DetectUserIntent_Route = "intentdetectionskill";

		/// <summary>
		/// The get user greeting response route
		/// </summary>
		internal const string GetUserGreetingResponse_Route = "usergreetingskill";

		/// <summary>
		/// The get rag text response route
		/// </summary>
		internal const string GetRAGTextResponse_Route = "ragtextresponseskill";

		/// <summary>
		/// The get nl to SQL response route
		/// </summary>
		internal const string GetNlToSqlResponse_Route = "nltosqlskill";
	}

	/// <summary>
	/// The Health Check Routes.
	/// </summary>
	public static class HealthCheck
	{
		/// <summary>
		/// The get agent status route
		/// </summary>
		public const string GetAgentStatus_Route = "agentstatus";
	}
}
