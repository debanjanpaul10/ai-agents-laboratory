namespace AIAgents.Laboratory.Persistence.MongoDatabase.Models;

/// <summary>
/// The Chat message model class.
/// </summary>
public sealed record ChatHistoryModel
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
