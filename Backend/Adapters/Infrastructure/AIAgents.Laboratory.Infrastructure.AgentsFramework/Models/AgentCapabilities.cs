namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.Models;

/// <summary>
/// Defines the capabilities and feature flags for an agent.
/// </summary>
public sealed record AgentCapabilities
{
    /// <summary>
    /// Gets or sets a value indicating whether the agent supports chat completion functionality.
    /// </summary>
    public bool SupportsChatCompletion { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the agent supports plugin execution.
    /// </summary>
    public bool SupportsPluginExecution { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the agent supports MCP (Model Context Protocol) integration.
    /// </summary>
    public bool SupportsMcpIntegration { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the agent supports streaming responses.
    /// </summary>
    public bool SupportsStreaming { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the agent supports function calling.
    /// </summary>
    public bool SupportsFunctionCalling { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the agent supports vision/image processing.
    /// </summary>
    public bool SupportsVision { get; set; } = false;

    /// <summary>
    /// Gets or sets the list of supported AI service providers for this agent.
    /// </summary>
    public List<string> SupportedProviders { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of supported plugin types.
    /// </summary>
    public List<string> SupportedPluginTypes { get; set; } = new();

    /// <summary>
    /// Gets or sets the maximum number of concurrent operations the agent can handle.
    /// </summary>
    public int MaxConcurrentOperations { get; set; } = 1;

    /// <summary>
    /// Gets or sets the maximum context length the agent can handle.
    /// </summary>
    public int? MaxContextLength { get; set; }

    /// <summary>
    /// Gets or sets additional capability flags as key-value pairs.
    /// </summary>
    public Dictionary<string, bool> AdditionalCapabilities { get; set; } = new();

    /// <summary>
    /// Validates the agent capabilities configuration.
    /// </summary>
    /// <returns>A list of validation errors, empty if configuration is valid.</returns>
    public List<string> Validate()
    {
        var errors = new List<string>();

        if (MaxConcurrentOperations <= 0)
        {
            errors.Add("MaxConcurrentOperations must be greater than 0.");
        }

        if (MaxContextLength.HasValue && MaxContextLength.Value <= 0)
        {
            errors.Add("MaxContextLength must be greater than 0 when specified.");
        }

        // Validate that at least one core capability is enabled
        if (!SupportsChatCompletion && !SupportsPluginExecution && !SupportsMcpIntegration)
        {
            errors.Add("At least one core capability (ChatCompletion, PluginExecution, or McpIntegration) must be enabled.");
        }

        // Validate supported providers list
        if (SupportedProviders.Count == 0)
        {
            errors.Add("At least one supported provider must be specified.");
        }

        // Validate provider names
        var validProviders = new[] { "GoogleGemini", "PerplexityAi", "OpenAiGpt", "ChatGpt" };
        var invalidProviders = SupportedProviders.Where(p => !validProviders.Contains(p, StringComparer.OrdinalIgnoreCase)).ToList();

        if (invalidProviders.Any())
        {
            errors.Add($"Invalid providers specified: {string.Join(", ", invalidProviders)}. Valid providers are: {string.Join(", ", validProviders)}");
        }

        return errors;
    }

    /// <summary>
    /// Checks if the agent supports a specific provider.
    /// </summary>
    /// <param name="providerName">The name of the provider to check.</param>
    /// <returns>True if the provider is supported, false otherwise.</returns>
    public bool SupportsProvider(string providerName)
    {
        return SupportedProviders.Contains(providerName, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks if the agent supports a specific plugin type.
    /// </summary>
    /// <param name="pluginType">The type of plugin to check.</param>
    /// <returns>True if the plugin type is supported, false otherwise.</returns>
    public bool SupportsPluginType(string pluginType)
    {
        return SupportedPluginTypes.Contains(pluginType, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the value of an additional capability flag.
    /// </summary>
    /// <param name="capabilityName">The name of the capability.</param>
    /// <returns>The capability value, or false if not found.</returns>
    public bool GetAdditionalCapability(string capabilityName)
    {
        return AdditionalCapabilities.TryGetValue(capabilityName, out var value) && value;
    }

    /// <summary>
    /// Sets the value of an additional capability flag.
    /// </summary>
    /// <param name="capabilityName">The name of the capability.</param>
    /// <param name="value">The capability value.</param>
    public void SetAdditionalCapability(string capabilityName, bool value)
    {
        AdditionalCapabilities[capabilityName] = value;
    }
}