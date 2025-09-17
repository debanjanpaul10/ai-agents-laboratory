using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Handlers;
using AIAgents.Laboratory.API.Adapters.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace AIAgents.Laboratory.API.Adapters.IOC;

/// <summary>
/// The Dependency Injection Container Class.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DIContainer
{
	/// <summary>
	/// Adds the API handlers.
	/// </summary>
	/// <param name="services">The services.</param>
	/// <returns>The service collection.</returns>
	public static IServiceCollection AddAPIHandlers(this IServiceCollection services) =>
		services.AddScoped<IPluginsHandler, PluginsHandler>()
			.AddScoped<ICommonAiHandler, CommonAiHandler>()
			.AddScoped<ISkillsHandler, SkillsHandler>()
			.AddScoped<IAgentSkillsHandler, AgentSkillsHandler>()
			.AddAutoMapper(config =>
			{
				config.AddProfile<DomainMapperProfile>();
			});
}
