namespace AIAgents.Laboratory.Persistence.SQLDatabase.Models;

/// <summary>
/// The bug report data entity class representing the bug report data in the database.
/// </summary>
public sealed record BugReportDataEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bug severity identifier.
    /// </summary>
    /// <value>
    /// The bug severity identifier.
    /// </value>
    public int BugSeverityId { get; set; }

    /// <summary>
    /// Gets or sets the bug status identifier.
    /// </summary>
    /// <value>
    /// The bug status identifier.
    /// </value>
    public int BugStatusId { get; set; }

    /// <summary>
    /// The agent id
    /// </summary>
    public string AgentDetails { get; set; } = string.Empty;
}
