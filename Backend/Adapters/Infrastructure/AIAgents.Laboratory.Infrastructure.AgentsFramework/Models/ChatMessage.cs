namespace AIAgents.Laboratory.Infrastructure.AgentsFramework.Models;

/// <summary>
/// The chat message model.
/// </summary>
public sealed record ChatMessage
{
    /// <summary>
    /// The role of the message sender.
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// The content of the message.
    /// </summary>
    public string Content { get; set; } = string.Empty;
}
