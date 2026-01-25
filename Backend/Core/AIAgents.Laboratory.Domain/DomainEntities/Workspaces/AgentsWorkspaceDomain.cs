using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AIAgents.Laboratory.Domain.DomainEntities.Workspaces;

/// <summary>
/// The agents workspace domain model.
/// </summary>
/// <seealso cref="BaseEntity"/>
[BsonIgnoreExtraElements]
public sealed record AgentsWorkspaceDomain : BaseEntity
{
    /// <summary>
    /// Gets or sets the Tool skill id.
    /// </summary>
    /// <value>The Tool skill id.</value>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the agent workspace guid id.
    /// </summary>
    /// <value>The agent workspace guid id.</value>
    public string AgentWorkspaceGuid { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the agent workspace name.
    /// </summary>
    /// <value>The agent workspace name.</value>
    public string AgentWorkspaceName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the active agents name and active agents guids.
    /// </summary>
    /// <value>The active agents name and active agents guids.</value>
    public IEnumerable<WorkspaceAgentsDataDomain> ActiveAgentsListInWorkspace { get; set; } = [];

    /// <summary>
    /// Gets or sets the active users in the workspace.
    /// </summary>
    /// <value>The active users in the workspace.</value>
    public IEnumerable<string> WorkspaceUsers { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether group chat is enabled.
    /// </summary>
    /// <value>The value indicating whether group chat is enabled.</value>
    public bool IsGroupChatEnabled { get; set; }
}
