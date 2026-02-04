namespace AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

/// <summary>
/// The orchestrator agent response domain model.
/// </summary>
public sealed record OrchestratorFinalResponseDomain
{
    /// <summary>
    /// Gets or sets the final response.
    /// </summary>
    /// <value>The final response.</value>
    public string FinalResponse { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets The group chat agents response domain model.
    /// </summary>
    /// <value>The group chat agents response domain model.</value>
    public IEnumerable<GroupChatAgentsResponseDomain> GroupChatAgentsResponses { get; init; } = [];
}
