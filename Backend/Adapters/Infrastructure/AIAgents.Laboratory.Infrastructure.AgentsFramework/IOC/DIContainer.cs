using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.AgentServices;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.AgentServices.FileReaders;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.Contracts;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.Memory;
using static AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers.Constants;

namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.IOC;

/// <summary>
/// The DI Container Class for Agents Framework dependencies.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DIContainer
{
#pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    /// <summary>
    /// Adds the agents framework dependencies (alternative method name for backward compatibility).
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddAgentsFrameworkDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(serviceProvider => AgentsFactory.CreateAgentConfigurationFromAppConfig(configuration, serviceProvider))
            .AddSingleton(serviceProvider => serviceProvider.GetRequiredService<AgentConfiguration>().CreateChatClientFromConfiguration())
            .AddScoped<IMcpClientServices, McpAgentServices>().AddScoped<IAiServices, AgentFrameworkServices>();

        services.RegisterTextEmbeddingGenerationService(configuration);
        services.AddScoped<IFileContentReader, TextFileContentReader>()
            .AddScoped<IFileContentReader, PdfFileContentReader>()
            .AddScoped<IFileContentReader, SpreadsheetFileContentReader>()
            .AddScoped<IFileContentReader, WordFileContentReader>()
            .AddScoped<FileContentReaderFactory>();

        services.AddScoped<IKnowledgeBaseProcessor, KnowledgeBaseProcessor>()
            .AddScoped<IVisionProcessor, VisionProcessor>()
            .AddSingleton<IMemoryStore, VolatileMemoryStore>();

        return services;
    }

    /// <summary>
    /// Registers the text embedding generation service.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    private static void RegisterTextEmbeddingGenerationService(this IServiceCollection services, IConfiguration configuration)
    {
        var openaiModelId = configuration[OpenAiGptConstants.ModelId];
        var openAiApiKey = configuration[OpenAiGptConstants.ApiKey];
        var openAiApiEndpoint = configuration[OpenAiGptConstants.ApiEndpoint];
        if (string.IsNullOrEmpty(openaiModelId) || string.IsNullOrEmpty(openAiApiKey) || string.IsNullOrEmpty(openAiApiEndpoint))
            throw new InvalidOperationException(ExceptionConstants.AiAPIKeyMissingMessage);

        services.AddAzureOpenAIEmbeddingGenerator(openaiModelId, openAiApiEndpoint, openAiApiKey);
    }
}
