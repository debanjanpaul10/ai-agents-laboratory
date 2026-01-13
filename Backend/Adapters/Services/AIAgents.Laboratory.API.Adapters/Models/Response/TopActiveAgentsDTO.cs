namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The top active agents data DTO.
/// </summary>
public sealed record TopActiveAgentsDTO
{
    /// <summary>
    /// Gets or sets the count of active agents.
    /// </summary>
    /// <value>The count of active agents.</value>
    public int ActiveAgentsCount { get; set; }

    /// <summary>
    /// Gets or sets the list of top active agents.
    /// </summary>
    /// <value>The list of top active agents.</value>
    public IEnumerable<AgentDataDTO> TopActiveAgents { get; set; } = [];

}
