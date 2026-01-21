namespace AIAgents.Laboratory.Domain.DomainEntities;

public sealed record WorkspaceAgentChatRequestDomain
{
    /// <summary>
    /// Gets or sets the conversation identifier.
    /// </summary>
    /// <value>The conversation id guid.</value>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the workspace identifier.
    /// </summary>
    /// <value>The workspace id guid.</value>
    public string WorkspaceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the agent identifier.
    /// </summary>
    /// <value>
    /// The agent identifier.
    /// </value>
    public string AgentId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user message.
    /// </summary>
    /// <value>
    /// The user message.
    /// </value>
    public string UserMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the application name.
    /// </summary>
    /// <value>The application name.</value>
    public string ApplicationName { get; set; } = string.Empty;
}
