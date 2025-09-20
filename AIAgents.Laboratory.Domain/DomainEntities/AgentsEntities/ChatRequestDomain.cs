namespace AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

/// <summary>
/// The Chat Request Domain Model.
/// </summary>
public class ChatRequestDomain
{
	/// <summary>
	/// Gets or sets the name of the agent.
	/// </summary>
	/// <value>
	/// The name of the agent.
	/// </value>
	public string AgentName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the agent identifier.
	/// </summary>
	/// <value>
	/// The agent identifier.
	/// </value>
	public string AgentId { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the conversation identifier.
	/// </summary>
	/// <value>
	/// The conversation identifier.
	/// </value>
	public string ConversationId { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the user message.
	/// </summary>
	/// <value>
	/// The user message.
	/// </value>
	public string UserMessage { get; set; } = string.Empty;
}
