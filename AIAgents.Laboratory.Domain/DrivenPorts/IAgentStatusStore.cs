using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.DrivenPorts;

/// <summary>
/// The Agent Status Store Interface.
/// </summary>
public interface IAgentStatusStore
{
    /// <summary>
    /// Gets the current agent status.
    /// </summary>
    /// <value>
    /// The current agent status.
    /// </value>
    AgentStatus Current { get; }

    /// <summary>
    /// Tries to update status.
    /// </summary>
    /// <param name="newValue">if set to <c>true</c> [new value].</param>
    /// <param name="updated">The updated value.</param>
    /// <returns>The boolean for success/failure</returns>
    bool TryUpdate(bool newValue, out AgentStatus updated);
}
