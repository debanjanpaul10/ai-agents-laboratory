namespace AIAgents.Laboratory.Domain.Helpers;

/// <summary>
/// The cache keys constants class.
/// </summary>
internal static class CacheKeys
{
    /// <summary>
    /// The all appsettings key name.
    /// </summary>
    internal const string AllAppSettingsKeyName = "AllAppsettingsCache";

    /// <summary>
    /// The cachek key expiration timeoutvalue.
    /// </summary>
    internal static TimeSpan CacheExpirationTimeout = TimeSpan.FromMinutes(15);
}
