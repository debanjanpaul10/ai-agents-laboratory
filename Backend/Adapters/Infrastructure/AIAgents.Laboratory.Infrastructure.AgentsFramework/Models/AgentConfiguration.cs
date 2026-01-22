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
}