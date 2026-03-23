namespace AIAgents.Laboratory.Persistence.SQLDatabase.Models;

/// <summary>
/// The registered application entity class representing the registered applications in the system.
/// </summary>
public sealed record RegisteredApplicationEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the registered application.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the registered application.
    /// </summary>
    public string ApplicationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the registered application.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the application registration GUID, which is used to identify the application in Azure Active Directory.
    /// </summary>
    public Guid? ApplicationRegistrationGuid { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the application is registered in Azure Active Directory.
    /// </summary>
    public bool IsAzureRegistered { get; set; } = false;
}
