// *********************************************************************************
//	<copyright file="DIContainer.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The DI Container Class.</summary>
// *********************************************************************************

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AIAgents.Laboratory.Agents.Adapters.IOC;

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
	public static IServiceCollection AddAIDependencies(this IServiceCollection services, IConfiguration configuration)
	{
		return services.AddSingleton(KernelFactory.CreateKernel(configuration))
			.AddSingleton(KernelFactory.CreateMemory());
	}
}
