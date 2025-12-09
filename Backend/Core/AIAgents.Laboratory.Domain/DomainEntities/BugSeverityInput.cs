namespace AIAgents.Laboratory.Domain.DomainEntities;

/// <summary>
/// The Bug Severity Input Domain Class.
/// </summary>
public sealed record BugSeverityInput
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
