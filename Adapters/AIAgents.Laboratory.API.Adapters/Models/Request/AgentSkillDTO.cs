namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The Agent Skill request DTO.
/// </summary>
public class AgentSkillDTO
{
	/// <summary>
	/// Gets or sets the skill identifier.
	/// </summary>
	/// <value>
	/// The skill identifier.
	/// </value>
	public string SkillId { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the name of the skill.
	/// </summary>
	/// <value>
	/// The name of the skill.
	/// </value>
	public string SkillName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the spec URL.
	/// </summary>
	/// <value>
	/// The spec URL.
	/// </value>
	public string SpecUrl { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the raw spec.
	/// </summary>
	/// <value>
	/// The raw spec.
	/// </value>
	public string RawSpec { get; set; } = string.Empty;
}
