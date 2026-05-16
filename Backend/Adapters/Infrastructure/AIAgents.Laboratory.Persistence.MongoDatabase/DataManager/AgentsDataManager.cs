using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities.AgentsEntities;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Persistence.MongoDatabase.Contracts;
using AIAgents.Laboratory.Persistence.MongoDatabase.Mapper;
using AIAgents.Laboratory.Persistence.MongoDatabase.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Persistence.MongoDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.MongoDatabase.DataManager;

/// <summary>
/// Provides implementation for the <see cref="IAgentsDataManager"/> to manage agent data in a MongoDB database, including creating, retrieving, updating, and deleting agent data while ensuring proper access control based on user email and privacy settings. 
/// </summary>
/// <remarks>
/// The service ensures that only authorized users can access or modify agent data based on the created by email and privacy settings, and it abstracts the MongoDB data access logic from the domain layer.
/// This service uses <see cref="MongoDataMapperProfile"/> for mapping between domain entities and database models, and relies on a MongoDB repository for data access operations.
/// </remarks>
/// <param name="logger">The logger used to record diagnostic and operational information for the service.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="correlationContext">The correlation context used to track and correlate logs and operations across different components and services during agent data management operations.</param>
/// <param name="mongoDatabaseRepository">The mongodb repository.</param>
/// <seealso cref="IAgentsDataManager"/>
public sealed class AgentsDataManager(
    ILogger<AgentsDataManager> logger,
    IConfiguration configuration,
    ICorrelationContext correlationContext,
    IMongoDatabaseRepository mongoDatabaseRepository) : IAgentsDataManager
{
    /// <summary>
    /// The mongo database name configuration value.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbCollectionConstants.AiAgentsPrimaryDatabase]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The agents data collection name configuration value.
    /// </summary>
    private readonly string AgentsDataCollectionName = configuration[MongoDbCollectionConstants.AgentsCollectionName]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// Creates the new agent asynchronous.
    /// </summary>
    /// <param name="agentData">The agent data.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> CreateNewAgentAsync(AgentDataDomain agentData, string userEmail, CancellationToken cancellationToken = default)
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(CreateNewAgentAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userEmail, agentData.AgentName })
            );
            var dbInput = MongoDataMapperProfile.MapToModel(domain: agentData);
            response = await mongoDatabaseRepository.SaveDataAsync(
                data: dbInput,
                databaseName: this.MongoDatabaseName,
                collectionName: this.AgentsDataCollectionName,
                bypassDocumentValidation: false,
                cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(CreateNewAgentAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(CreateNewAgentAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, userEmail, agentData.AgentName, response })
            );
        }
    }

    /// <summary>
    /// Deletes an existing agent data.
    /// </summary>
    /// <param name="agentId">The agent id.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> DeleteExistingAgentDataAsync(string agentId, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        var filter = Builders<AgentDataModel>.Filter.Where(x => x.IsActive && x.AgentId == agentId);
        var allAgents = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.AgentsDataCollectionName,
            filter: filter,
            cancellationToken
        ).ConfigureAwait(false);

        var updateAgent = allAgents.FirstOrDefault() ?? throw new KeyNotFoundException(ExceptionConstants.AgentNotFoundExceptionMessage);
        if (updateAgent.CreatedBy != currentUserEmail)
            throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);

        var updates = new List<UpdateDefinition<AgentDataModel>>
        {
            Builders<AgentDataModel>.Update.Set(x => x.IsActive, false),
            Builders<AgentDataModel>.Update.Set(x => x.DateModified, DateTime.UtcNow)
        };
        var update = Builders<AgentDataModel>.Update.Combine(updates);

        return await mongoDatabaseRepository.UpdateDataInCollectionAsync(
            filter,
            update,
            databaseName: this.MongoDatabaseName,
            collectionName: this.AgentsDataCollectionName,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the agent data by identifier asynchronous.
    /// </summary>
    /// <param name="agentId">The agent identifier.</param>
    /// <param name="userEmail">The user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The agent data dto.</returns>
    public async Task<AgentDataDomain> GetAgentDataByIdAsync(string agentId, string userEmail, CancellationToken cancellationToken = default)
    {
        var filter = Builders<AgentDataModel>.Filter.And(
            Builders<AgentDataModel>.Filter.Eq(x => x.IsActive, true),
            Builders<AgentDataModel>.Filter.Eq(x => x.AgentId, agentId)
        );

        if (!string.IsNullOrEmpty(userEmail))
            filter = Builders<AgentDataModel>.Filter.And(
                filter,
                Builders<AgentDataModel>.Filter.Or(
                    Builders<AgentDataModel>.Filter.Eq(x => x.IsPrivate, false),
                    Builders<AgentDataModel>.Filter.And(
                        Builders<AgentDataModel>.Filter.Eq(x => x.IsPrivate, true),
                        Builders<AgentDataModel>.Filter.Eq(x => x.CreatedBy, userEmail)
                    )
                )
            );

        var allData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.AgentsDataCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);
        return MongoDataMapperProfile.MapToDomain(model: allData.First());
    }

    /// <summary>
    /// Gets all agents data asynchronous.
    /// </summary>
    /// <param name="userEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="AgentDataDomain"/></returns>
    public async Task<IEnumerable<AgentDataDomain>> GetAllAgentsDataAsync(string userEmail, CancellationToken cancellationToken = default)
    {
        var filter = Builders<AgentDataModel>.Filter.And(
            Builders<AgentDataModel>.Filter.Eq(x => x.IsActive, true),
            Builders<AgentDataModel>.Filter.Eq(x => x.IsDefaultChatbot, false),
            Builders<AgentDataModel>.Filter.Or(
                Builders<AgentDataModel>.Filter.Eq(x => x.IsPrivate, false),
                Builders<AgentDataModel>.Filter.And(
                    Builders<AgentDataModel>.Filter.Eq(x => x.IsPrivate, true),
                    Builders<AgentDataModel>.Filter.Eq(x => x.CreatedBy, userEmail)
                )
            )
        );

        var dbResult = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.AgentsDataCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);
        return [.. dbResult.Select(MongoDataMapperProfile.MapToDomain)];
    }

    /// <summary>
    /// Updates the existing agent data.
    /// </summary>
    /// <param name="updateDataDomain">The update agent data domain model.</param>
    /// <param name="userEmail">The current logged in user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> UpdateExistingAgentDataAsync(AgentDataDomain updateDataDomain, string userEmail, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(updateDataDomain);
        ArgumentException.ThrowIfNullOrWhiteSpace(updateDataDomain.AgentId);
        ArgumentException.ThrowIfNullOrWhiteSpace(userEmail);

        // Ownership + active constraint: only the creator can update their agent, and only while active.
        var dbFilterInput = Builders<AgentDataModel>.Filter.And(
            Builders<AgentDataModel>.Filter.Eq(x => x.IsActive, true),
            Builders<AgentDataModel>.Filter.Eq(x => x.AgentId, updateDataDomain.AgentId),
            Builders<AgentDataModel>.Filter.Eq(x => x.CreatedBy, userEmail)
        );

        var updates = new List<UpdateDefinition<AgentDataModel>>
        {
            Builders<AgentDataModel>.Update.Set(x => x.AgentMetaPrompt, updateDataDomain.AgentMetaPrompt),
            Builders<AgentDataModel>.Update.Set(x => x.AgentName, updateDataDomain.AgentName),
            Builders<AgentDataModel>.Update.Set(x => x.ApplicationId, updateDataDomain.ApplicationId),
            Builders<AgentDataModel>.Update.Set(x => x.IsPrivate, updateDataDomain.IsPrivate),
            Builders<AgentDataModel>.Update.Set(x => x.AgentDescription, updateDataDomain.AgentDescription),
            Builders<AgentDataModel>.Update.Set(x => x.AssociatedSkillGuids, updateDataDomain.AssociatedSkillGuids),
            Builders<AgentDataModel>.Update.Set(x => x.DateModified, DateTime.UtcNow),
            Builders<AgentDataModel>.Update.Set(x => x.ModifiedBy, userEmail),
        };

        var hasKnowledgeBaseUpdate =
            (updateDataDomain.RemovedKnowledgeBaseDocuments is not null && updateDataDomain.RemovedKnowledgeBaseDocuments.Any())
            || (updateDataDomain.KnowledgeBaseDocument is not null && updateDataDomain.KnowledgeBaseDocument.Any());

        if (hasKnowledgeBaseUpdate)
            updates.Add(Builders<AgentDataModel>.Update.Set(x => x.StoredKnowledgeBase, updateDataDomain.StoredKnowledgeBase));

        var hasVisionUpdate =
            (updateDataDomain.RemovedAiVisionImages is not null && updateDataDomain.RemovedAiVisionImages.Any())
            || (updateDataDomain.VisionImages is not null && updateDataDomain.VisionImages.Any());

        if (hasVisionUpdate)
            updates.Add(Builders<AgentDataModel>.Update.Set(x => x.AiVisionImagesData, updateDataDomain.AiVisionImagesData));

        var dbInput = Builders<AgentDataModel>.Update.Combine(updates);
        return await mongoDatabaseRepository.UpdateDataInCollectionAsync(
            filter: dbFilterInput,
            update: dbInput,
            databaseName: this.MongoDatabaseName,
            collectionName: this.AgentsDataCollectionName,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
