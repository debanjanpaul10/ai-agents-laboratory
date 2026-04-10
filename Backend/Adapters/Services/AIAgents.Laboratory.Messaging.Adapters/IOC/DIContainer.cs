using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Messaging.Adapters.Contracts;
using AIAgents.Laboratory.Messaging.Adapters.Services;
using Azure.Communication.Email;
using Azure.Messaging.ServiceBus;
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
    public static IServiceCollection AddMessagingDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var emailNotificationConnectionString = configuration[AzureAppConfigurationConstants.EmailNotificationServiceConnectionString];
        ArgumentException.ThrowIfNullOrWhiteSpace(emailNotificationConnectionString);

        var serviceBusConnectionString = configuration[AzureAppConfigurationConstants.ServiceBusConnectionString];
        ArgumentException.ThrowIfNullOrWhiteSpace(serviceBusConnectionString);

        return services.AddSingleton(new EmailClient(emailNotificationConnectionString))
            .AddSingleton(new ServiceBusClient(serviceBusConnectionString))
            .AddScoped<IServiceBusManager, ServiceBusManager>()
            .AddScoped<IEmailNotificationService, EmailNotificationService>()
            .AddScoped<IApplicationNotificationsService, AppPushNotificationService>();
    }

}
