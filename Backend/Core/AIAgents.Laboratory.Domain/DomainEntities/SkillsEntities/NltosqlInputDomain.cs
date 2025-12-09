namespace AIAgents.Laboratory.Domain.DomainEntities.SkillsEntities;

/// <summary>
/// The NL to SQL input domain.
/// </summary>
/// <seealso cref="AIAgents.Laboratory.Domain.DomainEntities.SkillsEntities.SkillsInputDomain" />
public sealed record NltosqlInputDomain : SkillsInputDomain
{
    /// <summary>
    /// Gets or sets the database schema.
    /// </summary>
    /// <value>
    /// The database schema.
    /// </value>
    public string DatabaseSchema { get; set; } = string.Empty;
}
