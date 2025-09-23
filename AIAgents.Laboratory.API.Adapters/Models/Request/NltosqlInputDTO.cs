namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The NL to SQL input DTO.
/// </summary>
/// <seealso cref="AIAgents.Laboratory.API.Adapters.Models.Request.SkillsInputDTO" />
public class NltosqlInputDTO : SkillsInputDTO
{
	/// <summary>
	/// Gets or sets the database schema.
	/// </summary>
	/// <value>
	/// The database schema.
	/// </value>
	public string DatabaseSchema { get; set; } = string.Empty;
}
