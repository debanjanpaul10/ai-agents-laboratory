namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The user story tag response DTO.
/// </summary>
public sealed record TagResponseDTO : BaseResponseDTO
{
    /// <summary>
    /// The user story genre.
    /// </summary>
    public string UserStoryTag { get; set; } = string.Empty;
}
