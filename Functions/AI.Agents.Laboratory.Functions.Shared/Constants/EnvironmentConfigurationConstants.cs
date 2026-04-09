namespace AI.Agents.Laboratory.Functions.Shared.Constants;

/// <summary>
/// The Environment Configuration Constants.
/// </summary>
public static class EnvironmentConfigurationConstants
{
    /// <summary>
    /// The application configuration endpoint key constant
    /// </summary>
    public const string AppConfigurationEndpointKeyConstant = "AppConfigurationEndpoint";

    /// <summary>
    /// The managed identity client identifier constant
    /// </summary>
    public const string ManagedIdentityClientIdConstant = "ManagedIdentityClientId";

    /// <summary>
    /// The Azure Functions environment variable name constant, used to determine the current hosting environment of the Azure Function application (e.g., Development, Staging, Production) and to load the appropriate configuration settings based on that environment.
    /// </summary>
    public const string AzureFunctionsEnvironmentConstant = "AZURE_FUNCTIONS_ENVIRONMENT";

    /// <summary>
    /// The appsettings local JSON file name constant, representing the default configuration file name (appsettings.json) used for local development and configuration settings in the Azure Function application. This constant is used to ensure consistency when referencing the local configuration file throughout the application.
    /// </summary>
    public const string AppsettingsLocalJsonFileName = "appsettings.json";

    /// <summary>
    /// The appsettings environment-based file name constant, representing the naming convention for environment-specific configuration files (e.g., appsettings.Development.json, appsettings.Production.json) used in the Azure Function application. This constant includes a placeholder for the environment name, allowing the application to dynamically load the appropriate configuration file based on the current hosting environment.
    /// </summary>
    public const string AppsettingsEnvironmentBasedFileName = "appsettings.{0}.json";

    /// <summary>
    /// The appsettings search pattern constant, used to define the search pattern for configuration files when loading settings in the Azure Function application. This constant allows the application to identify and load all configuration files that match the specified pattern (e.g., appsettings.*.json), enabling support for multiple environment-specific configuration files and ensuring that the application can access the necessary settings based on the current hosting environment.
    /// </summary>
    public const string AppsettingsSearchPattern = "appsettings.*";
}
