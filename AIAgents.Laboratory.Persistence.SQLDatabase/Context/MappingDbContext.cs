using AIAgents.Laboratory.Domain.DomainEntities.FeedbackEntities;
using Microsoft.EntityFrameworkCore;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.Context;

/// <summary>
/// The Mapping DB Context class.
/// </summary>
public partial class SqlDbContext
{
    /// <summary>
    /// Gets or sets the bug item status mapping.
    /// </summary>
    /// <value>
    /// The bug item status mapping.
    /// </value>
    public virtual DbSet<BugItemStatusMapping> BugItemStatusMapping { get; set; }

    /// <summary>
    /// Gets or sets the bug severity mapping.
    /// </summary>
    /// <value>
    /// The bug severity mapping.
    /// </value>
    public virtual DbSet<BugSeverityMapping> BugSeverityMapping { get; set; }
}
