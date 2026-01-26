namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The group chat response data transfer object.
/// </summary>
public sealed record GroupChatResponseDTO
{
    /// <summary>
    /// The agent response.
    /// </summary>
    public string AgentResponse { get; init; } = string.Empty;

    /// <summary>
    /// The list of agents invoked.
    /// </summary>
    public IList<string> AgentsInvoked { get; init; } = [];
}
