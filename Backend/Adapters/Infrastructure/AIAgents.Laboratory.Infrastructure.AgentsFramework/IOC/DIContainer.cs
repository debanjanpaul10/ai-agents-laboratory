using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.AgentServices;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.IOC;

/// <summary>
/// The DI Container Class for Agents Framework dependencies.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DIContainer
{
    /// <summary>
    /// Adds the agents framework dependencies (alternative method name for backward compatibility).
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddAgentsFrameworkDependencies(this IServiceCollection services, IConfiguration configuration) =>
        services.AddSingleton(serviceProvider => AgentsFactory.CreateAgentConfigurationFromAppConfig(configuration, serviceProvider))
            .AddSingleton(serviceProvider => serviceProvider.GetRequiredService<AgentConfiguration>().CreateChatClientFromConfiguration())
            .AddScoped<IMcpClientServices, McpAgentServices>().AddScoped<IAiServices, AgentFrameworkServices>();
}
