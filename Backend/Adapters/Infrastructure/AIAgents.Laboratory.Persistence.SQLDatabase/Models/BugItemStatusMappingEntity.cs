namespace AIAgents.Laboratory.Persistence.SQLDatabase.Models;

/// <summary>
/// The Bug Item Status Mapping Entity Class.
/// </summary>
/// <seealso cref="BaseEntity"/>
public sealed record BugItemStatusMappingEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the status.
    /// </summary>
    /// <value>
    /// The name of the status.
    /// </value>
    public string StatusName { get; set; } = string.Empty;
}
