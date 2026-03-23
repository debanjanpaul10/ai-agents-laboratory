using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AutoMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// Provides the implementation for handling registered application related operations in the API layer, such as retrieving, creating, updating, and deleting registered applications for the current logged in user.
/// </summary>
/// <param name="mapper">The auto mapper service.</param>
/// <param name="raService">The registered application services.</param>
/// <seealso cref="IRegisteredApplicationHandler"/>
public sealed class RegisteredApplicationHandler(IMapper mapper, IRegisteredApplicationService raService) : IRegisteredApplicationHandler
{
    /// <summary>
    /// Creates a new registered application for the current logged in user with the provided application data.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="newApplicationData">The new application creation data model.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> CreateNewRegisteredApplicationAsync(string currentLoggedInUser, RegisteredApplicationDto newApplicationData)
    {
        var domainInput = mapper.Map<RegisteredApplication>(newApplicationData);
        return await raService.CreateNewRegisteredApplicationAsync(currentLoggedInUser, newApplicationData: domainInput).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a registered application by its ID for the current logged in user. 
    /// </summary>
    /// <remarks>The deletion is performed as a soft delete, where the IsActive property of the application is set to false instead of physically removing the record from the database.</remarks>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="applicationId">The application id for which data is to be deleted.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> DeleteRegisteredApplicationByIdAsync(string currentLoggedInUser, int applicationId)
    {
        return await raService.DeleteRegisteredApplicationByIdAsync(currentLoggedInUser, applicationId).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the details of a specific registered application by its ID for the current logged in user.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="applicationId">The application id to be searched for.</param>
    /// <returns>The registered application data model.</returns>
    public async Task<RegisteredApplicationDto> GetRegisteredApplicationByIdAsync(string currentLoggedInUser, int applicationId)
    {
        var domainResult = await raService.GetRegisteredApplicationByIdAsync(currentLoggedInUser, applicationId).ConfigureAwait(false);
        return mapper.Map<RegisteredApplicationDto>(domainResult);
    }

    /// <summary>
    /// Gets the list of registered applications for the current logged in user.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <returns>The list of <see cref="RegisteredApplicationDto"/></returns>
    public async Task<IEnumerable<RegisteredApplicationDto>> GetRegisteredApplicationsAsync(string currentLoggedInUser)
    {
        var domainResult = await raService.GetRegisteredApplicationsAsync(currentLoggedInUser).ConfigureAwait(false);
        return mapper.Map<IEnumerable<RegisteredApplicationDto>>(domainResult);
    }

    /// <summary>
    /// Updates an existing registered application for the current logged in user with the provided application data. 
    /// </summary>
    /// <remarks>The application to be updated is identified by the Id property of the provided application data model.</remarks>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="updateApplicationData">The update application data model.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> UpdateExistingRegisteredApplicationAsync(string currentLoggedInUser, RegisteredApplicationDto updateApplicationData)
    {
        var domainInput = mapper.Map<RegisteredApplication>(updateApplicationData);
        return await raService.UpdateExistingRegisteredApplicationAsync(currentLoggedInUser, updateApplicationData: domainInput).ConfigureAwait(false);
    }
}
