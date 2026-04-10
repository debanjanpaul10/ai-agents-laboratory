using AIAgents.Laboratory.Domain.DomainEntities;
using ModelContextProtocol.Client;

namespace AIAgents.Laboratory.Domain.Ports.In;

/// <summary>
/// The Tool Skills services interface.
/// </summary>
public interface IToolSkillsService
{
    /// <summary>
    /// Adds a new tool skill asynchronously.
    /// </summary>
    /// <param name="toolSkillData">The tool skill data domain model.</param>
    /// <param name="userEmail">The user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for <c>success/failure</c></returns>
    Task<bool> AddNewToolSkillAsync(
        ToolSkillDomain toolSkillData,
        string userEmail,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Updates an existing tool skill data asynchronously.
    /// </summary>
    /// <param name="updateToolSkillData">The tool skill data domain model.</param>
    /// <param name="currentUserEmail">The user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for <c>success/failure</c></returns>
    Task<bool> UpdateExistingToolSkillDataAsync(
        ToolSkillDomain updateToolSkillData,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets all the tool skill data asynchronously.
    /// </summary>
    /// <param name="userEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="ToolSkillDomain"/></returns>
    Task<IEnumerable<ToolSkillDomain>> GetAllToolSkillsAsync(
        string userEmail,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets a single tool skill data by its skill id asynchronously.
    /// </summary>
    /// <param name="toolSkillId">The tool skill id to be fetched.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The tool skill domain model.</returns>
    Task<ToolSkillDomain> GetToolSkillBySkillIdAsync(
        string toolSkillId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Deletes an existing tool by tool skill id asynchronously.
    /// </summary>
    /// <param name="toolSkillId">The tool skill id to delete.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for success/failure.</returns>
    Task<bool> DeleteExistingToolSkillBySkillIdAsync(
        string toolSkillId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets all MCP tools available asynchronously.
    /// </summary>
    /// <param name="serverUrl">The MCP server url.</param>
    /// <param name="currentUserEmail">The current user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="McpClientTool"/></returns>
    Task<IEnumerable<McpClientTool>> GetAllMcpToolsAvailableAsync(
        string serverUrl,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Associates a skill and an agent asynchronously.
    /// </summary>
    /// <param name="agentData">The agent data containing agent name and agent guid.</param>
    /// <param name="toolSkillId">The tool skill guid id.</param>
    /// <param name="currentUserEmail">The current user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    Task<bool> AssociateSkillAndAgentAsync(
        IList<AssociatedAgentsSkillDataDomain> agentData,
        string toolSkillId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    );
}
