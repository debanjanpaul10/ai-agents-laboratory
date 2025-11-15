using System.ClientModel;
using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Processor.Contracts;
using AIAgents.Laboratory.Processor.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static AIAgents.Laboratory.Processor.Helpers.ProcessorConstants;

namespace AIAgents.Laboratory.Processor.IOC;

/// <summary>
/// The DI Container Class.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DIContainer
{
#pragma warning disable SKEXP0010

    /// <summary>
    /// Adds the processor dependencies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The services collection.</returns>
    public static IServiceCollection AddProcessorDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterTextEmbeddingGenerationService(services, configuration);
        return services.AddScoped<IKnowledgeBaseProcessor, KnowledgeBaseProcessor>();
    }

    /// <summary>
    /// Registers the text embedding generation service.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    private static void RegisterTextEmbeddingGenerationService(IServiceCollection services, IConfiguration configuration)
    {
        var currentAiServiceProvider = configuration[AzureAppConfigurationConstants.CurrentAiServiceProvider];
        if (string.IsNullOrEmpty(currentAiServiceProvider))
            throw new InvalidOperationException($"Configuration key '{AzureAppConfigurationConstants.CurrentAiServiceProvider}' is missing or empty. Please check your Azure App Configuration.");

        switch (currentAiServiceProvider)
        {
            case GoogleGeminiAiConstants.ServiceProviderName:
                var isProModelEnabled = bool.TryParse(configuration[GoogleGeminiAiConstants.IsProModelEnabledFlag], out bool parsedValue) && parsedValue;
                var geminiAiModel = isProModelEnabled ? GoogleGeminiAiConstants.GeminiProModel : GoogleGeminiAiConstants.GeminiFlashModel;
                var modelId = configuration[geminiAiModel];
                var apiKey = configuration[GoogleGeminiAiConstants.GeminiAPIKeyConstant];
                if (string.IsNullOrEmpty(modelId) || string.IsNullOrEmpty(apiKey))
                    throw new InvalidOperationException($"{ExceptionConstants.AiAPIKeyMissingMessage} Provider: {currentAiServiceProvider}, ModelId: {modelId ?? "null"}, ApiKey: {(string.IsNullOrEmpty(apiKey) ? "null" : "***")}");

                services.AddGoogleAIEmbeddingGenerator(modelId, apiKey);
                break;

            case PerplexityAiConstants.ServiceProviderName:
                var perplexityModelId = configuration[PerplexityAiConstants.ModelId];
                var perplexityApiKey = configuration[PerplexityAiConstants.ApiKey];
                var perplexityEndpoint = configuration[PerplexityAiConstants.ApiEndpoint];
                if (string.IsNullOrEmpty(perplexityModelId) || string.IsNullOrEmpty(perplexityApiKey) || string.IsNullOrEmpty(perplexityEndpoint))
                    throw new InvalidOperationException($"{ExceptionConstants.AiAPIKeyMissingMessage} Provider: {currentAiServiceProvider}, ModelId: {perplexityModelId ?? "null"}, ApiKey: {(string.IsNullOrEmpty(perplexityApiKey) ? "null" : "***")}");

                var openAIClient = new OpenAI.OpenAIClient(new ApiKeyCredential(perplexityApiKey), new OpenAI.OpenAIClientOptions
                {
                    Endpoint = new Uri(perplexityEndpoint)
                });
                services.AddSingleton(openAIClient);
                services.AddOpenAIEmbeddingGenerator(modelId: perplexityModelId);
                break;

            default:
                throw new InvalidOperationException(string.Format(ExceptionConstants.InvalidServiceProvider, currentAiServiceProvider));
        }
    }
}
