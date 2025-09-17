using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace AIAgents.Laboratory.Domain.IOC;

/// <summary>
/// The Dependency Injection Container Class.
/// </summary>
public static class DIContainer
{
	/// <summary>
	/// Adds the domain dependencies.
	/// </summary>
	/// <param name="services">The services.</param>
	/// <returns>The service collection.</returns>
	public static IServiceCollection AddDomainDependencies(this IServiceCollection services) =>
		services.AddScoped<IPluginsAiService, PluginsAiService>()
			.AddScoped<ICommonAiService, CommonAiService>()
			.AddScoped<ISkillsService, SkillsService>()
			.AddScoped<IAgentSkillsService, AgentSkillsService>();
}
