using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Persistence.MongoDatabase.Contracts;
using AIAgents.Laboratory.Persistence.MongoDatabase.DataManager;
using AIAgents.Laboratory.Persistence.MongoDatabase.Mapper;
using AIAgents.Laboratory.Persistence.MongoDatabase.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using static AIAgents.Laboratory.Persistence.MongoDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.MongoDatabase.IOC;

/// <summary>
/// The Dependency Injection Container class.
/// </summary>
public static class DIContainer
{
    /// <summary>
    /// Adds the mongo database adapter dependencies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddMongoDbAdapterDependencies(this IServiceCollection services, IConfiguration configuration) =>
        services.ConfigureMongoDbServer(configuration)
        .AddDataManagers()
        .AddDataRepositories()
        .AddAutoMapper(config => config.AddProfile<MongoDataMapperProfile>());

    /// <summary>
    /// Configures the mongo database server.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection.</returns>
    private static IServiceCollection ConfigureMongoDbServer(this IServiceCollection services, IConfiguration configuration)
    {
        try
        {
            var mongoDbConnectionString = configuration[ConfigurationConstants.AiAgentsLabMongoConnectionString];
            ArgumentException.ThrowIfNullOrWhiteSpace(mongoDbConnectionString);

            var settings = MongoClientSettings.FromConnectionString(mongoDbConnectionString);
            settings.UseTls = true;
            settings.SslSettings = new SslSettings()
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13,
                CheckCertificateRevocation = false
            };

            services.AddSingleton<IMongoClient>(new MongoClient(settings));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(string.Format(ExceptionConstants.FailedToConfigureMongoDbClientExceptionMessage, ex.Message));
        }

        return services;
    }

    /// <summary>
    /// Adds the data managers.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddDataManagers(this IServiceCollection services) =>
        services.AddScoped<IAgentsDataManager, AgentsDataManager>()
        .AddScoped<IConversationHistoryDataManager, ConversationHistoryDataManager>()
        .AddScoped<IRegisteredApplicationDataManager, RegisteredApplicationDataManager>()
        .AddScoped<IWorkspacesDataManager, WorkspacesDataManager>()
        .AddScoped<IToolSkillsDataManager, ToolSkillsDataManager>()
        .AddScoped<INotificationsDataManager, NotificationsDataManager>();

    /// <summary>
    /// Adds the data repositories.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddDataRepositories(this IServiceCollection services) =>
        services.AddScoped<IMongoDatabaseRepository, MongoDatabaseRepository>();
}
