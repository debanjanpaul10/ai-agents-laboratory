using AIAgents.Laboratory.API.Adapters.Models.Base;

namespace AIAgents.Laboratory.API.Adapters.Models.Response;

/// <summary>
/// The New Feature Request Data DTO Class.
/// </summary>
/// <seealso cref="BaseModelDTO"/>
public sealed record NewFeatureRequestDataDto : BaseModelDTO
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public int Id { get; set; }

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
}
