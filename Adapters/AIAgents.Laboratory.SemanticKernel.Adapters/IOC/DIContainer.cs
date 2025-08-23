// *********************************************************************************
//	<copyright file="DIContainer.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The DI Container Class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.SemanticKernel.Adapters.AIServices;
using AIAgents.Laboratory.SemanticKernel.Adapters.Plugins.FitGymTool;
using AIAgents.Laboratory.SemanticKernel.Adapters.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

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
            .AddSingleton<IHttpClientHelper, HttpClientHelper>()
            .AddSingleton<ChatbotPlugins>()
            .AddSingleton(KernelFactory.CreateKernel(configuration))
            .AddSingleton(KernelFactory.CreateMemory())
            .ConfigureHttpClientFactory(configuration);
    }

    /// <summary>
    /// Configures the HTTP client factory.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    private static IServiceCollection ConfigureHttpClientFactory(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient(ConfigurationConstants.FGToolHttpClient, client =>
        {
            var apiBaseAddress = bool.TryParse(configuration[ConfigurationConstants.IsDevelopmentModeConstant], out var isDevelopmentMode) && isDevelopmentMode
                ? configuration[ConfigurationConstants.LocalFGToolBaseUrl] : configuration[ConfigurationConstants.FitGymToolApiBaseUrl];
            if (string.IsNullOrEmpty(apiBaseAddress))
            {
                throw new ArgumentNullException(apiBaseAddress);
            }

            client.BaseAddress = new Uri(apiBaseAddress);
            client.Timeout = TimeSpan.FromMinutes(3);
        });

        return services;
    }
}
