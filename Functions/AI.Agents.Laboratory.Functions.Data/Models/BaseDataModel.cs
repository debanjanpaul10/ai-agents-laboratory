namespace AI.Agents.Laboratory.Functions.Data.Models;

/// <summary>
/// The BaseDataModel class serves as a foundational data model that includes common properties for all data models in the AIAgents Laboratory application, such as IsActive, DateCreated, CreatedBy, DateModified, and ModifiedBy. 
/// This class can be inherited by other data models to ensure consistency and reduce code duplication across the application.
/// </summary>
public record BaseDataModel
{
    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
    /// </value>
    public bool IsActive { get; set; } = true;

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
