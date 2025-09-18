// *********************************************************************************
//	<copyright file="Constants.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Constants Class.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The Constants Class.
/// </summary>
internal static class Constants
{
	/// <summary>
	/// Logging constants.
	/// </summary>
	internal static class LoggingConstants
	{
		/// <summary>
		/// The log helper method start.
		/// </summary>
		internal const string LogHelperMethodStart = "{0} started at {1} for {2}";

		/// <summary>
		/// The log helper method failed.
		/// </summary>
		internal const string LogHelperMethodFailed = "{0} failed at {1} with {2}";

		/// <summary>
		/// The log helper method end.
		/// </summary>
		internal const string LogHelperMethodEnd = "{0} ended at {1} for {2}";
	}

	/// <summary>
	/// The Exception Constants Class.
	/// </summary>
	internal static class ExceptionConstants
	{
		/// <summary>
		/// The story cannot be empty message.
		/// </summary>
		internal const string StoryCannotBeEmptyMessage = "The entered story/string is empty!";

		/// <summary>
		/// The model name not found exception constant
		/// </summary>
		internal const string ModelNameNotFoundExceptionConstant = "It seems the model name could not be determined";

		/// <summary>
		/// The input parameters cannot be empty message
		/// </summary>
		internal const string InputParametersCannotBeEmptyMessage = "The input parameters cannot be empty";

		/// <summary>
		/// The cannot process user query message
		/// </summary>
		internal const string CannotProcessUserQueryMessage = "This message query cannot be processed right now. Please try something else!";

		/// <summary>
		/// The agent not found exception message
		/// </summary>
		internal const string AgentNotFoundExceptionMessage = "Oops! It seems the AI agent you are looking for does not exists!";
	}

	/// <summary>
	/// The Azure App Configuration Constants Class.
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
		/// The gemini flash model
		/// </summary>
		internal const string GeminiFlashModel = "GeminiFlashModel";

		/// <summary>
		/// The azure application configuration constant
		/// </summary>
		internal const string AzureAppConfigurationConstant = "AzureAppConfiguration";
	}

	/// <summary>
	/// The MongoDB collection constants class.
	/// </summary>
	internal static class MongoDbCollectionConstants
	{
		/// <summary>
		/// The skills database name
		/// </summary>
		internal const string AiAgentsPrimaryDatabase = "ai-agents-primary";

		/// <summary>
		/// The agents collection name
		/// </summary>
		internal const string AgentsCollectionName = "agents";
	}

}
