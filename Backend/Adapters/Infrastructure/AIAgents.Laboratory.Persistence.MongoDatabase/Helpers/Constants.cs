namespace AIAgents.Laboratory.Persistence.MongoDatabase.Helpers;

/// <summary>
/// The constants class.
/// </summary>
internal static class Constants
{
    /// <summary>
    /// The Configuration Constants.
    /// </summary>
    internal static class ConfigurationConstants
    {
        /// <summary>
        /// The ai agents lab mongo connection string
        /// </summary>
        internal const string AiAgentsLabMongoConnectionString = "MongoDbConnectionString";
    }

    // <summary>
    /// The Logging Constants Class.
    /// </summary>
    internal static class LoggingConstants
    {
        /// <summary>
        /// The method started message constant
        /// </summary>
        internal const string MethodStartedMessageConstant = "Method {0} started at {1} for {2}";

        /// <summary>
        /// The method ended message constant
        /// </summary>
        internal const string MethodEndedMessageConstant = "Method {0} ended at {1} for {2}";

        /// <summary>
        /// The method failed with message constant.
        /// </summary>
        /// <returns>{0} failed at {1} with {2}</returns>
        internal const string MethodFailedWithMessageConstant = "Method {0} failed at {1} with {2}";
    }

    /// <summary>
    /// The exception constants class.
    /// </summary>
    internal static class ExceptionConstants
    {
        /// <summary>
        /// Something went wrong message constant
        /// </summary>
        internal const string SomethingWentWrongMessageConstant = "Oops! Something went wrong while processing this request!";

        /// <summary>
        /// The collection does not exists message
        /// </summary>
        internal const string CollectionDoesNotExistsMessage = "Oops! It seems the collection does not exists";

        /// <summary>
        /// No document was updated message constant.
        /// </summary>
        internal const string NoDocumentWasUpdatedMessageConstant = "No document was updated. Document may not exist or no changes were needed.";

        /// <summary>
        /// The exception message for failure to configure MongoDB client.
        /// </summary>
        internal const string FailedToConfigureMongoDbClientExceptionMessage = "Failed to configure MongoDB client. Connection string format may be incorrect. Error: {0}";
    }

}
