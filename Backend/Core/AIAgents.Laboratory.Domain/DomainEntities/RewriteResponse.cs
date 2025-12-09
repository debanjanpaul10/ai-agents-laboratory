namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The Rewrite response data DTO.
/// </summary>
public sealed record RewriteResponse : BaseResponse
{
    /// <summary>
    /// The rewrittent story.
    /// </summary>
    public string RewrittenStory { get; set; } = string.Empty;
}
