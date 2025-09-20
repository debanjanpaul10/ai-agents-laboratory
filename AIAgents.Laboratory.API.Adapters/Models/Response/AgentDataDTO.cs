using AIAgents.Laboratory.API.Adapters.Models.Request;

namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The Agent Data DTO model.
/// </summary>
/// <seealso cref="AIAgents.Laboratory.API.Adapters.Models.Request.CreateAgentDTO" />
public class AgentDataDTO : CreateAgentDTO
{
    /// <summary>
    /// Gets or sets the agent identifier.
    /// </summary>
    /// <value>
    /// The agent identifier.
    /// </value>
    public string AgentId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the created by.
    /// </summary>
    /// <value>
    /// The created by.
    /// </value>
    public string CreatedBy { get; set; } = string.Empty;
}
