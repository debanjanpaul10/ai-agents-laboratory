using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Request;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.Ports.In;
using static AIAgents.Laboratory.API.Adapters.Mapper.DomainToResponseMapper;
using static AIAgents.Laboratory.API.Adapters.Mapper.RequestToDomainMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The agents api adapter handler implementation class.
/// </summary>
/// <param name="agentsService">The agents service.</param>
/// <seealso cref="AIAgents.Laboratory.API.Adapters.Contracts.IAgentsHandler" />
public sealed class AgentsHandler(IAgentsService agentsService) : IAgentsHandler
{
    /// <inheritdoc/>
    public async Task<bool> CreateNewAgentAsync(
        CreateAgentDTO agentData,
        string userEmail,
        CancellationToken cancellationToken = default
    )
    {
        var domainInput = MapToDomain(dto: agentData);
        return await agentsService.CreateNewAgentAsync(
            agentData: domainInput,
            userEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<AgentDataDTO> GetAgentDataByIdAsync(
        string agentId,
        string userEmail,
        CancellationToken cancellationToken = default
    )
    {
        var domainResult = await agentsService.GetAgentDataByIdAsync(
            agentId,
            userEmail,
            cancellationToken
        ).ConfigureAwait(false);
        return MapToDto(domainResult);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AgentDataDTO>> GetAllAgentsDataAsync(
        string userEmail,
        CancellationToken cancellationToken = default
    )
    {
        var domainResult = await agentsService.GetAllAgentsDataAsync(
            userEmail,
            cancellationToken
        ).ConfigureAwait(false);
        return [.. domainResult.Select(MapToDto)];
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateExistingAgentDataAsync(
        AgentDataDTO updateAgentData,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        var domainRequest = MapToDomain(updateAgentData);
        return await agentsService.UpdateExistingAgentDataAsync(
            updateDataDomain: domainRequest,
            userEmail: currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteExistingAgentDataAsync(
        string agentId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        return await agentsService.DeleteExistingAgentDataAsync(
            agentId,
            currentUserEmail,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<string> DownloadKnowledgebaseFileAsync(
        string agentGuid,
        string fileName,
        CancellationToken cancellationToken = default
    )
    {
        return await agentsService.DownloadKnowledgebaseFileAsync(
            agentGuid,
            fileName,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
