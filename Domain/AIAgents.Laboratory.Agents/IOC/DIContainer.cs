// *********************************************************************************
//	<copyright file="DIContainer.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The DI Container Class.</summary>
// *********************************************************************************

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AIAgents.Laboratory.Agents.IOC;

/// <summary>
/// The DI Container Class.
/// </summary>
public static class DIContainer
{
	/// <summary>
	/// Configures the semantic kernel.
	/// </summary>
	/// <param name="services">The services.</param>
	/// <param name="configuration">The configuration.</param>
	public static void ConfigureSemanticKernel(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton(KernelFactory.CreateKernel(configuration));
		services.AddSingleton(KernelFactory.CreateMemory());
	}
}
