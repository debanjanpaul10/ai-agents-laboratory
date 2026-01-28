using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Messaging.Adapters.Services;
using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static AIAgents.Laboratory.Messaging.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.Messaging.Adapters.IOC;

/// <summary>
/// The Dependency Injection Container.
/// </summary>
public static class DIContainer
{
    /// <summary>
    /// Adds the messaging dependencies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The services collection.</returns>
    public static IServiceCollection AddMessagingDependencies(this IServiceCollection services, IConfiguration configuration) =>
        services.AddHostedService<AgentStatusWatcher>().AddSingleton<IAgentStatusStore, AgentStatusStore>()
            .AddEmailNotificationService(configuration);

    /// <summary>
    /// Adds the email notification service.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration service.</param>
    /// <returns>The service collection.</returns>
    /// <exception cref="Exception"></exception>
    internal static IServiceCollection AddEmailNotificationService(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration[AzureAppConfigurationConstants.EmailNotificationServiceConnectionString]
            ?? throw new Exception(ExceptionMessagesConstants.ConfigurationMissingExceptionMessage);

        return services.AddSingleton(new EmailClient(connectionString))
            .AddScoped<IEmailNotificationService, EmailNotificationService>();
    }
}
