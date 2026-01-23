using AIAgents.Laboratory.API.Adapters.Models.Base;
using AIAgents.Laboratory.API.Adapters.Models.Request;

namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The Agents Workspace DTO model.
/// </summary>
/// <seealso cref="BaseModelDTO"/>
public sealed record AgentsWorkspaceDTO : BaseModelDTO
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
    public IEnumerable<WorkspaceAgentsDataDTO> ActiveAgentsListInWorkspace { get; set; } = [];

    /// <summary>
    /// Gets or sets the active users in the workspace.
    /// </summary>
    public IEnumerable<string> WorkspaceUsers { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether group chat is enabled.
    /// </summary>
    /// <value>The value indicating whether group chat is enabled.</value>
    public bool IsGroupChatEnabled { get; set; }
}
