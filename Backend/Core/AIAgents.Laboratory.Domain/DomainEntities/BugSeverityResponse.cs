namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The Bug Severity Response.
/// </summary>
/// <seealso cref="BaseResponse" />
public sealed record BugSeverityResponse : BaseResponse
{
    /// <summary>
    /// Gets or sets the bug severity.
    /// </summary>
    /// <value>
    /// The bug severity.
    /// </value>
    public string BugSeverity { get; set; } = string.Empty;
}
