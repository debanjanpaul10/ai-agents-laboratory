namespace AIAgents.Laboratory.Persistence.SQLDatabase.Models;

/// <summary>
/// The Bug Severity Mapping Entity.
/// </summary>
/// <seealso cref="BaseEntity"/>
public sealed record BugSeverityMappingEntity : BaseEntity
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
