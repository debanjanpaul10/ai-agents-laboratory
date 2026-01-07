namespace AIAgents.Laboratory.Storage.Cloudinary.Helpers;

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
    /// The Azure app configuration constants class.
    /// </summary>
    internal static class AzureAppConfigurationConstants
    {
        /// <summary>
        /// The cloudinary cloud name constant.
        /// </summary>
        internal const string CloudinaryCloudNameConstant = "Cloudinary:CloudName";

        /// <summary>
        /// The cloudinary api key constant.
        /// </summary>
        internal const string CloudinaryApiKeyConstant = "Cloudinary:APIKey";

        /// <summary>
        /// The cloudinary api secret constant.
        /// </summary>
        internal const string CloudinaryApiSecretConstant = "Cloudinary:APISecret";

        /// <summary>
        /// The cloudinary folder name constant.
        /// </summary>
        internal const string CloudinaryFolderNameConstant = "Cloudinary:FolderName";
    }

    /// <summary>
    /// The cloudinary constants class.
    /// </summary>
    internal static class CloudinaryConstants
    {
        /// <summary>
        /// The agent images folder structure format constant.
        /// </summary>
        internal const string AgentImagesFolderStructureFormat = "{0}/{1}";
    }
}
