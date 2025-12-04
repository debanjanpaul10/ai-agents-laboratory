namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The Direct Chat Request DTO model.
/// </summary>
public sealed record DirectChatRequestDTO
{
    /// <summary>
    /// Gets or sets the user message.
    /// </summary>
    /// <value>
    /// The user message.
    /// </value>
    public string UserMessage { get; set; } = string.Empty;
}
