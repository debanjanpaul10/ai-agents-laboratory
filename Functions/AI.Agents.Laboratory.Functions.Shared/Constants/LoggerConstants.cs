namespace AI.Agents.Laboratory.Functions.Shared.Constants;

/// <summary>
/// The <c>LoggerConstants</c> class defines constant values used for logging throughout the application.
/// </summary>
public static class LoggerConstants
{
    /// <summary>
    /// The Correlation Id header constant.
    /// </summary>
    public const string CorrelationIdHeader = "X-Correlation-ID";

    /// <summary>
    /// The log helper method start.
    /// </summary>
    public const string LogHelperMethodStart = "{0} started at {1} for {2}";

    /// <summary>
    /// The log helper method failed.
    /// </summary>
    public const string LogHelperMethodFailed = "{0} failed at {1} with {2}";

    /// <summary>
    /// The log helper method end.
    /// </summary>
    public const string LogHelperMethodEnd = "{0} ended at {1} for {2}";

    /// <summary>
    /// The http logging message constant.
    /// </summary>
    public const string HttpLoggingMessage = "HTTP {Method} {Path} started";

    /// <summary>
    /// The http logging message constant with time elapsed.
    /// </summary>
    public const string HttpLoggingMessageWithTime = "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms";

    /// <summary>
    /// The unhandled exception logging message constant.
    /// </summary>
    public const string UnhandledExceptionMessage = "Unhandled exception occurred. CorrelationId: {CorrelationId}, Path: {Path}, Method: {Method}";

    /// <summary>
    /// The header logging constants class.
    /// </summary>
    public static class HeaderLoggingConstants
    {
        /// <summary>
        /// The correlation id constant.
        /// </summary>
        public const string CorrelationId = "CorrelationId";

        /// <summary>
        /// The request path constant.
        /// </summary>
        public const string RequestPath = "RequestPath";

        /// <summary>
        /// The request method constant.
        /// </summary>
        public const string RequestMethod = "RequestMethod";

        /// <summary>
        /// The status code constant.
        /// </summary>
        public const string StatusCode = "StatusCode";
    }
}
