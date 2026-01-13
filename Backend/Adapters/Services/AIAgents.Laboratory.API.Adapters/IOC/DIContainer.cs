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
    /// Adds the API adapter dependencies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddAPIAdapterDependencies(this IServiceCollection services) =>
        services.AddScoped<ICommonAiHandler, CommonAiHandler>()
            .AddScoped<IChatbotSkillsHandler, ChatbotSkillsHandler>()
            .AddScoped<IAgentsHandler, AgentsHandler>()
            .AddScoped<IChatHandler, ChatHandler>()
            .AddScoped<IFeedbackHandler, FeedbackHandler>()
            .AddScoped<IToolSkillsHandler, ToolSkillsHandler>()
            .AddScoped<IWorkspacesHandler, WorkspacesHandler>()
            .AddAutoMapper(config => config.AddProfile<DomainMapperProfile>());
}
