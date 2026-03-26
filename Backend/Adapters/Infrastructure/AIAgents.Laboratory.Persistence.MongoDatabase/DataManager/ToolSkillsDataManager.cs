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
/// The <c>ToolSkillsDataManager</c> class provides methods for managing tool skills data in a MongoDB database.
/// </summary>
/// <remarks>
/// It implements the <see cref="IToolSkillsDataManager"/> interface, allowing for operations such as creating, retrieving, updating, and deleting tool skills.
/// The class uses an instance of <see cref="IMongoDatabaseRepository"/> to interact with the MongoDB database and an <see cref="IMapper"/> for mapping between domain models and data models.
/// </remarks>
/// <param name="configuration">The configuration service.</param>
/// <param name="mapper">The mapper service.</param>
/// <param name="mongoDatabaseRepository">The MongoDB database repository.</param>
/// <seealso cref="IToolSkillsDataManager"/>
public sealed class ToolSkillsDataManager(IConfiguration configuration, IMapper mapper, IMongoDatabaseRepository mongoDatabaseRepository) : IToolSkillsDataManager
{
    /// <summary>
    /// The mongo database name configuration value.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbCollectionConstants.AiAgentsPrimaryDatabase]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The tool skills collection name configuration value.
    /// </summary>
    private readonly string ToolSkillsCollectionName = configuration[MongoDbCollectionConstants.ToolSkillsCollectionName]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// Adds a new tool skill asynchronously.
    /// </summary>
    /// <param name="toolSkillData">The tool skill data domain model.</param>
    /// <param name="userEmail">The user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for <c>success/failure</c></returns>
    public async Task<bool> AddNewToolSkillAsync(ToolSkillDomain toolSkillData, string userEmail, CancellationToken cancellationToken = default)
    {
        var dbInput = mapper.Map<ToolSkillModel>(toolSkillData);
        return await mongoDatabaseRepository.SaveDataAsync(
            data: dbInput,
            databaseName: this.MongoDatabaseName,
            collectionName: this.ToolSkillsCollectionName,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes an existing tool by tool skill id asynchronously.
    /// </summary>
    /// <param name="toolSkillId">The tool skill id to delete.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> DeleteExistingToolSkillBySkillIdAsync(string toolSkillId, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ToolSkillModel>.Filter.Where(tsd => tsd.IsActive && tsd.ToolSkillGuid == toolSkillId);
        var allToolSkills = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.ToolSkillsCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);

        var updateToolSkill = allToolSkills.FirstOrDefault() ?? throw new FileNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);
        if (updateToolSkill.CreatedBy != currentUserEmail)
            throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);

        var updates = new List<UpdateDefinition<ToolSkillModel>>
            {
                Builders<ToolSkillModel>.Update.Set(x => x.IsActive, false),
                Builders<ToolSkillModel>.Update.Set(x => x.DateModified, DateTime.UtcNow),
                Builders<ToolSkillModel>.Update.Set(x => x.ModifiedBy, currentUserEmail)
            };
        var update = Builders<ToolSkillModel>.Update.Combine(updates);
        return await mongoDatabaseRepository.UpdateDataInCollectionAsync(
            filter,
            update,
            databaseName: this.MongoDatabaseName,
            collectionName: this.ToolSkillsCollectionName,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets all the tool skill data asynchronously.
    /// </summary>
    /// <param name="userEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="ToolSkillDomain"/></returns>
    public async Task<IEnumerable<ToolSkillDomain>> GetAllToolSkillsAsync(string userEmail, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ToolSkillModel>.Filter.And(Builders<ToolSkillModel>.Filter.Eq(x => x.IsActive, true));
        var result = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.ToolSkillsCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);
        return mapper.Map<IEnumerable<ToolSkillDomain>>(result);
    }

    /// <summary>
    /// Gets a single tool skill data by its skill id asynchronously.
    /// </summary>
    /// <param name="toolSkillId">The tool skill id to be fetched.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The tool skill domain model.</returns>
    public async Task<ToolSkillDomain> GetToolSkillBySkillIdAsync(string toolSkillId, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ToolSkillModel>.Filter.And(
            Builders<ToolSkillModel>.Filter.Eq(x => x.IsActive, true), Builders<ToolSkillModel>.Filter.Eq(x => x.ToolSkillGuid, toolSkillId));
        var allData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.ToolSkillsCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);

        return mapper.Map<ToolSkillDomain>(allData?.First() ?? throw new FileNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage));
    }

    /// <summary>
    /// Updates an existing tool skill data asynchronously.
    /// </summary>
    /// <param name="updateToolSkillData">The tool skill data domain model.</param>
    /// <param name="currentUserEmail">The user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for <c>success/failure</c></returns>
    public async Task<bool> UpdateExistingToolSkillDataAsync(ToolSkillDomain updateToolSkillData, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        var dbInput = mapper.Map<ToolSkillModel>(updateToolSkillData);
        var filter = Builders<ToolSkillModel>.Filter.And(
            Builders<ToolSkillModel>.Filter.Eq(x => x.IsActive, true),
            Builders<ToolSkillModel>.Filter.Eq(x => x.ToolSkillGuid, dbInput.ToolSkillGuid));

        var toolSkillsData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.ToolSkillsCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);
        var existingToolSkill = toolSkillsData.FirstOrDefault() ?? throw new FileNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);

        if (existingToolSkill.CreatedBy != currentUserEmail)
            throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);

        var updates = new List<UpdateDefinition<ToolSkillModel>>
            {
                Builders<ToolSkillModel>.Update.Set(x => x.AssociatedAgents, dbInput.AssociatedAgents),
                Builders<ToolSkillModel>.Update.Set(x => x.ToolSkillDisplayName, dbInput.ToolSkillDisplayName),
                Builders<ToolSkillModel>.Update.Set(x => x.ToolSkillMcpServerUrl, dbInput.ToolSkillMcpServerUrl),
                Builders<ToolSkillModel>.Update.Set(x => x.ToolSkillTechnicalName, dbInput.ToolSkillTechnicalName),
                Builders<ToolSkillModel>.Update.Set(x => x.DateModified, DateTime.UtcNow),
                Builders<ToolSkillModel>.Update.Set(x => x.ModifiedBy, currentUserEmail),
            };

        var update = Builders<ToolSkillModel>.Update.Combine(updates);
        return await mongoDatabaseRepository.UpdateDataInCollectionAsync(
            filter,
            update,
            databaseName: this.MongoDatabaseName,
            collectionName: this.ToolSkillsCollectionName,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
