namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The Add New Bug Report Data DTO.
/// </summary>
public class AddBugReportDTO
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

    /// <summary>
    /// Gets or sets the bug severity.
    /// </summary>
    /// <value>
    /// The bug severity.
    /// </value>
    public int BugSeverity { get; set; } = 0;

    /// <summary>
    /// Gets or sets the created by.
    /// </summary>
    /// <value>
    /// The created by.
    /// </value>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the page URL.
    /// </summary>
    /// <value>
    /// The page URL.
    /// </value>
    public string PageUrl { get; set; } = string.Empty;
}
