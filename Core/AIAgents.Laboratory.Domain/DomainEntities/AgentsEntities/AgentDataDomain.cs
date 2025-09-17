using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

/// <summary>
/// The Agent Data Domain model.
/// </summary>
[BsonIgnoreExtraElements]
public class AgentDataDomain
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
	/// Gets or sets the agent identifier.
	/// </summary>
	/// <value>
	/// The agent identifier.
	/// </value>
	public string AgentId { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the name of the agent.
	/// </summary>
	/// <value>
	/// The name of the agent.
	/// </value>
	public string AgentName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the agent meta prompt.
	/// </summary>
	/// <value>
	/// The agent meta prompt.
	/// </value>
	public string AgentMetaPrompt { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the agent skill ids.
	/// </summary>
	/// <value>
	/// The agent skill ids.
	/// </value>
	public IEnumerable<string> AgentSkillIds { get; set; } = [];

	/// <summary>
	/// Gets or sets the name of the application.
	/// </summary>
	/// <value>
	/// The name of the application.
	/// </value>
	public string ApplicationName { get; set; } = string.Empty;
}
