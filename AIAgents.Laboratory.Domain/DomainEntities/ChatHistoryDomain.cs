namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The Chat message domain model class.
/// </summary>
public class ChatHistoryDomain
{
    /// <summary>
    /// The role of the message sender.
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// The message content.
    /// </summary>
    public string Content { get; set; } = string.Empty;
}