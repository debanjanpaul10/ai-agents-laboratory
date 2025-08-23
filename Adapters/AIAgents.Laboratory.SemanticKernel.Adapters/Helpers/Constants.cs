// *********************************************************************************
//	<copyright file="AiConstants.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>AI Constants class.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.SemanticKernel.Adapters.Helpers;

/// <summary>
/// The AI Constants Class.
/// </summary>
internal static class Constants
{
	/// <summary>
	/// The Arguments Constants.
	/// </summary>
	internal static class ArgumentsConstants
	{
		/// <summary>
		/// The kernel arguments input constant.
		/// </summary>
		internal const string KernelArgumentsInputConstant = "input";

		/// <summary>
		/// The knowledge base input constant
		/// </summary>
		internal const string KnowledgeBaseInputConstant = "knowledge_base";

		/// <summary>
		/// The database schema input constant
		/// </summary>
		internal const string DatabaseSchemaInputConstant = "database_schema";

		/// <summary>
		/// The SQL query result input constant
		/// </summary>
		internal const string SQLQueryResultInputConstant = "sql_result";

		/// <summary>
		/// The SQL json input constant
		/// </summary>
		internal const string SQLJsonInputConstant = "sql_json";

		/// <summary>
		/// The total tokens count constant.
		/// </summary>
		internal const string TotalTokenCountConstant = "TotalTokenCount";

		/// <summary>
		/// The candidates token count constant.
		/// </summary>
		internal const string CandidatesTokenCountConstant = "CandidatesTokenCount";

		/// <summary>
		/// The prompt token count constant.
		/// </summary>
		internal const string PromptTokenCountConstant = "PromptTokenCount";
	}

	/// <summary>
	/// The Azure App Configuration Constants.
	/// </summary>
	internal static class AzureAppConfigurationConstants
	{
		/// <summary>
		/// The gemini pro model
		/// </summary>
		internal const string GeminiProModel = "GeminiProModel";

		/// <summary>
		/// The is pro model enabled flag
		/// </summary>
		internal const string IsProModelEnabledFlag = "IsProModelEnabled";

		/// <summary>
		/// The gemini API key constant
		/// </summary>
		internal const string GeminiAPIKeyConstant = "GeminiAPIKey";

		/// <summary>
		/// The gemini flash model
		/// </summary>
		internal const string GeminiFlashModel = "GeminiFlashModel";
	}

	/// <summary>
	/// Logging constants.
	/// </summary>
	internal static class LoggingConstants
	{
		/// <summary>
		/// The log helper method start.
		/// </summary>
		internal const string LogHelperMethodStart = "{0} started at {1}";

		/// <summary>
		/// The log helper method failed.
		/// </summary>
		internal const string LogHelperMethodFailed = "{0} failed at {1} with {2}";

		/// <summary>
		/// The log helper method end.
		/// </summary>
		internal const string LogHelperMethodEnd = "{0} ended at {1}";
	}

	/// <summary>
	/// The Exception Constants Class.
	/// </summary>
	internal static class ExceptionConstants
	{
		/// <summary>
		/// The ai api key missing message.
		/// </summary>
		public const string AiAPIKeyMissingMessage = "The AI Api Key is missing in configuration.";

		/// <summary>
		/// Something went wrong message
		/// </summary>
		internal const string SomethingWentWrongMessage = "Something went wrong while processing the request!";

		/// <summary>
		/// The default ai exception message
		/// </summary>
		internal const string DefaultAIExceptionMessage = "I am currently unable to access the required information to process this request. Please try again later!";
	}

	/// <summary>
	/// The external api route constants class.
	/// </summary>
	internal static class ExternalApiRouteConstants
	{
		/// <summary>
		/// The Fit Gym Tool API constants class.
		/// </summary>
		internal static class FitGymToolAPI
		{
			/// <summary>
			/// The SQL knowledge base SQL API route
			/// </summary>
			internal const string SqlKnowledgeBaseSql_ApiRoute = "aiservices/getsqlknowledgebasejson";

			/// <summary>
			/// The database schema SQL API route
			/// </summary>
			internal const string DatabaseSchemaSql_ApiRoute = "aiservices/getdatabaseschemajson";

			/// <summary>
			/// The rag knowledge base API route
			/// </summary>
			internal const string RagKnowledgeBase_ApiRoute = "aiservices/getragknowledgebasejson";
		}
	}

	/// <summary>
	/// The intent constants class.
	/// </summary>
	internal static class IntentConstants
	{
		/// <summary>
		/// The greeting intent
		/// </summary>
		internal const string GreetingIntent = "GREETING";

		/// <summary>
		/// The SQL intent
		/// </summary>
		internal const string SQLIntent = "SQL";

		/// <summary>
		/// The rag intent
		/// </summary>
		internal const string RAGIntent = "RAG";

		/// <summary>
		/// The unclear intent
		/// </summary>
		internal const string UnclearIntent = "UNCLEAR";
	}

	/// <summary>
	/// The Configuration Constants class.
	/// </summary>
	internal static class ConfigurationConstants
	{
		/// <summary>
		/// The tenant identifier constant
		/// </summary>
		internal const string FGToolTenantIdConstant = "FitGymTool:TenantId";

		/// <summary>
		/// The fg tool client identifier constant
		/// </summary>
		internal const string FGToolClientIdConstant = "FitGymTool:ClientId";

		/// <summary>
		/// The fg tool client secret constant
		/// </summary>
		internal const string FGToolClientSecretConstant = "FitGymTool:ClientSecret";

		/// <summary>
		/// The bearer constant
		/// </summary>
		internal const string BearerConstant = "Bearer";

		/// <summary>
		/// The application json constant
		/// </summary>
		internal const string ApplicationJsonConstant = "application/json";

		/// <summary>
		/// The fg tool HTTP client
		/// </summary>
		internal const string FGToolHttpClient = nameof(FGToolHttpClient);

		/// <summary>
		/// The token scope format.
		/// </summary>
		internal const string TokenScopeFormat = "{0}/.default";

		/// <summary>
		/// The is development mode constant
		/// </summary>
		internal const string IsDevelopmentModeConstant = "IsDevelopmentMode";

		/// <summary>
		/// The managed identity client identifier constant
		/// </summary>
		internal const string ManagedIdentityClientIdConstant = "ManagedIdentityClientId";

		/// <summary>
		/// The FG Tool api base URL
		/// </summary>
		internal const string FitGymToolApiBaseUrl = "FitGymToolApiBaseUrl";
	}

}
