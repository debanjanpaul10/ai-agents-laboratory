namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The Agents Workspace DTO model.
/// </summary>
public sealed record AgentsWorkspaceDTO
{
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
