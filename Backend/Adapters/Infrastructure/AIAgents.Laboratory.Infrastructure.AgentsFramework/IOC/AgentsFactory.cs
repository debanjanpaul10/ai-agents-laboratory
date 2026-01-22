using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Infrastructure.AgentsFramework.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using static AIAgents.Laboratory.Infrastructure.AgentsFramework.Helpers.Constants;

namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.IOC;

/// <summary>
/// The Agents Factory Class for creating and configuring AI agents.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class AgentsFactory
{
    /// <summary>
    /// Creates an agent configuration from application configuration settings.
    /// </summary>
    /// <param name="appConfiguration">The application configuration.</param>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    /// <returns>A configured agent configuration.</returns>
    /// <exception cref="InvalidOperationException">Thrown when required configuration is missing.</exception>
    internal static AgentConfiguration CreateAgentConfigurationFromAppConfig(IConfiguration appConfiguration, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(appConfiguration);

        var currentAiServiceProvider = appConfiguration[AzureAppConfigurationConstants.CurrentAiServiceProvider]
            ?? throw new InvalidOperationException(ExceptionConstants.CurrentAiServiceProviderMissingMessage);

        var agentConfiguration = BuildAgentConfigurationFromAppConfig(appConfiguration, currentAiServiceProvider);
        return CreateAgentConfiguration(agentConfiguration, serviceProvider);
    }

    /// <summary>
    /// Creates a chat client based on the agent configuration.
    /// </summary>
    /// <param name="config">The agent configuration.</param>
    /// <returns>A configured chat client.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the service provider is not supported.</exception>
    internal static IChatClient CreateChatClientFromConfiguration(this AgentConfiguration config) =>
        config.ServiceProvider switch
        {
            GoogleGeminiAiConstants.ServiceProviderName => CreateGoogleGeminiClient(config),
            PerplexityAiConstants.ServiceProviderName => CreatePerplexityAiClient(config),
            ChatGptAiConstants.ServiceProviderName => CreateAzureOpenAIClient(config),
            _ => throw new InvalidOperationException(string.Format(ExceptionConstants.InvalidServiceProvider, config.ServiceProvider))
        };

    #region PRIVATE METHODS

    /// <summary>
    /// Creates a Google Gemini chat client.
    /// </summary>
    /// <param name="config">The agent configuration.</param>
    /// <returns>A configured Google Gemini chat client.</returns>
    private static IChatClient CreateGoogleGeminiClient(AgentConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.ApiKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.ModelId);

        var geminiClient = new Google.GenAI.Client(apiKey: config.ApiKey);
        return geminiClient.AsIChatClient();
    }

    /// <summary>
    /// Creates a Perplexity AI chat client.
    /// </summary>
    /// <param name="config">The agent configuration.</param>
    /// <returns>A configured Perplexity AI chat client.</returns>
    private static IChatClient CreatePerplexityAiClient(AgentConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.ApiKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.ModelId);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.ApiEndpoint);

        var options = new OpenAIClientOptions
        {
            Endpoint = new Uri(config.ApiEndpoint)
        };
        var openAiClient = new OpenAIClient(new System.ClientModel.ApiKeyCredential(config.ApiKey), options);
        return openAiClient.GetChatClient(config.ModelId).AsIChatClient();
    }

    /// <summary>
    /// Creates an Azure OpenAI chat client.
    /// </summary>
    /// <param name="config">The agent configuration.</param>
    /// <returns>The chat client.</returns>
    private static IChatClient CreateAzureOpenAIClient(AgentConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.ApiKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.ModelId);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.ApiEndpoint);

        var azureCredential = new Azure.AzureKeyCredential(config.ApiKey);
        var client = new Azure.AI.OpenAI.AzureOpenAIClient(new Uri(config.ApiEndpoint), azureCredential);
        return client.GetChatClient(config.ModelId).AsIChatClient();
    }

    /// <summary>
    /// Creates an agent configuration based on the provided settings.
    /// </summary>
    /// <param name="configuration">The agent configuration.</param>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    /// <returns>A validated agent configuration.</returns>
    /// <exception cref="ArgumentNullException">Thrown when configuration is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when configuration is invalid or service provider is unsupported.</exception>
    private static AgentConfiguration CreateAgentConfiguration(AgentConfiguration configuration, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        // Validate configuration
        var validationErrors = configuration.Validate();
        if (validationErrors.Count != 0)
            throw new InvalidOperationException(string.Format(ExceptionConstants.InvalidAgentConfigurationExceptionMessage, string.Join(", ", validationErrors)));

        var currentAiServiceProvider = configuration.ServiceProvider;
        IEnumerable<string> availableAiServiceProviders = [GoogleGeminiAiConstants.ServiceProviderName, PerplexityAiConstants.ServiceProviderName, ChatGptAiConstants.ServiceProviderName];
        if (availableAiServiceProviders.Contains(currentAiServiceProvider, StringComparer.OrdinalIgnoreCase))
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(configuration.ApiKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(configuration.ModelId);

            if (!string.Equals(currentAiServiceProvider, GoogleGeminiAiConstants.ServiceProviderName, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(currentAiServiceProvider, ChatGptAiConstants.ServiceProviderName, StringComparison.OrdinalIgnoreCase))
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(configuration.ApiEndpoint);
            }

            // Ensure capabilities are set correctly for the service provider.
            configuration.Capabilities.SupportsVision = true;
            configuration.Capabilities.SupportsFunctionCalling = true;

            if (!configuration.Capabilities.SupportedProviders.Contains(currentAiServiceProvider, StringComparer.OrdinalIgnoreCase))
                configuration.Capabilities.SupportedProviders.Add(currentAiServiceProvider);
        }
        else
        {
            throw new InvalidOperationException(string.Format(ExceptionConstants.InvalidServiceProvider, configuration.ServiceProvider));
        }

        return configuration;
    }

    /// <summary>
    /// Builds an agent configuration from application configuration.
    /// </summary>
    /// <param name="appConfiguration">The application configuration.</param>
    /// <param name="serviceProvider">The service provider name.</param>
    /// <returns>An agent configuration.</returns>
    private static AgentConfiguration BuildAgentConfigurationFromAppConfig(IConfiguration appConfiguration, string serviceProvider)
    {
        var agentConfig = new AgentConfiguration
        {
            Name = $"{serviceProvider} Agent",
            ServiceProvider = serviceProvider,
            Capabilities = new AgentCapabilities
            {
                SupportsChatCompletion = true,
                SupportsPluginExecution = true,
                SupportsMcpIntegration = true,
                SupportedProviders = [serviceProvider]
            }
        };

        // Configure based on service provider
        switch (serviceProvider)
        {
            case GoogleGeminiAiConstants.ServiceProviderName:
                var isProModelEnabled = bool.TryParse(appConfiguration[GoogleGeminiAiConstants.IsProModelEnabledFlag], out bool parsedValue) && parsedValue;
                var geminiAiModel = isProModelEnabled ? GoogleGeminiAiConstants.GeminiProModel : GoogleGeminiAiConstants.GeminiFlashModel;

                agentConfig.ModelId = appConfiguration[geminiAiModel];
                agentConfig.ApiKey = appConfiguration[GoogleGeminiAiConstants.GeminiAPIKeyConstant];
                agentConfig.ServiceProvider = GoogleGeminiAiConstants.ServiceProviderName;
                break;

            case PerplexityAiConstants.ServiceProviderName:
                agentConfig.ModelId = appConfiguration[PerplexityAiConstants.ModelId];
                agentConfig.ApiKey = appConfiguration[PerplexityAiConstants.ApiKey];
                agentConfig.ApiEndpoint = appConfiguration[PerplexityAiConstants.ApiEndpoint];
                agentConfig.ServiceProvider = PerplexityAiConstants.ServiceProviderName;
                break;

            case ChatGptAiConstants.ServiceProviderName:
                agentConfig.ModelId = appConfiguration[ChatGptAiConstants.ModelId];
                agentConfig.ApiKey = appConfiguration[ChatGptAiConstants.ApiKey];
                agentConfig.ApiEndpoint = appConfiguration[ChatGptAiConstants.ApiEndpoint];
                agentConfig.ServiceProvider = ChatGptAiConstants.ServiceProviderName;
                break;

            default:
                throw new InvalidOperationException(string.Format(ExceptionConstants.InvalidServiceProvider, serviceProvider));
        }

        return agentConfig;
    }

    #endregion
}