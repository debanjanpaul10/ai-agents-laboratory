namespace AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;

/// <summary>
/// The Bug Severity Mapping.
/// </summary>
/// <seealso cref="BaseEntity"/>
public class BugSeverityMapping : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the severity.
    /// </summary>
    /// <value>
    /// The name of the severity.
    /// </value>
    public string SeverityName { get; set; } = string.Empty;
}