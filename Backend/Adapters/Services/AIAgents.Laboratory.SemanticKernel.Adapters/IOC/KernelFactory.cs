using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.SemanticKernel.Adapters.InvocationFilter;
using AIAgents.Laboratory.SemanticKernel.Adapters.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.IOC;

/// <summary>
/// The Kernel Factory Class.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class KernelFactory
{
    /// <summary>
    /// Creates the kernel.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service provider and kernel.</returns>
    internal static Func<IServiceProvider, Kernel> CreateKernel(IConfiguration configuration) => (provider) =>
    {
        var currentAiServiceProvider = configuration[AzureAppConfigurationConstants.CurrentAiServiceProvider] ?? throw new Exception();
        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.Services.AddLogging();
        kernelBuilder.Services.AddSingleton<IFunctionInvocationFilter, FunctionInvocationFilter>();

        switch (currentAiServiceProvider)
        {
            case GoogleGeminiAiConstants.ServiceProviderName:
                var isProModelEnabled = bool.TryParse(configuration[GoogleGeminiAiConstants.IsProModelEnabledFlag], out bool parsedValue) && parsedValue;
                var geminiAiModel = isProModelEnabled ? GoogleGeminiAiConstants.GeminiProModel : GoogleGeminiAiConstants.GeminiFlashModel;
                var geminiModelId = configuration[geminiAiModel];
                var geminiApiKey = configuration[GoogleGeminiAiConstants.GeminiAPIKeyConstant];
                if (string.IsNullOrEmpty(geminiModelId) || string.IsNullOrEmpty(geminiApiKey))
                    throw new InvalidOperationException(ExceptionConstants.AiAPIKeyMissingMessage);

                kernelBuilder.AddGoogleAIGeminiChatCompletion(geminiModelId, geminiApiKey);
                break;
            case PerplexityAiConstants.ServiceProviderName:
                var perplexityModelId = configuration[PerplexityAiConstants.ModelId];
                var perplexityApiKey = configuration[PerplexityAiConstants.ApiKey];
                var perplexityApiEndpoint = configuration[PerplexityAiConstants.ApiEndpoint];
                if (string.IsNullOrEmpty(perplexityModelId) || string.IsNullOrEmpty(perplexityApiKey) || string.IsNullOrEmpty(perplexityApiEndpoint))
                    throw new InvalidOperationException(ExceptionConstants.AiAPIKeyMissingMessage);

                kernelBuilder.AddOpenAIChatCompletion(perplexityModelId, apiKey: perplexityApiKey, endpoint: new Uri(perplexityApiEndpoint));
                break;

            default:
                throw new InvalidOperationException(string.Format(ExceptionConstants.InvalidServiceProvider, currentAiServiceProvider));
        }

        var kernel = kernelBuilder.Build();

        kernel.Plugins.AddFromType<ApplicationPlugins>(ApplicationPluginsHelpers.PluginName, provider);
        kernel.Plugins.AddFromType<ChatbotPlugins>(ChatbotPluginHelpers.PluginName, provider);
        return kernel;
    };
}
