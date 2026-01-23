using AIAgents.Laboratory.API.Adapters.Models.Base;

namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The tool skill dto model.
/// </summary>
/// <seealso cref="BaseModelDTO"/>
public sealed record ToolSkillDTO : BaseModelDTO
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
}
