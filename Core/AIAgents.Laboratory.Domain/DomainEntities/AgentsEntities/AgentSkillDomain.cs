using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

/// <summary>
/// The Agent Skill Domain model.
/// </summary>
[BsonIgnoreExtraElements]
public class AgentSkillDomain
{
	/// <summary>
	/// Gets or sets the identifier.
	/// </summary>
	/// <value>
	/// The identifier.
	/// </value>
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; } = string.Empty;

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

	/// <summary>
	/// Gets or sets the status.
	/// </summary>
	/// <value>
	/// The status.
	/// </value>
	public string Status { get; set; } = string.Empty;
}
