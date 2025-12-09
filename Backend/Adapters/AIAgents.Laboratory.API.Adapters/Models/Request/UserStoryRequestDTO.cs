namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The Common User Story Request dto.
/// </summary>
public sealed record UserStoryRequestDTO
{
    /// <summary>
    /// Gets or sets the story.
    /// </summary>
    /// <value>
    /// The story.
    /// </value>
    public string Story { get; set; } = string.Empty;
}