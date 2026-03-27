using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Persistence.MongoDatabase.Contracts;
using AIAgents.Laboratory.Persistence.MongoDatabase.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using static AIAgents.Laboratory.Persistence.MongoDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.MongoDatabase.DataManager;

/// <summary>
/// Provides data management operations for registered applications, implementing the <see cref="IRegisteredApplicationDataManager"/> interface to interact with the MongoDB database for CRUD operations on registered application data.
/// </summary>
/// <remarks>This class serves as the concrete implementation of the data manager for registered applications, utilizing the MongoDB repository to perform database operations and AutoMapper for mapping between domain models and data models.</remarks>
/// <param name="configuration">The configuration service.</param>
/// <param name="mapper">The AutoMapper service.</param>
/// <param name="mongoDatabaseRepository">The MongoDB database repository.</param>
/// <seealso cref="IRegisteredApplicationDataManager"/>
public sealed class RegisteredApplicationDataManager(IConfiguration configuration, IMapper mapper, IMongoDatabaseRepository mongoDatabaseRepository) : IRegisteredApplicationDataManager
{
    /// <summary>
    /// The mongo database name configuration value.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbCollectionConstants.AiAgentsPrimaryDatabase]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The agents data collection name configuration value.
    /// </summary>
    private readonly string RegisteredApplicationsCollectionName = configuration[MongoDbCollectionConstants.RegisteredApplicationsCollectionName]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// Creates a new registered application for the current logged in user with the provided application data.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="newApplicationData">The new application creation data model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> CreateNewRegisteredApplicationAsync(string currentLoggedInUser, RegisteredApplicationDomain newApplicationData, CancellationToken cancellationToken = default)
    {
        var dbInput = mapper.Map<RegisteredApplicationDataModel>(newApplicationData);
        return await mongoDatabaseRepository.SaveDataAsync(
            data: dbInput,
            databaseName: MongoDatabaseName,
            collectionName: RegisteredApplicationsCollectionName,
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
        var filter = Builders<RegisteredApplicationDataModel>.Filter.Where(item => item.IsActive && item.Id == applicationId);
        var allApplications = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.RegisteredApplicationsCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);
        var updateApplication = allApplications.FirstOrDefault() ?? throw new KeyNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);

        if (updateApplication.CreatedBy != currentLoggedInUser)
            throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);

        var updates = new List<UpdateDefinition<RegisteredApplicationDataModel>>
        {
            Builders<RegisteredApplicationDataModel>.Update.Set(x => x.IsActive, false),
            Builders<RegisteredApplicationDataModel>.Update.Set(x => x.DateModified, DateTime.UtcNow)
        };
        var update = Builders<RegisteredApplicationDataModel>.Update.Combine(updates);
        return await mongoDatabaseRepository.UpdateDataInCollectionAsync(
            filter,
            update,
            databaseName: this.MongoDatabaseName,
            collectionName: this.RegisteredApplicationsCollectionName,
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
    public async Task<RegisteredApplicationDomain> GetRegisteredApplicationByIdAsync(string currentLoggedInUser, int applicationId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<RegisteredApplicationDataModel>.Filter.And(
            Builders<RegisteredApplicationDataModel>.Filter.Eq(x => x.IsActive, true),
            Builders<RegisteredApplicationDataModel>.Filter.Eq(x => x.Id, applicationId));

        var allData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.RegisteredApplicationsCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);
        return mapper.Map<RegisteredApplicationDomain>(allData.FirstOrDefault()) ?? throw new KeyNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);
    }

    /// <summary>
    /// Gets the list of registered applications for the current logged in user.
    /// </summary>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="RegisteredApplicationDomain"/></returns>
    public async Task<IEnumerable<RegisteredApplicationDomain>> GetRegisteredApplicationsAsync(string currentLoggedInUser, CancellationToken cancellationToken = default)
    {
        var filter = Builders<RegisteredApplicationDataModel>.Filter.And(
            Builders<RegisteredApplicationDataModel>.Filter.Eq(x => x.IsActive, true));
        var result = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.RegisteredApplicationsCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);
        return mapper.Map<IEnumerable<RegisteredApplicationDomain>>(result);
    }

    /// <summary>
    /// Updates an existing registered application for the current logged in user with the provided application data. 
    /// </summary>
    /// <remarks>The application to be updated is identified by the Id property of the provided application data model.</remarks>
    /// <param name="currentLoggedInUser">The current logged in user.</param>
    /// <param name="updateApplicationData">The update application data model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> UpdateExistingRegisteredApplicationAsync(string currentLoggedInUser, RegisteredApplicationDomain updateApplicationData, CancellationToken cancellationToken = default)
    {
        var dbInput = mapper.Map<RegisteredApplicationDataModel>(updateApplicationData);

        var filter = Builders<RegisteredApplicationDataModel>.Filter.And(
            Builders<RegisteredApplicationDataModel>.Filter.Eq(x => x.IsActive, true),
            Builders<RegisteredApplicationDataModel>.Filter.Eq(x => x.Id, dbInput.Id));
        var updates = new List<UpdateDefinition<RegisteredApplicationDataModel>>
        {
            Builders<RegisteredApplicationDataModel>.Update.Set(ra => ra.ApplicationName, dbInput.ApplicationName),
            Builders<RegisteredApplicationDataModel>.Update.Set(ra => ra.Description, dbInput.Description),
            Builders<RegisteredApplicationDataModel>.Update.Set(ra => ra.ApplicationRegistrationGuid, dbInput.ApplicationRegistrationGuid),
            Builders<RegisteredApplicationDataModel>.Update.Set(ra => ra.DateModified, DateTime.UtcNow),
            Builders<RegisteredApplicationDataModel>.Update.Set(ra => ra.ModifiedBy, currentLoggedInUser)
        };
        var update = Builders<RegisteredApplicationDataModel>.Update.Combine(updates);
        return await mongoDatabaseRepository.UpdateDataInCollectionAsync(
            filter,
            update,
            databaseName: this.MongoDatabaseName,
            collectionName: this.RegisteredApplicationsCollectionName,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
