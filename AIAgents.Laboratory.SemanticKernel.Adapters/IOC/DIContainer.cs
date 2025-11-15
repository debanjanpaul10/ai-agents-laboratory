using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.SemanticKernel.Adapters.AIServices;
using AIAgents.Laboratory.SemanticKernel.Adapters.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.Memory;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.IOC;

/// <summary>
/// The DI Container Class.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DIContainer
{
#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0050

	/// <summary>
	/// Adds the ai dependencies.
	/// </summary>
	/// <param name="services">The services.</param>
	/// <param name="configuration">The configuration.</param>
	/// <returns>The service collection.</returns>
	public static IServiceCollection AddAIAgentDependencies(this IServiceCollection services, IConfiguration configuration)
	{
		// Register Kernel first (factory) so other services that rely on Kernel can resolve it.
		services.AddSingleton(KernelFactory.CreateKernel(configuration));

		// Register memory store and other AI-related services.
		services.AddSingleton<IMemoryStore, VolatileMemoryStore>()
			.AddSingleton<ChatbotPlugins>().AddScoped<IAiServices, AiServices>();

		return services;
	}

}
