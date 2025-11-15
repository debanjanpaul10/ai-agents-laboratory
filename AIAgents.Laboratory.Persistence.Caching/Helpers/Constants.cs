namespace AIAgents.Laboratory.Persistence.Caching.Helpers;

/// <summary>
/// The Caching Constants class.
/// </summary>
internal class Constants
{
    /// <summary>
    /// The logging constants class.
    /// </summary>
    internal static class LoggingConstants
    {
        /// <summary>
        /// The method started message constant
        /// </summary>
        public const string MethodStartedMessageConstant = "Method {0} started at {1} for {2}";

        /// <summary>
        /// The method ended message constant
        /// </summary>
        public const string MethodEndedMessageConstant = "Method {0} ended at {1} for {2}";

        /// <summary>
        /// The method failed with message constant.
        /// </summary>
        /// <returns>{0} failed at {1} with {2}</returns>
        public const string MethodFailedWithMessageConstant = "Method {0} failed at {1} with {2}";

        /// <summary>
        /// The cache key not found message constant.
        /// </summary>
        internal const string CacheKeyNotFoundMessageConstant = "Cache service could not find the key: {0}";

        /// <summary>
        /// The cache key found message constant.
        /// </summary>
        internal const string CacheKeyFoundMessageConstant = "Cache service found the existing key: {0}";
    }

    /// <summary>
    /// The Exception constants class.
    /// </summary>
    internal static class ExceptionConstants
    {
        /// <summary>
        /// The key name is null message constant
        /// </summary>
        internal const string KeyNameIsNullMessageConstant = "Key name is null or empty";
    }
}
