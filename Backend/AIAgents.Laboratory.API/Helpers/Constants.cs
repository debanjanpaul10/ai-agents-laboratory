namespace AIAgents.Laboratory.API.Helpers;

/// <summary>
/// The Constants Class.
/// </summary>
internal static class Constants
{
    /// <summary>
    /// The Swagger Constants class.
    /// </summary>
    internal static class SwaggerConstants
    {
        public const string ApiVersion = "v1";
        public const string SwaggerEndpointUrl = "/swagger/v1/swagger.json";
        public const string SwaggerDescription = "API documentation for AI.Agents.Laboratory";

        public const string SwaggerUiPrefix = "swaggerui";
        public const string ApplicationAPIName = "AI.Agents.Laboratory.API";

        /// <summary>
        /// The Author Details class contains information about the author of the API.
        /// </summary>
        public static class AuthorDetails
        {
            public static readonly string Name = "Debanjan Paul";
            public static readonly string Email = "debanjanpaul10@gmail.com";
        }
    }

    /// <summary>
    /// Logging constants.
    /// </summary>
    internal static class LoggingConstants
    {
        internal const string CorrelationIdHeader = "X-Correlation-ID";

        internal const string LogHelperMethodStart = "{0} started at {1} for {2}";
        internal const string LogHelperMethodFailed = "{0} failed at {1} with {2}";
        internal const string LogHelperMethodEnd = "{0} ended at {1} for {2}";

        internal const string HttpLoggingMessage = "HTTP {Method} {Path} started";

        internal const string HttpLoggingMessageWithTime = "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms";
        internal const string UnhandledExceptionMessage = "Unhandled exception occurred. CorrelationId: {CorrelationId}, Path: {Path}, Method: {Method}";
        internal const string NotificationStreamCancelledMessage = "Notification stream cancelled.";

        /// <summary>
        /// The header logging constants class.
        /// </summary>
        internal static class HeaderLoggingConstants
        {
            internal const string CorrelationId = "CorrelationId";
            internal const string RequestPath = "RequestPath";
            internal const string RequestMethod = "RequestMethod";
            internal const string StatusCode = "StatusCode";
        }
    }

    /// <summary>
    /// The Environment Configuration Constants.
    /// </summary>
    internal static class EnvironmentConfigurationConstants
    {
        internal const string LocalAppsetingsFileName = "appsettings.development.json";

        internal const string AppConfigurationEndpointKeyConstant = "AppConfigurationEndpoint";
        internal const string ManagedIdentityClientIdConstant = "ManagedIdentityClientId";
        internal const string ApplicationJsonConstant = "application/json";
    }

    /// <summary>
    /// The Exception Constants class.
    /// </summary>
    internal static class ExceptionConstants
    {
        /// <summary>
        /// The default message for unhandled exceptions, providing a user-friendly response when an unexpected error occurs during request processing.
        /// </summary>
        internal const string SomethingWentWrongDefaultMessage = "Oops! Something went wrong while processing the request. Please try again after sometime!";

        /// <summary>
        /// The missing configuration message.
        /// </summary>
        public const string MissingConfigurationMessage = "The Configuration Key is missing";

        /// <summary>
        /// The invalid token exception constant.
        /// </summary>
        public const string InvalidTokenExceptionConstant = "Invalid token: Identity is not authenticated.";

        /// <summary>
        /// The unauthorized access message exception constant.
        /// </summary>
        internal const string UnauthorizedAccessMessageConstant = "Unauthorized access. Please log in to continue.";

        /// <summary>
        /// The file not found exception message constant.
        /// </summary>
        internal const string FileNotFoundExceptionMessageConstant = "Oops! The file could not be downloaded at this moment!";

        /// <summary>
        /// The conversation history cannot be fetched exception message constant.
        /// </summary>
        internal const string ConversationHistoryCannotBeFetchedMessageConstant = "Oops! It seems the conversation history could not be fetched!";

        /// <summary>
        /// The conversation history cannot be fetched exception message constant.
        /// </summary>
        internal const string ConversationHistoryCannotBeClearedMessageConstant = "Oops! It seems the conversation history cannot be cleared or there does not exists any!";

        /// <summary>
        /// The invalid bug report data message.
        /// </summary>
        internal const string InvalidBugReportDataMessage = "The bug report data provided is invalid.";

        /// <summary>
        /// The invalid feature request data message.
        /// </summary>
        internal const string InvalidFeatureRequestDataMessage = "The feature request data provided is invalid.";

        /// <summary>
        /// The requested data not found exception message constant.
        /// </summary>
        internal const string DataCannotBeFoundExceptionMessage = "Oops! The requested data not exist!";
    }

    /// <summary>
    /// The Azure App Configurations Constants.
    /// </summary>
    internal static class AzureAppConfigurationConstants
    {
        /// <summary>
        /// The base configuration application configuration key constant
        /// </summary>
        internal const string BaseConfigurationAppConfigKeyConstant = "BaseConfiguration";

        /// <summary>
        /// The feature flag application configuration key constant.
        /// </summary>
        internal const string FeatureFlagAppConfigKeyConstant = "FeatureFlag";

        /// <summary>
        /// The ai config application configuration key constant.
        /// </summary>
        internal const string AiConfigurationAppConfigKeyConstant = "AiConfiguration";

        /// <summary>
        /// The mongo database application configuration key constant.
        /// </summary>
        internal const string MongoDbAppConfigKeyConstant = "MongoDb";

        /// <summary>
        /// The azure ad tenant identifier constant
        /// </summary>
        internal const string AzureAdTenantIdConstant = "TenantId";

        /// <summary>
        /// The token format url.
        /// </summary>
        internal const string TokenFormatUrlConstant = "TokenFormatUrl";

        /// <summary>
        /// The ai agents client identifier constant
        /// </summary>
        internal const string AIAgentsClientIdConstant = "AiAgentsClientId";

        /// <summary>
        /// The application insights connection string constant.
        /// </summary>
        internal const string ApplicationInsightsConnectionString = "ApplicationInsights:ConnectionString";
    }

    /// <summary>
    /// The Header constants class.
    /// </summary>
    internal static class HeaderConstants
    {
        /// <summary>
        /// The user full name claim constant.
        /// </summary>
        public const string NotApplicableStringConstant = "NA";

        /// <summary>
        /// The user email claim constant.
        /// </summary>
        public const string UserEmailClaimConstant = "preferred_username";

        /// <summary>
        /// The client id claim constant.
        /// </summary>
        public const string ClientIdClaimConstant = "aud";

        /// <summary>
        /// The cache control header
        /// </summary>
        internal const string CacheControlHeader = "no-cache";

        /// <summary>
        /// The connection control header
        /// </summary>
        internal const string ConnectionControlHeader = "keep-alive";

        /// <summary>
        /// The content type control header
        /// </summary>
        internal const string ContentTypeControlHeader = "text/event-stream";
    }
}
