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
	}

}
