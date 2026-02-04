namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The group chat agent response DTO.
/// </summary>
public sealed record GroupChatAgentsResponseDto
{
    /// <summary>
    /// Gets or sets the agent name.
    /// </summary>
    /// <value>The agent name.</value>
    public string AgentName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the agent response.
    /// </summary>
    /// <value>The agent response.</value>
    public string AgentResponse { get; set; } = string.Empty;
}
