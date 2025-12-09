namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The moderation content response DTO.
/// </summary>
public sealed record ModerationContentResponse : BaseResponse
{
    /// <summary>
    /// The content rating for the user story.
    /// </summary>
    public string ContentRating { get; set; } = string.Empty;
}
