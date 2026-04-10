using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AI.Agents.Laboratory.Functions.Business.Contracts;
using AI.Agents.Laboratory.Functions.Business.Services;
using AI.Agents.Laboratory.Functions.Data.Contracts;
using AI.Agents.Laboratory.Functions.Data.Services;
using AI.Agents.Laboratory.Functions.Shared.Constants;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace AI.Agents.Laboratory.Functions.IOC;

/// <summary>
/// This static class is responsible for configuring dependency injection for the Azure Functions application. 
/// </summary>
/// <remarks>
/// It includes methods to set up Azure App Configuration, register business, data, and repository dependencies, and configure the MongoDB client for database interactions. 
/// The class ensures that all necessary services are registered in the IServiceCollection, allowing for seamless dependency injection throughout the application. 
/// It also includes logic to determine the root directory for configuration file loading, ensuring that the application can correctly access configuration settings regardless of the execution context.
///</remarks>
[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    /// <summary>
    /// Configures Azure App Configuration for the application, allowing it to retrieve configuration values from Azure App Configuration service. 
    /// It sets up authentication using Managed Identity in production and DefaultAzureCredential in development, and ensures that the necessary configuration keys are selected for use in the application.
    /// </summary>
    /// <param name="context">The HostBuilderContext provides information about the hosting environment and configuration during application startup.</param>
    /// <param name="config">The IConfigurationBuilder is used to build the application's configuration, allowing it to read from various sources such as JSON files, environment variables, and Azure App Configuration.</param>
    public static IConfigurationBuilder ConfigureAzureAppConfiguration(this HostBuilderContext context, IConfigurationBuilder config)
    {
        var environment = Environment.GetEnvironmentVariable(EnvironmentConfigurationConstants.AzureFunctionsEnvironmentConstant)
            ?? context.HostingEnvironment.EnvironmentName;
        var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var rootDirectory = GetRootDirectory(currentDirectory!);
        config.SetBasePath(rootDirectory)
            .AddJsonFile(
                path: EnvironmentConfigurationConstants.AppsettingsLocalJsonFileName,
                optional: true, reloadOnChange: false)
            .AddJsonFile(
                path: string.Format(EnvironmentConfigurationConstants.AppsettingsEnvironmentBasedFileName, environment),
                optional: true, reloadOnChange: false)
            .AddEnvironmentVariables();

        config.AddUserSecrets(
            assembly: Assembly.GetExecutingAssembly(),
            optional: true
        );

        // Build the configuration to access the Managed Identity Client ID for Azure App Configuration authentication
        var configuration = config.Build();
        var miCredentials = configuration[EnvironmentConfigurationConstants.ManagedIdentityClientIdConstant];

        var credentials = context.HostingEnvironment.IsDevelopment()
            ? new DefaultAzureCredential()
            : new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                ManagedIdentityClientId = miCredentials,
            });

        var appConfigurationEndpoint = configuration[EnvironmentConfigurationConstants.AppConfigurationEndpointKeyConstant];
        if (string.IsNullOrEmpty(appConfigurationEndpoint))
            throw new InvalidOperationException(ExceptionConstants.MissingConfigurationMessage);

        config.AddAzureAppConfiguration(options =>
        {
            options.Connect(new Uri(appConfigurationEndpoint), credentials)
                .Select(KeyFilter.Any)
                .Select(KeyFilter.Any, AzureAppConfigurationConstants.BaseConfigurationAppConfigKeyConstant)
                .Select(KeyFilter.Any, AzureAppConfigurationConstants.FeatureFlagAppConfigKeyConstant)
                .Select(KeyFilter.Any, AzureAppConfigurationConstants.MongoDbAppConfigKeyConstant)
                .Select(KeyFilter.Any, AzureAppConfigurationConstants.FunctionAppConfigKeyConstant)
            .ConfigureKeyVault(configure => configure.SetCredential(credentials));
        });
        return config;
    }

    /// <summary>
    /// Adds business layer dependencies to the service collection, allowing for dependency injection of business services throughout the application.
    /// </summary>
    /// <param name="services">The IServiceCollection is used to register services for dependency injection, enabling the application to resolve and inject these services where needed.</param>
    /// <returns>The updated IServiceCollection with the registered business dependencies.</returns>
    public static IServiceCollection AddBusinessDependencies(this IServiceCollection services) =>
        services.AddScoped<IPushNotificationsService, PushNotificationsService>();

    /// <summary>
    /// Adds data layer dependencies to the service collection, allowing for dependency injection of data services throughout the application.
    /// </summary>
    /// <param name="services">The IServiceCollection is used to register services for dependency injection, enabling the application to resolve and inject these services where needed.</param>
    /// <returns>The updated IServiceCollection with the registered data dependencies.</returns>
    public static IServiceCollection AddDataDependencies(this IServiceCollection services, IConfiguration configuration) =>
        services.ConfigureMongoDbServer(configuration)
            .AddScoped<INotificationsDataManager, NotificationsDataManager>();

    /// <summary>
    /// Adds repository layer dependencies to the service collection, allowing for dependency injection of repository services throughout the application.
    /// </summary>
    /// <param name="services">The IServiceCollection to register the repository services in.</param>
    /// <returns>The updated IServiceCollection with the registered repository dependencies.</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddScoped<IMongoDatabaseRepository, MongoDatabaseRepository>();

    /// <summary>
    /// Configures the MongoDB server connection settings and registers the MongoDB client in the service collection for dependency injection. 
    /// </summary>
    /// <remarks>It retrieves the MongoDB connection string from the configuration, sets up the MongoClientSettings with TLS and SSL settings, and adds the IMongoClient as a singleton service to the IServiceCollection, 
    /// allowing it to be injected and used throughout the application for database operations.</remarks>
    /// <param name="services">The IServiceCollection to register the MongoDB client in.</param>
    /// <param name="configuration">The IConfiguration to retrieve the MongoDB connection string from.</param>
    /// <returns>The updated IServiceCollection with the registered MongoDB client.</returns>
    private static IServiceCollection ConfigureMongoDbServer(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbConnectionString = configuration[MongoDbConfigurationConstants.MongoDbConnectionString];
        ArgumentException.ThrowIfNullOrWhiteSpace(mongoDbConnectionString);

        var settings = MongoClientSettings.FromConnectionString(mongoDbConnectionString);
        settings.UseTls = true;
        settings.SslSettings = new SslSettings
        {
            EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13,
            CheckCertificateRevocation = false
        };

        services.AddSingleton<IMongoClient>(new MongoClient(settings));
        return services;
    }

    /// <summary>
    /// Determines the root directory for configuration file loading based on the current directory of the executing assembly. 
    /// </summary>
    /// <remarks>It checks if the current directory contains the configuration files, and if not, it traverses up one level in the directory structure to find the root directory where the configuration files are located. 
    /// This ensures that the application can correctly load configuration files regardless of the execution context or directory structure.</remarks>
    /// <param name="currentDirectory">The current directory to check for configuration files.</param>
    /// <returns>The root directory where the configuration files are located.</returns>
    private static string GetRootDirectory(string currentDirectory)
    {
        string rootDirectory;
        if (string.IsNullOrEmpty(currentDirectory))
            rootDirectory = Directory.GetCurrentDirectory();

        else if (Directory.GetFiles(currentDirectory, EnvironmentConfigurationConstants.AppsettingsSearchPattern, SearchOption.TopDirectoryOnly).Length > 0)
            rootDirectory = currentDirectory;

        else
            rootDirectory = Path.GetFullPath(Path.Combine(currentDirectory, ".."));

        return rootDirectory;
    }
}
