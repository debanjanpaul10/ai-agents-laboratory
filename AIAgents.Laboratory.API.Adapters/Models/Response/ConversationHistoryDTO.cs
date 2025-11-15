namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The Conversation History DTO Model.
/// </summary>
public class ConversationHistoryDTO
{
	/// <summary>
	/// Gets or sets the identifier.
	/// </summary>
	/// <value>
	/// The identifier.
	/// </value>
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The conversation id guid.
	/// </summary>
	public string ConversationId { get; set; } = string.Empty;

	/// <summary>
	/// The user name.
	/// </summary>
	public string UserName { get; set; } = string.Empty;

	/// <summary>
	/// The Chat history domain.
	/// </summary>
	public List<ChatHistoryDTO> ChatHistory { get; set; } = [];

	/// <summary>
	/// The is active boolean flag.
	/// </summary>
	public bool IsActive { get; set; }

	/// <summary>
	/// The last modified on date.
	/// </summary>
	public DateTime LastModifiedOn { get; set; }
}
