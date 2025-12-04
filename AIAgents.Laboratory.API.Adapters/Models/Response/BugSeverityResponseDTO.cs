namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The Bug Severity Response DTO.
/// </summary>
/// <seealso cref="BaseResponseDTO" />
public sealed record BugSeverityResponseDTO : BaseResponseDTO
{
    /// <summary>
    /// Gets or sets the bug severity.
    /// </summary>
    /// <value>
    /// The bug severity.
    /// </value>
    public string BugSeverity { get; set; } = string.Empty;
}
