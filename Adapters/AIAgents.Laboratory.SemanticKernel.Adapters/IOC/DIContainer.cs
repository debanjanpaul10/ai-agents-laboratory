// *********************************************************************************
//	<copyright file="DIContainer.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The DI Container Class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.SemanticKernel.Adapters.AIServices;
using AIAgents.Laboratory.SemanticKernel.Adapters.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.IOC;

/// <summary>
/// The DI Container Class.
/// </summary>
public static class DIContainer
{
    /// <summary>
    /// Adds the ai dependencies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddAIAgentDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddScoped<IAIAgentServices, AgentServices>()
            .AddSingleton<ChatbotPlugins>()
            .AddSingleton(KernelFactory.CreateKernel(configuration))
            .AddSingleton(KernelFactory.CreateMemory());
    }
}
