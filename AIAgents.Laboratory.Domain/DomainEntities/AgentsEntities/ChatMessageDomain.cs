namespace AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;

/// <summary>
/// The Chat Message Domain model.
/// </summary>
public class ChatMessageDomain
{
	/// <summary>
	/// Gets or sets the name of the agent.
	/// </summary>
	/// <value>
	/// The name of the agent.
	/// </value>
	public string AgentName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the user message.
	/// </summary>
	/// <value>
	/// The user message.
	/// </value>
	public string UserMessage { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the agent meta prompt.
	/// </summary>
	/// <value>
	/// The agent meta prompt.
	/// </value>
	public string AgentMetaPrompt { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the knowledge base data.
	/// </summary>
	/// <value>
	/// The knowledge base domain data.
	/// </value>
	public string? KnowledgeBase { get; set; } = string.Empty;
}
