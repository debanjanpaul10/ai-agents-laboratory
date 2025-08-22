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
	/// The IBBS AI Routes.
	/// </summary>
	public static class IBBSAi
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
	}

	/// <summary>
	/// The FitGymTool AI Routes.
	/// </summary>
	internal static class FitGymToolAi
	{
		/// <summary>
		/// The get bug severity route
		/// </summary>
		internal const string GetBugSeverity_Route = "getbugseverity";

		/// <summary>
		/// The get orchestrator response route
		/// </summary>
		internal const string GetChatbotResponse_Route = "getchatbotresponse";

		/// <summary>
		/// The get SQL query markdown response route
		/// </summary>
		internal const string GetSQLQueryMarkdownResponse_Route = "getsqlquerymarkdownresponse";
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
