namespace AIAgents.Laboratory.API.Adapters.Models.Request;

/// <summary>
/// The New Feature Request Data DTO.
/// </summary>
public class NewFeatureRequestDTO
{
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
    /// Gets or sets the created by.
    /// </summary>
    /// <value>
    /// The created by.
    /// </value>
    public string CreatedBy { get; set; } = string.Empty;
}
