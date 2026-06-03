namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The group chat response data transfer object.
/// </summary>
public sealed record GroupChatResponseDto
{
    /// <summary>
    /// The agent response.
    /// </summary>
    public string AgentResponse { get; init; } = string.Empty;

    /// <summary>
    /// Gets the conversation identifier.
    /// </summary>
    /// <value>
    /// The conversation identifier.
    /// </value>
    public string ConversationId { get; init; } = string.Empty;

    /// <summary>
    /// The list of agents invoked.
    /// </summary>
    public IList<string> AgentsInvoked { get; init; } = [];
}
