using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Mapper;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.Ports.In;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// The Common AI Handler.
/// </summary>
/// <param name="commonAiService">The common ai service.</param>
/// <seealso cref="AIAgents.Laboratory.API.Adapters.Contracts.ICommonAiHandler" />
public sealed class CommonAiHandler(ICommonAiService commonAiService) : ICommonAiHandler
{
    /// <inheritdoc/>
    public Dictionary<string, string> GetConfigurationByKeyName(string key)
    {
        return commonAiService.GetConfigurationByKeyName(key);
    }

    /// <inheritdoc/>
    public Dictionary<string, string> GetConfigurationsData(string userName)
    {
        return commonAiService.GetConfigurationsData(userName);
    }

    /// <inheritdoc/>
    public async Task<TopActiveAgentsDTO> GetTopActiveAgentsDataAsync(string userName, CancellationToken cancellationToken = default)
    {
        var (ActiveAgentsCount, TopActiveAgentsList) = await commonAiService.GetTopActiveAgentsDataAsync(
            userName,
            cancellationToken
        ).ConfigureAwait(false);

        return new()
        {
            ActiveAgentsCount = ActiveAgentsCount,
            TopActiveAgents = [.. TopActiveAgentsList.Select(DomainMapperProfile.MapToDto)]
        };
    }
}
