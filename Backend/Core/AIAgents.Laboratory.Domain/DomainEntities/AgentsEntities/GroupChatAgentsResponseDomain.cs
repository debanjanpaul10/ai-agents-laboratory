using System;

namespace AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

/// <summary>
/// The group chat agents response domain model.
/// </summary>
public sealed record GroupChatAgentsResponseDomain
{
    /// <summary>
    /// Gets or sets the agent name.
    /// </summary>
    /// <value>The agent name.</value>
    public string AgentName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the agent response.
    /// </summary>
    /// <value>The agent response.</value>
    public string AgentResponse { get; set; } = string.Empty;
}
