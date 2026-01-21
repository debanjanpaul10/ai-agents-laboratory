using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The implementation for tool skills api adapter handler.
/// </summary>
/// <param name="mapper">The auto mapper service.</param>
/// <param name="toolSkillsService">The tool skills service.</param>
/// <seealso cref="IToolSkillsHandler"/>
public sealed class ToolSkillsHandler(IMapper mapper, IToolSkillsService toolSkillsService) : IToolSkillsHandler
{
    /// <summary>
    /// Adds a new tool skill asynchronously.
    /// </summary>
    /// <param name="toolSkillData">The tool skill data DTO model.</param>
    /// <param name="userEmail">The user email.</param>
    /// <returns>The boolean for <c>success/failure</c></returns>
    public async Task<bool> AddNewToolSkillAsync(ToolSkillDTO toolSkillData, string userEmail)
    {
        var domainInput = mapper.Map<ToolSkillDomain>(toolSkillData);
        return await toolSkillsService.AddNewToolSkillAsync(domainInput, userEmail).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes an existing tool by tool skill id asynchronously.
    /// </summary>
    /// <param name="toolSkillId">The tool skill id to delete.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> DeleteExistingToolSkillBySkillIdAsync(string toolSkillId, string currentUserEmail)
    {
        return await toolSkillsService.DeleteExistingToolSkillBySkillIdAsync(toolSkillId, currentUserEmail).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets all MCP tools available asynchronously.
    /// </summary>
    /// <param name="serverUrl">The MCP server url.</param>
    /// <param name="currentUserEmail">The current user email.</param>
    /// <returns>The list of <see cref="McpServerToolsDTO"/></returns>
    public async Task<IEnumerable<McpServerToolsDTO>> GetAllMcpToolsAvailableAsync(string serverUrl, string currentUserEmail)
    {
        var domainResult = await toolSkillsService.GetAllMcpToolsAvailableAsync(serverUrl, currentUserEmail).ConfigureAwait(false);
        return domainResult.Select(tool => new McpServerToolsDTO
        {
            ToolName = tool.Name,
            ToolDescription = tool.Description
        });
    }

    /// <summary>
    /// Gets all the tool skill data asynchronously.
    /// </summary>
    /// <param name="userEmail">The current logged in user email.</param>
    /// <returns>The list of <see cref="ToolSkillDTO"/></returns>
    public async Task<IEnumerable<ToolSkillDTO>> GetAllToolSkillsAsync(string userEmail)
    {
        var domainResult = await toolSkillsService.GetAllToolSkillsAsync(userEmail).ConfigureAwait(false);
        return mapper.Map<IEnumerable<ToolSkillDTO>>(domainResult);
    }

    /// <summary>
    /// Gets a single tool skill data by its skill id asynchronously.
    /// </summary>
    /// <param name="toolSkillId">The tool skill id to be fetched.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <returns>The tool skill DTO model.</returns>
    public async Task<ToolSkillDTO> GetToolSkillBySkillIdAsync(string toolSkillId, string currentUserEmail)
    {
        var domainResult = await toolSkillsService.GetToolSkillBySkillIdAsync(toolSkillId, currentUserEmail).ConfigureAwait(false);
        return mapper.Map<ToolSkillDTO>(domainResult);
    }

    /// <summary>
    /// Updates an existing tool skill data asynchronously.
    /// </summary>
    /// <param name="updateToolSkillData">The tool skill data DTO model.</param>
    /// <param name="currentUserEmail">The user email.</param>
    /// <returns>The boolean for <c>success/failure</c></returns>
    public async Task<bool> UpdateExistingToolSkillDataAsync(ToolSkillDTO updateToolSkillData, string currentUserEmail)
    {
        var domainInput = mapper.Map<ToolSkillDomain>(updateToolSkillData);
        return await toolSkillsService.UpdateExistingToolSkillDataAsync(domainInput, currentUserEmail).ConfigureAwait(false);
    }
}
