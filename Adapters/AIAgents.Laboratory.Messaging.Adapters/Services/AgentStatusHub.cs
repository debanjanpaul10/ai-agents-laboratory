// *********************************************************************************
//	<copyright file="AgentStatusHub.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>The Agent Status Hub for real-time notifications.</summary>
// *********************************************************************************

using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using Microsoft.AspNetCore.SignalR;

namespace AIAgents.Laboratory.Messaging.Adapters.Services;

/// <summary>
/// The Agent Status Hub for real-time notifications.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.SignalR.Hub" />
public class AgentStatusHub(IAgentStatusStore agentStatusStore) : Hub
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
        await Clients.Caller.SendAsync("ReceiveAgentStatus", agentStatusStore.Current);
        await base.OnConnectedAsync();
    }
}
