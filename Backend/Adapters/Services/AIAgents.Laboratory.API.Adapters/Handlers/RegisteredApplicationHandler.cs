using AIAgents.Laboratory.API.Adapters.Contracts;
using AIAgents.Laboratory.API.Adapters.Models.Response;
using AIAgents.Laboratory.Domain.Ports.In;
using static AIAgents.Laboratory.API.Adapters.Mapper.DomainToResponseMapper;
using static AIAgents.Laboratory.API.Adapters.Mapper.RequestToDomainMapper;

namespace AIAgents.Laboratory.API.Adapters.Handlers;

/// <summary>
/// Provides the implementation for handling registered application related operations in the API layer, such as retrieving, creating, updating, and deleting registered applications for the current logged in user.
/// </summary>
/// <param name="raService">The registered application services.</param>
/// <seealso cref="IRegisteredApplicationHandler"/>
public sealed class RegisteredApplicationHandler(
    IRegisteredApplicationService raService) : IRegisteredApplicationHandler
{
    /// <inheritdoc/>
    public async Task<bool> CreateNewRegisteredApplicationAsync(
        string currentLoggedInUser,
        RegisteredApplicationDto newApplicationData,
        CancellationToken cancellationToken = default
    )
    {
        var domainInput = MapToDomain(newApplicationData);
        return await raService.CreateNewRegisteredApplicationAsync(
            currentLoggedInUser,
            newApplicationData: domainInput,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteRegisteredApplicationByIdAsync(
        string currentLoggedInUser,
        int applicationId,
        CancellationToken cancellationToken = default
    ) =>
        await raService.DeleteRegisteredApplicationByIdAsync(
            currentLoggedInUser,
            applicationId,
            cancellationToken
        ).ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<RegisteredApplicationDto> GetRegisteredApplicationByIdAsync(
        string currentLoggedInUser,
        int applicationId,
        CancellationToken cancellationToken = default
    )
    {
        var domainResult = await raService.GetRegisteredApplicationByIdAsync(
            currentLoggedInUser,
            applicationId,
            cancellationToken
        ).ConfigureAwait(false);
        return MapToDto(domainResult);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<RegisteredApplicationDto>> GetRegisteredApplicationsAsync(
        string currentLoggedInUser,
        CancellationToken cancellationToken = default
    )
    {
        var domainResult = await raService.GetRegisteredApplicationsAsync(
            currentLoggedInUser,
            cancellationToken
        ).ConfigureAwait(false);
        return [.. domainResult.Select(MapToDto)];
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateExistingRegisteredApplicationAsync(
        string currentLoggedInUser,
        RegisteredApplicationDto updateApplicationData,
        CancellationToken cancellationToken = default
    )
    {
        var domainInput = MapToDomain(updateApplicationData);
        return await raService.UpdateExistingRegisteredApplicationAsync(
            currentLoggedInUser,
            updateApplicationData: domainInput,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
