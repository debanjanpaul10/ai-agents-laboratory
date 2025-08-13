
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
		/// The story cannot be empty message.
		/// </summary>
		internal const string StoryCannotBeEmptyMessage = "The entered story/string is empty!";

		/// <summary>
		/// The model name not found exception constant
		/// </summary>
		internal const string ModelNameNotFoundExceptionConstant = "It seems the model name could not be determined";
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
	}
}
