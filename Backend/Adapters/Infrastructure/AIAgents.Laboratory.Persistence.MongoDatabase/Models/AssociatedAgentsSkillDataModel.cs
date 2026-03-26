namespace AIAgents.Laboratory.Persistence.MongoDatabase.Models;

/// <summary>
/// The associated agents skill data model.
/// </summary>
public class AssociatedAgentsSkillDataModel
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
