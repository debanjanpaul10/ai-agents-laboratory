namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The user story tag response DTO.
/// </summary>
public sealed record TagResponse : BaseResponse
{
    /// <summary>
    /// The user story genre.
    /// </summary>
    public string UserStoryTag { get; set; } = string.Empty;
}
