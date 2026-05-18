using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
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
/// The workspaces data manager implementation for managing workspace related data operations with MongoDB as the underlying database.
/// </summary>
/// <param name="configuration">The configuration instance to access application settings.</param>
/// <param name="correlationContext">The correlation context for logging and tracing.</param>
/// <param name="logger">The logger instance for logging information and errors.</param>
/// <param name="mongoDatabaseRepository">The mongo database repository for performing database operations.</param>
/// <seealso cref="IWorkspacesDataManager" />
public sealed class WorkspacesDataManager(
    ILogger<WorkspacesDataManager> logger,
    ICorrelationContext correlationContext,
    IConfiguration configuration,
    IMongoDatabaseRepository mongoDatabaseRepository) : IWorkspacesDataManager
{
    /// <summary>
    /// The mongo database name configuration value.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbCollectionConstants.AiAgentsPrimaryDatabase]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The workspaces collection name configuration value.
    /// </summary>
    private readonly string WorkspacesCollectionName = configuration[MongoDbCollectionConstants.WorkspaceCollectionName]
        ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <inheritdoc />
    public async Task<bool> CreateNewWorkspaceAsync(
        AgentsWorkspaceDomain agentsWorkspaceData,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        var dbInput = MongoDataMapperProfile.MapToModel(domain: agentsWorkspaceData);
        return await mongoDatabaseRepository.SaveDataAsync(
            data: dbInput,
            databaseName: this.MongoDatabaseName,
            collectionName: this.WorkspacesCollectionName,
            bypassDocumentValidation: true,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteExistingWorkspaceAsync(
        string workspaceGuidId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        var filter = Builders<AgentsWorkspaceDataModel>.Filter.Where(ws => ws.IsActive && ws.AgentWorkspaceGuid == workspaceGuidId);
        var allWorkspaces = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.WorkspacesCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);
        var updateWorkspace = allWorkspaces.FirstOrDefault() ?? throw new FileNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);

        if (updateWorkspace.CreatedBy != currentUserEmail)
            throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);

        var updates = new List<UpdateDefinition<AgentsWorkspaceDataModel>>
        {
            Builders<AgentsWorkspaceDataModel>.Update.Set(x => x.IsActive, false),
            Builders<AgentsWorkspaceDataModel>.Update.Set(x => x.DateModified, DateTime.UtcNow),
            Builders<AgentsWorkspaceDataModel>.Update.Set(x => x.ModifiedBy, currentUserEmail)
        };
        var update = Builders<AgentsWorkspaceDataModel>.Update.Combine(updates);
        return await mongoDatabaseRepository.UpdateDataInCollectionAsync(
            filter,
            update,
            databaseName: this.MongoDatabaseName,
            collectionName: this.WorkspacesCollectionName,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AgentsWorkspaceDomain>> GetAllWorkspacesAsync(
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        var filter = Builders<AgentsWorkspaceDataModel>.Filter.And(Builders<AgentsWorkspaceDataModel>.Filter.Eq(x => x.IsActive, true));
        var result = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.WorkspacesCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);
        return [.. result.Select(MongoDataMapperProfile.MapToDomain)];
    }

    /// <inheritdoc />
    public async Task<AgentsWorkspaceDomain> GetWorkspaceByWorkspaceIdAsync(
        string workspaceId,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        var filter = Builders<AgentsWorkspaceDataModel>.Filter.And(
            Builders<AgentsWorkspaceDataModel>.Filter.Eq(x => x.IsActive, true),
            Builders<AgentsWorkspaceDataModel>.Filter.Eq(x => x.AgentWorkspaceGuid, workspaceId)
        );

        var allData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.WorkspacesCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);
        return MongoDataMapperProfile.MapToDomain(model: allData?.First() ?? throw new FileNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage));
    }

    /// <inheritdoc />
    public async Task<bool> UpdateExistingWorkspaceDataAsync(
        AgentsWorkspaceDomain agentsWorkspaceData,
        string currentUserEmail,
        CancellationToken cancellationToken = default
    )
    {
        bool response = false;
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(UpdateExistingWorkspaceDataAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { agentsWorkspaceData, currentUserEmail, correlationContext.CorrelationId })
            );

            var dbInput = MongoDataMapperProfile.MapToModel(domain: agentsWorkspaceData);
            var filter = Builders<AgentsWorkspaceDataModel>.Filter.And(
                Builders<AgentsWorkspaceDataModel>.Filter.Eq(x => x.IsActive, true),
                Builders<AgentsWorkspaceDataModel>.Filter.Eq(x => x.AgentWorkspaceGuid, agentsWorkspaceData.AgentWorkspaceGuid)
            );

            var allWorkspacesData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
                databaseName: this.MongoDatabaseName,
                collectionName: this.WorkspacesCollectionName,
                filter,
                cancellationToken
            ).ConfigureAwait(false);

            var existingWorkspaceData = allWorkspacesData.FirstOrDefault() ?? throw new FileNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);
            if (!existingWorkspaceData.WorkspaceUsers.Contains(currentUserEmail))
                throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);

            var updates = new List<UpdateDefinition<AgentsWorkspaceDataModel>>
        {
            Builders<AgentsWorkspaceDataModel>.Update.Set(x => x.AgentWorkspaceName, dbInput.AgentWorkspaceName),
            Builders<AgentsWorkspaceDataModel>.Update.Set(x => x.ActiveAgentsListInWorkspace, dbInput.ActiveAgentsListInWorkspace),
            Builders<AgentsWorkspaceDataModel>.Update.Set(x => x.WorkspaceUsers, dbInput.WorkspaceUsers),
            Builders<AgentsWorkspaceDataModel>.Update.Set(x => x.DateModified, DateTime.UtcNow),
            Builders<AgentsWorkspaceDataModel>.Update.Set(x => x.ModifiedBy, currentUserEmail),
            Builders<AgentsWorkspaceDataModel>.Update.Set(x => x.IsGroupChatEnabled, dbInput.IsGroupChatEnabled)
        };
            var update = Builders<AgentsWorkspaceDataModel>.Update.Combine(updates);
            response = await mongoDatabaseRepository.UpdateDataInCollectionAsync(
                filter,
                update,
                databaseName: this.MongoDatabaseName,
                collectionName: this.WorkspacesCollectionName,
                cancellationToken
            ).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(UpdateExistingWorkspaceDataAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException
            (
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggingConstants.MethodEndedMessageConstant,
                nameof(UpdateExistingWorkspaceDataAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { agentsWorkspaceData, currentUserEmail, correlationContext.CorrelationId, response })
            );
        }
    }
}
