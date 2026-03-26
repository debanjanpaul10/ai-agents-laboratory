using AIAgents.Laboratory.Domain.DomainEntities;

namespace AIAgents.Laboratory.Domain.Ports.In;

/// <summary>
/// Defines the contract for the Registered Application Service, which provides operations to manage registered applications for the current logged in user, 
/// including creating, reading, updating, and deleting registered applications. 
/// </summary>
/// <remarks>This service acts as a driving port in the application architecture, orchestrating the business logic and interactions with the underlying data management layer through the use of data managers.</remarks>
public interface IRegisteredApplicationService
{
    /// <summary>
    /// Gets the list of registered applications for the current logged in user.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <returns>The list of <see cref="RegisteredApplicationDomain"/></returns>
    Task<IEnumerable<RegisteredApplicationDomain>> GetRegisteredApplicationsAsync(string currentLoggedInUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the details of a specific registered application by its ID for the current logged in user.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="applicationId">The application id to be searched for.</param>
    /// <returns>The registered application data model.</returns>
    Task<RegisteredApplicationDomain> GetRegisteredApplicationByIdAsync(string currentLoggedInUser, int applicationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new registered application for the current logged in user with the provided application data.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="newApplicationData">The new application creation data model.</param>
    /// <returns>A boolean for success/failure.</returns>
    Task<bool> CreateNewRegisteredApplicationAsync(string currentLoggedInUser, RegisteredApplicationDomain newApplicationData, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing registered application for the current logged in user with the provided application data. 
    /// </summary>
    /// <remarks>The application to be updated is identified by the Id property of the provided application data model.</remarks>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="updateApplicationData">The update application data model.</param>
    /// <returns>A boolean for success/failure.</returns>
    Task<bool> UpdateExistingRegisteredApplicationAsync(string currentLoggedInUser, RegisteredApplicationDomain updateApplicationData, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a registered application by its ID for the current logged in user. 
    /// </summary>
    /// <remarks>The deletion is performed as a soft delete, where the IsActive property of the application is set to false instead of physically removing the record from the database.</remarks>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="applicationId">The application id for which data is to be deleted.</param>
    /// <returns>A boolean for success/failure.</returns>
    Task<bool> DeleteRegisteredApplicationByIdAsync(string currentLoggedInUser, int applicationId, CancellationToken cancellationToken = default);
}
