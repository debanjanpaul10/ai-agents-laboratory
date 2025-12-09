namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The Bug Severity Input DTO.
/// </summary>
public sealed record BugSeverityInputDTO
{
    /// <summary>
    /// Gets or sets the bug title.
    /// </summary>
    /// <value>
    /// The bug title.
    /// </value>
    public string BugTitle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bug description.
    /// </summary>
    /// <value>
    /// The bug description.
    /// </value>
    public string BugDescription { get; set; } = string.Empty;
}
