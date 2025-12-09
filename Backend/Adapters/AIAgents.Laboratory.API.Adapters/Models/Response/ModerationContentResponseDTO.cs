namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The moderation content response DTO.
/// </summary>
public sealed record ModerationContentResponseDTO : BaseResponseDTO
{
    /// <summary>
    /// The content rating for the user story.
    /// </summary>
    public string ContentRating { get; set; } = string.Empty;
}
