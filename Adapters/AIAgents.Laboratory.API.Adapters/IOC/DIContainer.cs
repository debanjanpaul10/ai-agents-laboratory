// *********************************************************************************
//	<copyright file="DependencyContainer.cs" company="Personal">
//		Copyright (c) 2025 <Debanjan's Lab>
//	</copyright>
// <summary>The DI Container Class.</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Handlers;
using AIAgents.Laboratory.API.Adapters.Mapper;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

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
	public static IServiceCollection AddAPIHandlers(this IServiceCollection services)
	{
		return services.AddScoped<IBulletinAiHandler, BulletinAiHandler>()
			.AddAutoMapper(config =>
			{
				config.AddProfile<DomainMapperProfile>();
			});
	}
}
