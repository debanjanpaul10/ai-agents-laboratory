using AIAgents.Laboratory.API.Adapters.Models.Base;

namespace AIAgents.Laboratory.API.Adapters.Models.Response;

public sealed record RegisteredApplicationDto : BaseModelDto
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
