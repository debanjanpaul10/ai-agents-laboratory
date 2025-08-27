// *********************************************************************************
//	<copyright file="DIContainer.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The DI Container Class.</summary>
// *********************************************************************************

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
	public static IServiceCollection AddDomainDependencies(this IServiceCollection services)
	{
		return services.AddScoped<IPluginsAiServices, PluginsAiServices>()
			.AddScoped<ICommonAiService, CommonAiService>()
			.AddScoped<IAiSkillsService, AiSkillsService>();
	}
}
