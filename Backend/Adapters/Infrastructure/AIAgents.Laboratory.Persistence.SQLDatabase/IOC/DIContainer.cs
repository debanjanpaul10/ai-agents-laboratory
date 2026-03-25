using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Persistence.SQLDatabase.Context;
using AIAgents.Laboratory.Persistence.SQLDatabase.DataManagers;
using AIAgents.Laboratory.Persistence.SQLDatabase.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        var sqlConnectionString = isDevelopmentMode ? configuration[ConfigurationConstants.LocalSqlConnectionStringConstant] : configuration[ConfigurationConstants.AzureSqlConnectionStringConstant];
        ArgumentException.ThrowIfNullOrWhiteSpace(sqlConnectionString);

        services.AddDbContext<SqlDbContext>(options =>
            options.UseSqlServer(
                connectionString: sqlConnectionString,
                sqlServerOptionsAction: options => options.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null))
        );

        services.AddDataManagerDependencies();
        return services;
    }

    /// <summary>
    /// Adds the data managers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The service collection.</returns>
    private static IServiceCollection AddDataManagerDependencies(this IServiceCollection services) =>
        services.AddScoped<IUnitOfWork, UnitOfWork>()
        .AddScoped<IFeedbackDataManager, FeedbackDataManager>()
        .AddAutoMapper(config => config.AddProfile<DataMapperProfile>());
}
