using AIAgents.Laboratory.API.Adapters.Models.Response;

namespace AIAgents.Laboratory.API.Adapters.Contracts;

/// <summary>
/// Provides an interface for handling registered application related operations in the API layer, such as retrieving, creating, updating, and deleting registered applications for the current logged in user.
/// </summary>
public interface IRegisteredApplicationHandler
{
    /// <summary>
    /// Gets the list of registered applications for the current logged in user.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <returns>The list of <see cref="RegisteredApplicationDto"/></returns>
    Task<IEnumerable<RegisteredApplicationDto>> GetRegisteredApplicationsAsync(string currentLoggedInUser);

    /// <summary>
    /// Gets the details of a specific registered application by its ID for the current logged in user.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="applicationId">The application id to be searched for.</param>
    /// <returns>The registered application data model.</returns>
    Task<RegisteredApplicationDto> GetRegisteredApplicationByIdAsync(string currentLoggedInUser, int applicationId);

    /// <summary>
    /// Creates a new registered application for the current logged in user with the provided application data.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="newApplicationData">The new application creation data model.</param>
    /// <returns>A boolean for success/failure.</returns>
    Task<bool> CreateNewRegisteredApplicationAsync(string currentLoggedInUser, RegisteredApplicationDto newApplicationData);

    /// <summary>
    /// Updates an existing registered application for the current logged in user with the provided application data. 
    /// </summary>
    /// <remarks>The application to be updated is identified by the Id property of the provided application data model.</remarks>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="updateApplicationData">The update application data model.</param>
    /// <returns>A boolean for success/failure.</returns>
    Task<bool> UpdateExistingRegisteredApplicationAsync(string currentLoggedInUser, RegisteredApplicationDto updateApplicationData);

    /// <summary>
    /// Deletes a registered application by its ID for the current logged in user. 
    /// </summary>
    /// <remarks>The deletion is performed as a soft delete, where the IsActive property of the application is set to false instead of physically removing the record from the database.</remarks>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="applicationId">The application id for which data is to be deleted.</param>
    /// <returns>A boolean for success/failure.</returns>
    Task<bool> DeleteRegisteredApplicationByIdAsync(string currentLoggedInUser, int applicationId);
}
