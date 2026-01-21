namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The associated agents skill data dto model.
/// </summary>
public sealed record AssociatedAgentsSkillDataDTO
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
