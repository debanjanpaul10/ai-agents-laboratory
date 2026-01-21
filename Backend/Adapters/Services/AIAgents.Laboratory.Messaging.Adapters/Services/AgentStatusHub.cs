using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using Microsoft.AspNetCore.SignalR;
using static AIAgents.Laboratory.Messaging.Adapters.Helpers.Constants;

namespace AIAgents.Laboratory.Messaging.Adapters.Services;

/// <summary>
/// The Agent Status Hub for real-time notifications.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.SignalR.Hub" />
public sealed class AgentStatusHub(IAgentStatusStore agentStatusStore) : Hub
{
    /// <summary>
    /// Gets the current status of the agent.
    /// </summary>
    /// <returns>The agent status.</returns>
    public AgentStatus GetCurrentStatus()
    {
        return agentStatusStore.Current;
    }

    /// <summary>
    /// Handles the Connection.
    /// </summary>
    /// <returns>A task to wait on.</returns>
    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync(MessagingConstants.ReceiveAgentStatusFunction, agentStatusStore.Current);
        await base.OnConnectedAsync();
    }
}
