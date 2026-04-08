using System.Diagnostics.CodeAnalysis;
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
/// The Dependency Injection Container Class.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    /// <summary>
    /// Configures Azure App Configuration for the application, allowing it to retrieve configuration values from Azure App Configuration service. 
    /// It sets up authentication using Managed Identity in production and DefaultAzureCredential in development, and ensures that the necessary configuration keys are selected for use in the application.
    /// </summary>
    /// <param name="context">The HostBuilderContext provides information about the hosting environment and configuration during application startup.</param>
    /// <param name="config">The IConfigurationBuilder is used to build the application's configuration, allowing it to read from various sources such as JSON files, environment variables, and Azure App Configuration.</param>
    public static void ConfigureAzureAppConfiguration(this HostBuilderContext context, IConfigurationBuilder config)
    {
        if (context.HostingEnvironment.IsDevelopment())
            config.AddJsonFile(EnvironmentConfigurationConstants.LocalAppsetingsFileName, optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();

        // Build the configuration to access the Managed Identity Client ID for Azure App Configuration authentication
        var builtConfig = config.Build();
        var miCredentials = builtConfig[EnvironmentConfigurationConstants.ManagedIdentityClientIdConstant];

        var credentials = context.HostingEnvironment.IsDevelopment()
            ? new DefaultAzureCredential()
            : new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                ManagedIdentityClientId = miCredentials,
            });

        var appConfigurationEndpoint = builtConfig[EnvironmentConfigurationConstants.AppConfigurationEndpointKeyConstant];
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
            .AddScoped<IMongoDatabaseRepository, MongoDatabaseRepository>()
            .AddScoped<INotificationsDataManager, NotificationsDataManager>();

    /// <summary>
    /// Configures the MongoDB server connection settings and registers the MongoDB client in the service collection for dependency injection. 
    /// It retrieves the MongoDB connection string from the configuration, sets up the MongoClientSettings with TLS and SSL settings, and adds the IMongoClient as a singleton service to the IServiceCollection, allowing it to be injected and used throughout the application for database operations.
    /// </summary>
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
}
