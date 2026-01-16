using AIAgents.Laboratory.API.Adapters.Models.Response;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// The interface for tool skills api adapter handler.
/// </summary>
public interface IToolSkillsHandler
{
    /// <summary>
    /// Adds a new tool skill asynchronously.
    /// </summary>
    /// <param name="toolSkillData">The tool skill data DTO model.</param>
    /// <param name="userEmail">The user email.</param>
    /// <returns>The boolean for <c>success/failure</c></returns>
    Task<bool> AddNewToolSkillAsync(ToolSkillDTO toolSkillData, string userEmail);

    /// <summary>
    /// Updates an existing tool skill data asynchronously.
    /// </summary>
    /// <param name="updateToolSkillData">The tool skill data DTO model.</param>
    /// <param name="currentUserEmail">The user email.</param>
    /// <returns>The boolean for <c>success/failure</c></returns>
    Task<bool> UpdateExistingToolSkillDataAsync(ToolSkillDTO updateToolSkillData, string currentUserEmail);

    /// <summary>
    /// Gets all the tool skill data asynchronously.
    /// </summary>
    /// <param name="userEmail">The current logged in user email.</param>
    /// <returns>The list of <see cref="ToolSkillDTO"/></returns>
    Task<IEnumerable<ToolSkillDTO>> GetAllToolSkillsAsync(string userEmail);

    /// <summary>
    /// Gets a single tool skill data by its skill id asynchronously.
    /// </summary>
    /// <param name="toolSkillId">The tool skill id to be fetched.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <returns>The tool skill DTO model.</returns>
    Task<ToolSkillDTO> GetToolSkillBySkillIdAsync(string toolSkillId, string currentUserEmail);

    /// <summary>
    /// Deletes an existing tool by tool skill id asynchronously.
    /// </summary>
    /// <param name="toolSkillId">The tool skill id to delete.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <returns>A boolean for success/failure.</returns>
    Task<bool> DeleteExistingToolSkillBySkillIdAsync(string toolSkillId, string currentUserEmail);

    /// <summary>
    /// Gets all MCP tools available asynchronously.
    /// </summary>
    /// <param name="serverUrl">The MCP server url.</param>
    /// <param name="currentUserEmail">The current user email.</param>
    /// <returns>The list of <see cref="McpServerToolsDTO"/></returns>
    Task<IEnumerable<McpServerToolsDTO>> GetAllMcpToolsAvailableAsync(string serverUrl, string currentUserEmail);
}
