﻿namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The Create Agent Data DTO model.
/// </summary>
public class CreateAgentDTO
{
	/// <summary>
	/// Gets or sets the name of the agent.
	/// </summary>
	/// <value>
	/// The name of the agent.
	/// </value>
	public string AgentName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the agent meta prompt.
	/// </summary>
	/// <value>
	/// The agent meta prompt.
	/// </value>
	public string AgentMetaPrompt { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the name of the application.
	/// </summary>
	/// <value>
	/// The name of the application.
	/// </value>
	public string ApplicationName { get; set; } = string.Empty;
}
