using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Persistence.SQLDatabase.Context;
using AIAgents.Laboratory.Persistence.SQLDatabase.DataManagers;
using AIAgents.Laboratory.Persistence.SQLDatabase.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using static AIAgents.Laboratory.Persistence.SQLDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.IOC;

/// <summary>
/// The Dependency Injection Container class.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DIContainer
{
    /// <summary>
    /// Adds the Relational SQL dependencies.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="isDevelopmentMode">The is development mode flag.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddRelationalSqlDependencies(this IServiceCollection services, IConfiguration configuration, bool isDevelopmentMode)
    {
        var currentSqlServiceProvider = configuration[ConfigurationConstants.CurrentSQLProviderConstant] ?? throw new KeyNotFoundException(ErrorMessages.DatabaseConnectionNotFound);
        switch (currentSqlServiceProvider)
        {
            case DatabaseConstants.AzureSQLConstant:
                services.ConfigureAzureSqlDatabase(configuration, isDevelopmentMode);
                break;
            case DatabaseConstants.PostgreSQLConstant:
                services.ConfigurePostgreSqlDatabase(configuration);
                break;
        }

        return services.AddDataManagerDependencies();
    }

    /// <summary>
    /// Configures the Postgre SQL database.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection.</returns>
    private static IServiceCollection ConfigurePostgreSqlDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlConnectionString = configuration[ConfigurationConstants.PostgreSQLConnectionStringConstant];
        ArgumentException.ThrowIfNullOrWhiteSpace(sqlConnectionString);

        AppContext.SetSwitch("System.Net.DisableIPv6", true);

        services.AddHealthChecks().AddNpgSql(
            connectionString: sqlConnectionString,
            healthQuery: HealthCheckConstants.DBHealthQuery,
            name: DatabaseConstants.PostgreSQLConstant,
            failureStatus: HealthStatus.Unhealthy,
            tags: [HealthCheckConstants.ApplicationName, HealthCheckConstants.PostgreSQLHealthCheckTag]);

        return services.AddDbContext<SqlDbContext>(options =>
            options.UseNpgsql(
                connectionString: sqlConnectionString,
                npgsqlOptionsAction: options => options.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null)));
    }

    /// <summary>
    /// Configures the Azure SQL database.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="isDevelopmentMode">The is development mode flag.</param>
    /// <returns>The service collection.</returns>
    private static IServiceCollection ConfigureAzureSqlDatabase(this IServiceCollection services, IConfiguration configuration, bool isDevelopmentMode)
    {
        var sqlConnectionString = isDevelopmentMode ? configuration[ConfigurationConstants.LocalSqlConnectionStringConstant] : configuration[ConfigurationConstants.AzureSqlConnectionStringConstant];
        ArgumentException.ThrowIfNullOrWhiteSpace(sqlConnectionString);

        services.AddHealthChecks().AddSqlServer(
            connectionString: sqlConnectionString,
            healthQuery: HealthCheckConstants.DBHealthQuery,
            name: DatabaseConstants.AzureSQLConstant,
            failureStatus: HealthStatus.Unhealthy,
            tags: [HealthCheckConstants.ApplicationName, HealthCheckConstants.AzureSQLHealthCheckTag]);

        return services.AddDbContext<SqlDbContext>(options =>
            options.UseSqlServer(
                connectionString: sqlConnectionString,
                sqlServerOptionsAction: options => options.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null))
        );
    }

    /// <summary>
    /// Adds the data managers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The service collection.</returns>
    private static IServiceCollection AddDataManagerDependencies(this IServiceCollection services) =>
        services.AddScoped<IUnitOfWork, UnitOfWork>()
        .AddScoped<IFeedbackDataManager, FeedbackDataManager>()
        .AddScoped<IRegisteredApplicationDataManager, RegisteredApplicationDataManager>()
        .AddAutoMapper(config => config.AddProfile<DataMapperProfile>());
}
