namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The AI Agent Response DTO.
/// </summary>
public sealed record AIAgentResponseDTO
{
    /// <summary>
    /// Gets or sets the ai response data.
    /// </summary>
    /// <value>
    /// The ai response data.
    /// </value>
    public string AIResponseData { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user query.
    /// </summary>
    /// <value>
    /// The user query.
    /// </value>
    public string UserQuery { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user intent.
    /// </summary>
    /// <value>
    /// The user intent.
    /// </value>
    public string UserIntent { get; set; } = string.Empty;
}
