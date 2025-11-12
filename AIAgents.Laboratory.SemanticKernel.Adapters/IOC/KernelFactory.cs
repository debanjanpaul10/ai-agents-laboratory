using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.SemanticKernel.Adapters.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using static AIAgents.Laboratory.SemanticKernel.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.SemanticKernel.Adapters.IOC;

/// <summary>
/// The Kernel Factory Class.
/// </summary>
[ExcludeFromCodeCoverage]
public static class KernelFactory
{
#pragma warning disable SKEXP0010

	/// <summary>
	/// Creates the kernel.
	/// </summary>
	/// <param name="configuration">The configuration.</param>
	/// <returns>The service provider and kernel.</returns>
	internal static Func<IServiceProvider, Kernel> CreateKernel(IConfiguration configuration) => (provider) =>
	{
		var currentAiServiceProvider = configuration[AzureAppConfigurationConstants.CurrentAiServiceProvider] ?? throw new Exception();
		var kernelBuilder = Kernel.CreateBuilder();

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
				var aiModelId = configuration[PerplexityAiConstants.ModelId];
				var aiApiKey = configuration[PerplexityAiConstants.ApiKey];
				var aiApiEndpoint = configuration[PerplexityAiConstants.ApiEndpoint];
				if (string.IsNullOrEmpty(aiModelId) || string.IsNullOrEmpty(aiApiKey) || string.IsNullOrEmpty(aiApiEndpoint))
					throw new InvalidOperationException(ExceptionConstants.AiAPIKeyMissingMessage);

				kernelBuilder.AddOpenAIChatCompletion(modelId: aiModelId, apiKey: aiApiKey, endpoint: new Uri(aiApiEndpoint));
				break;
			default:
				throw new InvalidOperationException(string.Format(ExceptionConstants.InvalidServiceProvider, currentAiServiceProvider));
		}

		var kernel = kernelBuilder.Build();

		kernel.Plugins.AddFromType<ApplicationPlugins>(ApplicationPluginsHelpers.PluginName, provider);
		kernel.Plugins.AddFromType<ChatbotPlugins>(ChatbotPluginHelpers.PluginName, provider);
		return kernel;
	};

	/// <summary>
	/// Registers the text embedding generation service.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <param name="configuration">The configuration.</param>
	internal static void RegisterTextEmbeddingGenerationService(IServiceCollection services, IConfiguration configuration)
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

				services.AddOpenAIEmbeddingGenerator(modelId: perplexityModelId, apiKey: perplexityApiKey);
				break;

			default:
				throw new InvalidOperationException(string.Format(ExceptionConstants.InvalidServiceProvider, currentAiServiceProvider));
		}
	}
}
