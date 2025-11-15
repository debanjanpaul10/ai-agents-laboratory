using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Messaging.Adapters.Services;
using Microsoft.Extensions.DependencyInjection;

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
    public static IServiceCollection AddMessagingDependencies(this IServiceCollection services) =>
        services.AddHostedService<AgentStatusWatcher>().AddSingleton<IAgentStatusStore, AgentStatusStore>();
}
