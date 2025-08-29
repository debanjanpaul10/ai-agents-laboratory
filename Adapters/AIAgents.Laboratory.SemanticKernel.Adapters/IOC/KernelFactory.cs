// *********************************************************************************
//	<copyright file="KernelFactory.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Kernel factory.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.SemanticKernel.Adapters.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Memory;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.IOC;

/// <summary>
/// The Kernel Factory Class.
/// </summary>
public static class KernelFactory
{
#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0050
#pragma warning disable SKEXP0010

    /// <summary>
    /// Creates the memory.
    /// </summary>
    /// <returns>The service provider and kernel memory</returns>
    public static Func<IServiceProvider, ISemanticTextMemory> CreateMemory() => (provider) =>
    {
        var memoryBuilder = new MemoryBuilder();
        memoryBuilder.WithMemoryStore(new VolatileMemoryStore());

        return memoryBuilder.Build();
    };

    /// <summary>
    /// Creates the kernel.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service provider and kernel.</returns>
    public static Func<IServiceProvider, Kernel> CreateKernel(IConfiguration configuration) => (provider) =>
    {
        var currentAiServiceProvider = configuration[AzureAppConfigurationConstants.CurrentAiServiceProvider] ?? throw new Exception();
        var kernelBuilder = Kernel.CreateBuilder();

        switch (currentAiServiceProvider)
        {
            case GoogleGeminiAiConstants.ServiceProviderName:
                kernelBuilder.ConfigureGoogleGeminiAiKernel(configuration);
                break;
            case PerplexityAiConstants.ServiceProviderName:
                kernelBuilder.ConfigurePerplexityAiKernel(configuration);
                break;
            default:
                throw new InvalidOperationException(string.Format(ExceptionConstants.InvalidServiceProvider, currentAiServiceProvider));
        }

        var kernel = kernelBuilder.Build();

        kernel.Plugins.AddFromType<RewriteTextPlugin>(PluginHelpers.RewriteTextPlugin.PluginName);
        kernel.Plugins.AddFromType<ContentPlugins>(PluginHelpers.ContentPlugins.PluginName);
        kernel.Plugins.AddFromType<UtilityPlugins>(PluginHelpers.UtilityPlugins.PluginName);
        kernel.Plugins.AddFromType<ChatbotPlugins>(ChatbotPluginHelpers.PluginName, provider);
        return kernel;
    };

    /// <summary>
    /// Configures the google gemini ai kernel.
    /// </summary>
    /// <param name="kernelBuilder">The kernel builder.</param>
    /// <param name="configuration">The configuration.</param>
    /// <exception cref="System.InvalidOperationException"></exception>
    private static void ConfigureGoogleGeminiAiKernel(this IKernelBuilder kernelBuilder, IConfiguration configuration)
    {
        var isProModelEnabled = bool.TryParse(configuration[GoogleGeminiAiConstants.IsProModelEnabledFlag], out bool parsedValue) && parsedValue;
        var geminiAiModel = isProModelEnabled ? GoogleGeminiAiConstants.GeminiProModel : GoogleGeminiAiConstants.GeminiFlashModel;
        var modelId = configuration[geminiAiModel];
        var apiKey = configuration[GoogleGeminiAiConstants.GeminiAPIKeyConstant];
        if (string.IsNullOrEmpty(modelId) || string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException(ExceptionConstants.AiAPIKeyMissingMessage);
        }

        kernelBuilder.AddGoogleAIGeminiChatCompletion(modelId, apiKey);
        kernelBuilder.AddGoogleAIEmbeddingGenerator(modelId, apiKey);
        kernelBuilder.Services.AddSingleton(CreateMemory());
    }

    /// <summary>
    /// Configures the perplexity ai kernel.
    /// </summary>
    /// <param name="kernelBuilder">The kernel builder.</param>
    /// <param name="configuration">The configuration.</param>
    /// <exception cref="System.InvalidOperationException"></exception>
    private static void ConfigurePerplexityAiKernel(this IKernelBuilder kernelBuilder, IConfiguration configuration)
    {
        var aiModelId = configuration[PerplexityAiConstants.ModelId];
        var aiApiKey = configuration[PerplexityAiConstants.ApiKey];
        var apiEndpoint = configuration[PerplexityAiConstants.ApiEndpoint];
        if (string.IsNullOrEmpty(aiModelId) || string.IsNullOrEmpty(aiApiKey) || string.IsNullOrEmpty(apiEndpoint))
        {
            throw new InvalidOperationException(ExceptionConstants.AiAPIKeyMissingMessage);
        }

        kernelBuilder.AddOpenAIChatCompletion(modelId: aiModelId, apiKey: aiApiKey, endpoint: new Uri(apiEndpoint));
        kernelBuilder.AddOpenAIEmbeddingGenerator(modelId: aiModelId, apiKey: aiApiKey);
        kernelBuilder.Services.AddSingleton(CreateMemory());
    }
}
