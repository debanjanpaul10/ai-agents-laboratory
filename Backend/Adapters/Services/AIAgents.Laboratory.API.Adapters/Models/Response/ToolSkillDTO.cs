namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The tool skill dto model.
/// </summary>
public sealed record ToolSkillDTO
{
    /// <summary>
    /// The tool skill id guid.
    /// </summary>
    public string ToolSkillGuid { get; set; } = string.Empty;

    /// <summary>
    /// The tool skill display name.
    /// </summary>
    public string ToolSkillDisplayName { get; set; } = string.Empty;

    /// <summary>
    /// The tool skill technical name.
    /// </summary>
    public string ToolSkillTechnicalName { get; set; } = string.Empty;

    /// <summary>
    /// The tool skill mcp server url.
    /// </summary>
    public string ToolSkillMcpServerUrl { get; set; } = string.Empty;

    /// <summary>
    /// The list of associated agents containing the agent guid and the agent name.
    /// </summary>
    public IList<AssociatedAgentsSkillDataDTO> AssociatedAgents { get; set; } = [];

    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    /// <value>
    /// The date created.
    /// </value>
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the created by.
    /// </summary>
    /// <value>
    /// The created by.
    /// </value>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date modified.
    /// </summary>
    /// <value>
    /// The date modified.
    /// </value>
    public DateTime DateModified { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the modified by.
    /// </summary>
    /// <value>
    /// The modified by.
    /// </value>
    public string ModifiedBy { get; set; } = string.Empty;

}
