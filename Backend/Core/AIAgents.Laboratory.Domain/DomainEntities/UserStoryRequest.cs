namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The Common User Story Request dto.
/// </summary>
public sealed record UserStoryRequest
{
    /// <summary>
    /// Gets or sets the story.
    /// </summary>
    /// <value>
    /// The story.
    /// </value>
    public string Story { get; set; } = string.Empty;
}