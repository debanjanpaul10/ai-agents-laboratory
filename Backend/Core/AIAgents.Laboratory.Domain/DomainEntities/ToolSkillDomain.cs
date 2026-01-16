
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The tool skill domain model.
/// </summary>
/// <seealso cref="BaseEntity"/>
[BsonIgnoreExtraElements]
public sealed record ToolSkillDomain : BaseEntity
{
    /// <summary>
    /// The Tool skill id.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

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
    public IList<AssociatedAgentsSkillDataDomain> AssociatedAgents { get; set; } = [];
}
