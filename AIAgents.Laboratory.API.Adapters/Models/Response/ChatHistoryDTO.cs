namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The Chat history DTO model.
/// </summary>
public class ChatHistoryDTO
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
