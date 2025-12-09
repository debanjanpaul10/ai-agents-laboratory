using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Persistence.SQLDatabase.Context;
using AIAgents.Laboratory.Persistence.SQLDatabase.Contracts;
using AIAgents.Laboratory.Persistence.SQLDatabase.DataManagers;
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
    /// Adds the SQL server dependencies.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="isDevelopmentMode">The is development mode flag.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddSqlServerDependencies(this IServiceCollection services, IConfiguration configuration, bool isDevelopmentMode) =>
        services.ConfigureSqlDatabase(configuration, isDevelopmentMode).AddDataManagerDependencies();

    /// <summary>
    /// Configures the SQL database.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="isDevelopmentMode">The is development mode flag.</param>
    /// <returns>The service collection.</returns>
    private static IServiceCollection ConfigureSqlDatabase(this IServiceCollection services, IConfiguration configuration, bool isDevelopmentMode)
    {
        var sqlConnectionString = isDevelopmentMode
            ? configuration[ConfigurationConstants.LocalSqlConnectionStringConstant]
            : configuration[ConfigurationConstants.AzureSqlConnectionStringConstant];

        if (string.IsNullOrWhiteSpace(sqlConnectionString))
            throw new ArgumentNullException(nameof(sqlConnectionString), ErrorMessages.DatabaseConnectionNotFound);

        return services.AddDbContext<SqlDbContext>(options =>
        {
            options.UseSqlServer(
                connectionString: sqlConnectionString,
                options => options.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null));
        });
    }

    /// <summary>
    /// Adds the data managers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The service collection.</returns>
    private static IServiceCollection AddDataManagerDependencies(this IServiceCollection services) =>
        services.AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IFeedbackDataManager, FeedbackDataManager>();

}
