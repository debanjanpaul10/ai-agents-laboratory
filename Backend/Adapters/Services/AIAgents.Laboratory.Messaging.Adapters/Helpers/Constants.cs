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

        /// <summary>
        /// The unable to relay message
        /// </summary>
        internal const string UnableToRelayMessage = "Unable to broadcast status change: Azure SignalR Service is not connected. This is expected when AI service is disabled. Error: {Message}";

        /// <summary>
        /// The no status change detected
        /// </summary>
        internal const string NoStatusChangeDetected = "No status change detected. Current status remains: {CurrentStatus}";

        /// <summary>
        /// The error broadcasting status change
        /// </summary>
        internal const string ErrorBroadcastingStatusChange = "Error broadcasting status change: {Message}";
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

        /// <summary>
        /// The email notification service connection string.
        /// </summary>
        internal const string EmailNotificationServiceConnectionString = "EmailNotification:ConnectionString";

        /// <summary>
        /// The email notification service sender address.
        /// </summary>
        internal const string EmailNotificationServiceSenderAddress = "EmailNotification:SenderAddress";
    }

    /// <summary>
    /// The Messaging Constants Class.
    /// </summary>
    internal static class MessagingConstants
    {
        /// <summary>
        /// The delay between iterations ms
        /// </summary>
        internal const int DelayBetweenIterationsMs = 60000;

        /// <summary>
        /// The agent status changed
        /// </summary>
        internal const string AgentStatusChanged = "AgentStatusChanged";

        /// <summary>
        /// The receive agent status function
        /// </summary>
        internal const string ReceiveAgentStatusFunction = "ReceiveAgentStatus";
    }

    /// <summary>
    /// The exception messages constant class.
    /// </summary>
    internal static class ExceptionMessagesConstants
    {
        /// <summary>
        /// The configuration missing exception message constant.
        /// </summary>
        internal const string ConfigurationMissingExceptionMessage = "The requested configuration was not found!";
    }
}
