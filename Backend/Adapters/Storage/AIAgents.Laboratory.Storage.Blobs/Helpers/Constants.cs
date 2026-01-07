namespace AIAgents.Laboratory.Storage.Blobs.Helpers;

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

        /// <summary>
        /// The GCP bucket name constant.
        /// </summary>
        internal const string GCPBucketNameConstant = "GCP:BucketName";

        /// <summary>
        /// The GCP folder name constant.
        /// </summary>
        internal const string GCPFolderNameConstant = "GCP:FolderName";

        /// <summary>
        /// The GCP service account JSON content constant.
        /// </summary>
        internal const string GCPServiceAccountJsonConstant = "GCP:ServiceAccountJson";
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

    /// <summary>
    /// The GCP cloud storage constants class.
    /// </summary>
    internal static class GCPCloudStorageConstants
    {
        /// <summary>
        /// The agent images folder structure format constant.
        /// </summary>
        internal const string AgentImagesFolderStructureFormat = "{0}/{1}";

        /// <summary>
        /// The GCP document public url constant.
        /// </summary>
        internal const string PublicUrlConstant = "https://storage.googleapis.com/{0}/{1}";
    }
}
