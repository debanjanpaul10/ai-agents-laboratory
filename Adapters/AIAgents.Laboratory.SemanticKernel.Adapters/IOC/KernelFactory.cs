// *********************************************************************************
//	<copyright file="KernelFactory.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Kernel factory.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.SemanticKernel.Adapters.Plugins.FitGymTool;
using AIAgents.Laboratory.SemanticKernel.Adapters.Plugins.IBBS;
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
    /// <summary>
    /// Creates memory.
    /// </summary>
    public static Func<IServiceProvider, ISemanticTextMemory> CreateMemory()
    {
        return provider =>
        {
            var memoryBuilder = new MemoryBuilder();
            memoryBuilder.WithMemoryStore(new VolatileMemoryStore());

            return memoryBuilder.Build();
        };
    }

    /// <summary>
    /// Creates the kernel.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public static Func<IServiceProvider, Kernel> CreateKernel(IConfiguration configuration)
    {
        var isProModelEnabled = bool.TryParse(configuration[AzureAppConfigurationConstants.IsProModelEnabledFlag], out bool parsedValue) && parsedValue;
        var geminiAiModel = isProModelEnabled ? AzureAppConfigurationConstants.GeminiProModel : AzureAppConfigurationConstants.GeminiFlashModel;

        return provider =>
        {
            var modelId = configuration[geminiAiModel];
            var apiKey = configuration[AzureAppConfigurationConstants.GeminiAPIKeyConstant];
            if (string.IsNullOrEmpty(modelId) || string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException(ExceptionConstants.AiAPIKeyMissingMessage);
            }

            var kernelBuilder = Kernel.CreateBuilder();

            if (!string.IsNullOrEmpty(modelId) && !string.IsNullOrEmpty(apiKey))
            {

                kernelBuilder.AddGoogleAIGeminiChatCompletion(modelId, apiKey);
                kernelBuilder.AddGoogleAIEmbeddingGenerator(modelId, apiKey);
                kernelBuilder.Services.AddSingleton(CreateMemory());
            }
            var kernel = kernelBuilder.Build();

            // Import Plugins
            kernel.Plugins.AddFromType<RewriteTextPlugin>(PluginHelpers.RewriteTextPlugin.PluginName);
            kernel.Plugins.AddFromType<ContentPlugins>(PluginHelpers.ContentPlugins.PluginName);
            kernel.Plugins.AddFromType<UtilityPlugins>(PluginHelpers.UtilityPlugins.PluginName);
            kernel.Plugins.AddFromType<ChatbotPlugins>(ChatbotPluginHelpers.PluginName, provider);

            return kernel;
        };
    }

}
