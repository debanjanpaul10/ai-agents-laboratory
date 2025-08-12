// *********************************************************************************
//	<copyright file="KernelFactory.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Kernel factory.</summary>
// *********************************************************************************


using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Memory;
using static AIAgents.Laboratory.Shared.Constants.ConfigurationConstants;

namespace AIAgents.Laboratory.API.IOC;

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
	/// Creates kernel.
	/// </summary>
	/// <param name="configuration">The configuration.</param>
	public static Func<IServiceProvider, Kernel> CreateKernel(IConfiguration configuration)
	{
		return provider =>
		{
			var modelId = configuration[AzureAppConfigurationConstants.GeminiAiModelIdConstant];
			var apiKey = configuration[AzureAppConfigurationConstants.GeminiAPIKeyConstant];
			var kernelBuilder = Kernel.CreateBuilder();

			if (!string.IsNullOrEmpty(modelId) && !string.IsNullOrEmpty(apiKey))
			{

				kernelBuilder.AddGoogleAIGeminiChatCompletion(modelId, apiKey);
				kernelBuilder.AddGoogleAIEmbeddingGenerator(modelId, apiKey);
				kernelBuilder.Services.AddSingleton(CreateMemory());
			}
			var kernel = kernelBuilder.Build();

			// Import Plugins

			return kernel;
		};
	}

}
