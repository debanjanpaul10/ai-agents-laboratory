using AIAgents.Laboratory.API.Adapters.Models.Base;

namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The NL to SQL input DTO.
/// </summary>
/// <seealso cref="Base.SkillsInputDto" />
public sealed record NltosqlInputDTO : SkillsInputDto
{
    /// <summary>
    /// Gets or sets the database schema.
    /// </summary>
    /// <value>
    /// The database schema.
    /// </value>
    public string DatabaseSchema { get; set; } = string.Empty;
}
