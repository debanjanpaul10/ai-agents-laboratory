// *********************************************************************************
//	<copyright file="Constants.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Constants Class.</summary>
// *********************************************************************************

namespace AIAgents.Laboratory.Messaging.Adapters.Helpers;

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
	/// The Azure App Configuration Constants.
	/// </summary>
	internal static class AzureAppConfigurationConstants
	{
		/// <summary>
		/// The is ai service enabled constant
		/// </summary>
		internal const string IsAIServiceEnabledConstant = "IsAIServiceEnabled";
	}

	/// <summary>
	/// The Messaging Constants Class.
	/// </summary>
	internal static class MessagingConstants
	{
		/// <summary>
		/// The delay between iterations ms
		/// </summary>
		internal const int DelayBetweenIterationsMs = 10000; 
	}
}
