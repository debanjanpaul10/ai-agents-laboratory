using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The agents workspace domain model.
/// </summary>
/// <seealso cref="BaseEntity"/>
[BsonIgnoreExtraElements]
public sealed record AgentsWorkspaceDomain : BaseEntity
{
    /// <summary>
    /// The Tool skill id.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the agent workspace guid id.
    /// </summary>
    public string AgentWorkspaceGuid { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the agent workspace name.
    /// </summary>
    public string AgentWorkspaceName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the active agents name and active agents guids.
    /// </summary>
    public Dictionary<string, string> ActiveAgentsListInWorkspace { get; set; } = [];

    /// <summary>
    /// Gets or sets the active users in the workspace.
    /// </summary>
    public IEnumerable<string> WorkspaceUsers { get; set; } = [];
}
