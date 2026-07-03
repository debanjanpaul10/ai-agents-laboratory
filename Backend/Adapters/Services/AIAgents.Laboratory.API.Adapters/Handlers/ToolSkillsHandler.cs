using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.Ports.In;
using static AIAgents.Laboratory.API.Adapters.Mapper.DomainToResponseMapper;
using static AIAgents.Laboratory.API.Adapters.Mapper.RequestToDomainMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The implementation for tool skills api adapter handler.
/// </summary>
/// <param name="toolSkillsService">The tool skills service.</param>
/// <seealso cref="IToolSkillsHandler"/>
public sealed class ToolSkillsHandler(IToolSkillsService toolSkillsService) : IToolSkillsHandler
{
    /// <inheritdoc />
    public async Task<bool> AddNewToolSkillAsync(
        ToolSkillDTO toolSkillData,
        string userEmail,
        CancellationToken cancellationToken = default
    )
    {
        var domainInput = MapToDomain(toolSkillData);
        return await toolSkillsService.AddNewToolSkillAsync(
            toolSkillData: domainInput,
            userEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteExistingToolSkillBySkillIdAsync(
        string toolSkillId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    ) =>
        await toolSkillsService.DeleteExistingToolSkillBySkillIdAsync(
            toolSkillId,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<IEnumerable<McpServerToolsDTO>> GetAllMcpToolsAvailableAsync(
        string serverUrl,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        var domainResult = await toolSkillsService.GetAllMcpToolsAvailableAsync(
            serverUrl,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
        return domainResult.Select(selector: tool => new McpServerToolsDTO
        {
            ToolName = tool.Name,
            ToolDescription = tool.Description
        });
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ToolSkillDTO>> GetAllToolSkillsAsync(
        string userEmail,
        CancellationToken cancellationToken = default
    )
    {
        var domainResult = await toolSkillsService.GetAllToolSkillsAsync(
            userEmail,
            cancellationToken
        ).ConfigureAwait(false);
        return [.. domainResult.Select(MapToDto)];
    }

    /// <inheritdoc />
    public async Task<ToolSkillDTO> GetToolSkillBySkillIdAsync(
        string toolSkillId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        var domainResult = await toolSkillsService.GetToolSkillBySkillIdAsync(
            toolSkillId,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
        return MapToDto(domainResult);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateExistingToolSkillDataAsync(
        ToolSkillDTO updateToolSkillData,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        var domainInput = MapToDomain(updateToolSkillData);
        return await toolSkillsService.UpdateExistingToolSkillDataAsync(
            updateToolSkillData: domainInput,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
