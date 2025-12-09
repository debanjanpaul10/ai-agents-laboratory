namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The Rewrite response data DTO.
/// </summary>
public sealed record RewriteResponseDTO : BaseResponseDTO
{
    /// <summary>
    /// The rewritten story.
    /// </summary>
    public string RewrittenStory { get; set; } = string.Empty;
}
