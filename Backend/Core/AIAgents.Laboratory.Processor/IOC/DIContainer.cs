using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Processor.Contracts;
using AIAgents.Laboratory.Processor.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.Memory;
using static AIAgents.Laboratory.Processor.Helpers.ProcessorConstants;

namespace AIAgents.Laboratory.Processor.IOC;

/// <summary>
/// The DI Container Class.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DIContainer
{
#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0001 
#pragma warning disable SKEXP0050 

    /// <summary>
    /// Adds the processor dependencies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The services collection.</returns>
    public static IServiceCollection AddProcessorDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterTextEmbeddingGenerationService(configuration);
        return services.AddScoped<IKnowledgeBaseProcessor, KnowledgeBaseProcessor>()
            .AddScoped<IVisionProcessor, VisionProcessor>()
            .AddSingleton<IMemoryStore, VolatileMemoryStore>();
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
