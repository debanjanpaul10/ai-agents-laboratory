using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Persistence.Caching.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AIAgents.Laboratory.Persistence.Caching.IOC;

/// <summary>
/// The Dependency Injection Container.
/// </summary>
public static class DIContainer
{
    /// <summary>
    /// Adds the cache dependencies.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The services collection.</returns>
    public static IServiceCollection AddCacheDependencies(this IServiceCollection services) =>
        services.AddScoped<ICacheService, CacheService>();
}
