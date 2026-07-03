namespace AIAgents.Laboratory.Domain.Models.Skills;

/// <summary>
/// The associated agents skill data domain model.
/// </summary>
public sealed record AssociatedAgentsSkillDataDomain
{
    /// <summary>
    /// The agent name.
    /// </summary>
    public string AgentName { get; set; } = string.Empty;

    /// <summary>
    /// The agent guid.
    /// </summary>
    public string AgentGuid { get; set; } = string.Empty;
}