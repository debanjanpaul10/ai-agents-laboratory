using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Persistence.MongoDatabase.Contracts;
using AIAgents.Laboratory.Persistence.MongoDatabase.Mapper;
using AIAgents.Laboratory.Persistence.MongoDatabase.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using static AIAgents.Laboratory.Persistence.MongoDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.MongoDatabase.DataManager;

/// <summary>
/// Provides data management operations for registered applications, implementing the <see cref="IRegisteredApplicationDataManager"/> interface to interact with the MongoDB database for CRUD operations on registered application data.
/// </summary>
/// <remarks>This class serves as the concrete implementation of the data manager for registered applications, utilizing the MongoDB repository to perform database operations and <see cref="MongoDataMapperProfile"/> for mapping between domain models and data models.</remarks>
/// <param name="configuration">The configuration service.</param>
/// <param name="mongoDatabaseRepository">The MongoDB database repository.</param>
/// <seealso cref="IRegisteredApplicationDataManager"/>
public sealed class RegisteredApplicationDataManager(
    IConfiguration configuration,
    IMongoDatabaseRepository mongoDatabaseRepository) : IRegisteredApplicationDataManager
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

    /// <inheritdoc/>
    public async Task<bool> CreateNewRegisteredApplicationAsync(
        string currentLoggedInUser,
        RegisteredApplicationDomain newApplicationData,
        CancellationToken cancellationToken = default
    )
    {
        var dbInput = MongoDataMapperProfile.MapToModel(domain: newApplicationData);
        return await mongoDatabaseRepository.SaveDataAsync(
            data: dbInput,
            databaseName: MongoDatabaseName,
            collectionName: RegisteredApplicationsCollectionName,
            bypassDocumentValidation: false,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteRegisteredApplicationByIdAsync(
        string currentLoggedInUser,
        int applicationId,
        CancellationToken cancellationToken = default
    )
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

    /// <inheritdoc/>
    public async Task<RegisteredApplicationDomain> GetRegisteredApplicationByIdAsync(
        string currentLoggedInUser,
        int applicationId,
        CancellationToken cancellationToken = default
    )
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
        return MongoDataMapperProfile.MapToDomain(model: allData.FirstOrDefault() ?? throw new KeyNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<RegisteredApplicationDomain>> GetRegisteredApplicationsAsync(
        string currentLoggedInUser,
        CancellationToken cancellationToken = default
    )
    {
        var filter = Builders<RegisteredApplicationDataModel>.Filter.And(
            Builders<RegisteredApplicationDataModel>.Filter.Eq(x => x.IsActive, true));
        var result = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.RegisteredApplicationsCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);
        return [.. result.Select(MongoDataMapperProfile.MapToDomain)];
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateExistingRegisteredApplicationAsync(
        string currentLoggedInUser,
        RegisteredApplicationDomain updateApplicationData,
        CancellationToken cancellationToken = default
    )
    {
        var dbInput = MongoDataMapperProfile.MapToModel(domain: updateApplicationData);

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
