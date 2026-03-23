namespace AIAgents.Laboratory.Persistence.SQLDatabase.Helpers;

/// <summary>
/// The constants class.
/// </summary>
internal static class Constants
{
    /// <summary>
    /// The Configuration Constants Class.
    /// </summary>
    internal static class ConfigurationConstants
    {
        /// <summary>
        /// The local SQL database connection string constant.
        /// </summary>
        internal const string LocalSqlConnectionStringConstant = "LocalSqlConnectionString";

        /// <summary>
        /// The Azure SQL database connection string constant.
        /// </summary>
        internal const string AzureSqlConnectionStringConstant = "AzureSqlConnectionString";

        /// <summary>
        /// The current sql service provider constant.
        /// </summary>
        internal const string CurrentSQLProviderConstant = "CurrentSQLProvider";
    }

    /// <summary>
    /// The Logging Constants Class.
    /// </summary>
    internal static class LoggingConstants
    {
        /// <summary>
        /// The method started message constant
        /// </summary>
        internal static readonly string MethodStartedMessageConstant = "Method {0} started at {1} for {2}";

        /// <summary>
        /// The method ended message constant
        /// </summary>
        internal static readonly string MethodEndedMessageConstant = "Method {0} ended at {1} for {2}";

        /// <summary>
        /// The method failed with message constant.
        /// </summary>
        /// <returns>{0} failed at {1} with {2}</returns>
        internal const string MethodFailedWithMessageConstant = "Method {0} failed at {1} with {2}";
    }

    /// <summary>
    /// The database constants class.
    /// </summary>
    internal static class DatabaseConstants
    {
        /// <summary>
        /// The constant for the Not Applicable string value.
        /// </summary>
        public const string NotApplicableStringConstant = "NA";

        /// <summary>
        /// The medium constant
        /// </summary>
        public const string MediumConstant = "Medium";

        /// <summary>
        /// The not started constant
        /// </summary>
        public const string NotStartedConstant = "Not Started";

        /// <summary>
        /// The Postgres SQL constant.
        /// </summary>
        internal const string PostgreSQLConstant = "PostgreSQL";

        /// <summary>
        /// The Azure SQL constant.
        /// </summary>
        internal const string AzureSQLConstant = "AzureSQL";

        /// <summary>
        /// The IsActive boolean flag constant used for filtering active records in database queries and operations.
        /// </summary>
        internal const string IsActiveBooleanFlag = "IsActive";
    }

    /// <summary>
    /// Provides constant values used for health check operations within the application.
    /// </summary>
    internal static class HealthCheckConstants
    {
        /// <summary>
        /// The database health query constant.
        /// </summary>
        internal const string DBHealthQuery = "SELECT 1";

        /// <summary>
        /// The application name constant used for health check tagging and identification.
        /// </summary>
        internal const string ApplicationName = "AI.Agents.Laboratory";

        /// <summary>
        /// The Azure SQL health check tag constant used to categorize health checks related to Azure SQL databases in monitoring and diagnostics.
        /// </summary>
        internal const string AzureSQLHealthCheckTag = "AzureSQL.Database";
    }
}
