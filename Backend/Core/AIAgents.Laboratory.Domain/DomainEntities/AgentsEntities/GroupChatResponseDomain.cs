namespace AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

/// <summary>
/// The group chat response domain entity.
/// </summary>
public sealed record GroupChatResponseDomain
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
