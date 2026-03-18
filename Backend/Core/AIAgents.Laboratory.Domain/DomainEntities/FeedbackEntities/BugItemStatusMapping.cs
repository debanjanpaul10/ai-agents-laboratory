namespace AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;

/// <summary>
/// The Bug Item Status Mapping Entity Class.
/// </summary>
/// <seealso cref="BaseDomainModel"/>
public sealed record BugItemStatusMapping : BaseDomainModel
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
