using System.ComponentModel.DataAnnotations;

namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.Models;

/// <summary>
/// Configuration class for agent settings and initialization parameters.
/// </summary>
public sealed record AgentConfiguration
{
    /// <summary>
    /// Gets or sets the unique identifier for the agent.
    /// </summary>
    [Required]
    public string AgentId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name of the agent.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the agent's purpose and capabilities.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the agent capabilities configuration.
    /// </summary>
    [Required]
    public AgentCapabilities Capabilities { get; set; } = new();

    /// <summary>
    /// Gets or sets additional configuration settings for the agent.
    /// </summary>
    public Dictionary<string, object> Settings { get; set; } = new();

    /// <summary>
    /// Gets or sets the AI service provider to use for this agent.
    /// </summary>
    [Required]
    public string ServiceProvider { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the model identifier for the AI service.
    /// </summary>
    public string? ModelId { get; set; }

    /// <summary>
    /// Gets or sets the API key for the AI service.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Gets or sets the API endpoint for the AI service.
    /// </summary>
    public string? ApiEndpoint { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of tokens for responses.
    /// </summary>
    public int? MaxTokens { get; set; }

    /// <summary>
    /// Gets or sets the temperature setting for response generation.
    /// </summary>
    public double? Temperature { get; set; }

    /// <summary>
    /// Validates the agent configuration.
    /// </summary>
    /// <returns>A list of validation errors, empty if configuration is valid.</returns>
    public List<string> Validate()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(AgentId))
        {
            errors.Add("AgentId is required and cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(Name))
        {
            errors.Add("Name is required and cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(ServiceProvider))
        {
            errors.Add("ServiceProvider is required and cannot be empty.");
        }

        if (Capabilities == null)
        {
            errors.Add("Capabilities configuration is required.");
        }
        else
        {
            errors.AddRange(Capabilities.Validate());
        }

        // Validate service provider specific requirements
        errors.AddRange(ValidateServiceProviderSettings());

        return errors;
    }

    /// <summary>
    /// Validates service provider specific settings.
    /// </summary>
    /// <returns>A list of validation errors for service provider settings.</returns>
    private List<string> ValidateServiceProviderSettings()
    {
        var errors = new List<string>();

        switch (ServiceProvider?.ToLowerInvariant())
        {
            case "googlegoogle":
            case "googlegemini":
                if (string.IsNullOrWhiteSpace(ApiKey))
                {
                    errors.Add("ApiKey is required for Google Gemini service provider.");
                }
                break;

            case "perplexityai":
                if (string.IsNullOrWhiteSpace(ApiKey))
                {
                    errors.Add("ApiKey is required for Perplexity AI service provider.");
                }
                if (string.IsNullOrWhiteSpace(ModelId))
                {
                    errors.Add("ModelId is required for Perplexity AI service provider.");
                }
                break;

            case "openai":
            case "chatgpt":
            case "openaigpt":
                if (string.IsNullOrWhiteSpace(ApiKey))
                {
                    errors.Add("ApiKey is required for OpenAI/ChatGPT service provider.");
                }
                if (string.IsNullOrWhiteSpace(ModelId))
                {
                    errors.Add("ModelId is required for OpenAI/ChatGPT service provider.");
                }
                break;

            default:
                errors.Add($"Unsupported service provider: {ServiceProvider}");
                break;
        }

        return errors;
    }
}