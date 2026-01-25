namespace AIAgents.Laboratory.API.Adapters.Models.Base;

/// <summary>
/// The Base Model DTO.
/// </summary>
public record BaseModelDTO
{
    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    /// <value>
    /// The date created.
    /// </value>
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the created by.
    /// </summary>
    /// <value>
    /// The created by.
    /// </value>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date modified.
    /// </summary>
    /// <value>
    /// The date modified.
    /// </value>
    public DateTime DateModified { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the modified by.
    /// </summary>
    /// <value>
    /// The modified by.
    /// </value>
    public string ModifiedBy { get; set; } = string.Empty;
}
