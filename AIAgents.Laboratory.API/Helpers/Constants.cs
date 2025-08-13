namespace AIAgents.Laboratory.API.Helpers;

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
	/// The Exception Constants class.
	/// </summary>
	internal static class ExceptionConstants
	{
		/// <summary>
		/// The ai services down message constant.
		/// </summary>
		internal const string AiServicesDownMessage = "Our AI Services are down right now. Please try again after sometime.";
	}
}
