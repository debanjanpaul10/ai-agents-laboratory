using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Ports.In;
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
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> CreateNewRegisteredApplicationAsync(string currentLoggedInUser, RegisteredApplicationDto newApplicationData, CancellationToken cancellationToken = default)
    {
        var domainInput = mapper.Map<RegisteredApplication>(newApplicationData);
        return await raService.CreateNewRegisteredApplicationAsync(
            currentLoggedInUser,
            newApplicationData: domainInput,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a registered application by its ID for the current logged in user. 
    /// </summary>
    /// <remarks>The deletion is performed as a soft delete, where the IsActive property of the application is set to false instead of physically removing the record from the database.</remarks>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="applicationId">The application id for which data is to be deleted.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> DeleteRegisteredApplicationByIdAsync(string currentLoggedInUser, int applicationId, CancellationToken cancellationToken = default)
    {
        return await raService.DeleteRegisteredApplicationByIdAsync(
            currentLoggedInUser,
            applicationId,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the details of a specific registered application by its ID for the current logged in user.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="applicationId">The application id to be searched for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The registered application data model.</returns>
    public async Task<RegisteredApplicationDto> GetRegisteredApplicationByIdAsync(string currentLoggedInUser, int applicationId, CancellationToken cancellationToken = default)
    {
        var domainResult = await raService.GetRegisteredApplicationByIdAsync(
            currentLoggedInUser,
            applicationId,
            cancellationToken
        ).ConfigureAwait(false);
        return mapper.Map<RegisteredApplicationDto>(domainResult);
    }

    /// <summary>
    /// Gets the list of registered applications for the current logged in user.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="RegisteredApplicationDto"/></returns>
    public async Task<IEnumerable<RegisteredApplicationDto>> GetRegisteredApplicationsAsync(string currentLoggedInUser, CancellationToken cancellationToken = default)
    {
        var domainResult = await raService.GetRegisteredApplicationsAsync(
            currentLoggedInUser,
            cancellationToken
        ).ConfigureAwait(false);
        return mapper.Map<IEnumerable<RegisteredApplicationDto>>(domainResult);
    }

    /// <summary>
    /// Updates an existing registered application for the current logged in user with the provided application data. 
    /// </summary>
    /// <remarks>The application to be updated is identified by the Id property of the provided application data model.</remarks>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="updateApplicationData">The update application data model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> UpdateExistingRegisteredApplicationAsync(string currentLoggedInUser, RegisteredApplicationDto updateApplicationData, CancellationToken cancellationToken = default)
    {
        var domainInput = mapper.Map<RegisteredApplication>(updateApplicationData);
        return await raService.UpdateExistingRegisteredApplicationAsync(
            currentLoggedInUser,
            updateApplicationData: domainInput,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
