// *********************************************************************************
//	<copyright file="ICommonAiHandler.cs" company="Personal">
//		Copyright (c) 2025 Personal
//	</copyright>
// <summary>Common AI handler interface.</summary>
// *********************************************************************************

using AIAgents.Laboratory.API.Adapters.Models.Response;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// The common AI handler interface.
/// </summary>
public interface ICommonAiHandler
{
	/// <summary>
	/// Gets the agent current status.
	/// </summary>
	/// <returns>The agent status data.</returns>
	AgentStatusDTO GetAgentCurrentStatus();
}
