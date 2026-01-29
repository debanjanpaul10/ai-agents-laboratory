using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace AIAgents.Laboratory.Domain.IOC;

/// <summary>
/// The Dependency Injection Container Class.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DIContainer
{
    /// <summary>
    /// Adds the domain dependencies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddDomainDependencies(this IServiceCollection services) =>
        services.AddScoped<ICommonAiService, CommonAiService>()
            .AddScoped<IAgentsService, AgentsService>()
            .AddScoped<IAgentChatService, AgentChatService>()
            .AddScoped<IDirectChatService, DirectChatService>()
            .AddScoped<IConversationHistoryService, ConversationHistoryService>()
            .AddScoped<IFeedbackService, FeedbackService>()
            .AddScoped<IDocumentIntelligenceService, DocumentIntelligenceService>()
            .AddScoped<IToolSkillsService, ToolSkillsService>()
            .AddScoped<IWorkspacesService, WorkspacesService>()
            .AddScoped<IOrchestratorService, OrchestratorService>();
}
