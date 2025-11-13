using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;

namespace AIAgents.Laboratory.Messaging.Adapters.Services;

/// <summary>
/// The Agent Status Store.
/// </summary>
/// <seealso cref="AIAgents.Laboratory.Domain.DrivenPorts.IAgentStatusStore" />
public class AgentStatusStore : IAgentStatusStore
{
    /// <summary>
    /// The lock object.
    /// </summary>
    private readonly Lock _lock = new();

    /// <summary>
    /// Gets the current agent status.
    /// </summary>
    /// <value>
    /// The current agent status.
    /// </value>
    public AgentStatus Current { get; private set; } = new() { IsAvailable = true, UpdatedAt = DateTimeOffset.UtcNow };

    /// <summary>
    /// Tries to update status.
    /// </summary>
    /// <param name="newValue">if set to <c>true</c> [new value].</param>
    /// <param name="updated">The updated value.</param>
    /// <returns>
    /// The boolean for success/failure
    /// </returns>
    public bool TryUpdate(bool newValue, out AgentStatus updated)
    {
        lock (_lock)
        {
            if (Current.IsAvailable == newValue)
            {
                updated = Current;
                return false;
            }

            Current = new AgentStatus
            {
                IsAvailable = newValue,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            updated = Current;
            return true;
        }
    }
}
