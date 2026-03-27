using AIAgents.Laboratory.Domain.DomainEntities.Workspaces;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Persistence.MongoDatabase.Contracts;
using AIAgents.Laboratory.Persistence.MongoDatabase.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using static AIAgents.Laboratory.Persistence.MongoDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.MongoDatabase.DataManager;

/// <summary>
/// The <c>WorkspacesDataManager</c> class provides methods for managing workspace data in a MongoDB database. 
/// </summary>
/// <remarks>
/// It implements the <see cref="IWorkspacesDataManager"/> interface, allowing for operations such as creating, retrieving, updating, and deleting workspaces. 
/// The class uses an instance of <see cref="IMongoDatabaseRepository"/> to interact with the MongoDB database and an <see cref="IMapper"/> for mapping between domain models and data models.
/// </remarks>
/// <param name="configuration">The configuration instance.</param>
/// <param name="mapper">The mapper instance.</param>
/// <param name="mongoDatabaseRepository">The MongoDB database repository instance.</param>
/// <seealso cref="IWorkspacesDataManager"/>
public sealed class WorkspacesDataManager(IConfiguration configuration, IMapper mapper, IMongoDatabaseRepository mongoDatabaseRepository) : IWorkspacesDataManager
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

    /// <summary>
    /// Creates a new workspace.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data.</param>
    /// <param name="currentUserEmail">The current user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> CreateNewWorkspaceAsync(AgentsWorkspaceDomain agentsWorkspaceData, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        var dbInput = mapper.Map<AgentsWorkspaceDataModel>(agentsWorkspaceData);
        return await mongoDatabaseRepository.SaveDataAsync(
            data: dbInput,
            databaseName: this.MongoDatabaseName,
            collectionName: this.WorkspacesCollectionName,
            cancellationToken
        ).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes the existing workspace by workspace guid id.
    /// </summary>
    /// <param name="workspaceGuidId">The workspace guid id.</param>
    /// <param name="currentUserEmail">The current logged in user email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> DeleteExistingWorkspaceAsync(string workspaceGuidId, string currentUserEmail, CancellationToken cancellationToken = default)
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

    /// <summary>
    /// Gets the collection of all available workspaces.
    /// </summary>
    /// <param name="currentUserEmail">The current logged in user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of <see cref="AgentsWorkspaceDomain"/></returns>
    public async Task<IEnumerable<AgentsWorkspaceDomain>> GetAllWorkspacesAsync(string currentUserEmail, CancellationToken cancellationToken = default)
    {
        var filter = Builders<AgentsWorkspaceDataModel>.Filter.And(Builders<AgentsWorkspaceDataModel>.Filter.Eq(x => x.IsActive, true));
        var result = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.WorkspacesCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);
        return mapper.Map<IEnumerable<AgentsWorkspaceDomain>>(result);
    }

    /// <summary>
    /// Gets the workspace by workspace id.
    /// </summary>
    /// <param name="workspaceId">The workspace id.</param>
    /// <param name="currentUserEmail">The current logged in user email</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The agent workspace domain model.</returns>
    public async Task<AgentsWorkspaceDomain> GetWorkspaceByWorkspaceIdAsync(string workspaceId, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        var filter = Builders<AgentsWorkspaceDataModel>.Filter.And(
            Builders<AgentsWorkspaceDataModel>.Filter.Eq(x => x.IsActive, true),
            Builders<AgentsWorkspaceDataModel>.Filter.Eq(x => x.AgentWorkspaceGuid, workspaceId));

        var allData = await mongoDatabaseRepository.GetDataFromCollectionAsync(
            databaseName: this.MongoDatabaseName,
            collectionName: this.WorkspacesCollectionName,
            filter,
            cancellationToken
        ).ConfigureAwait(false);
        return mapper.Map<AgentsWorkspaceDomain>(allData?.First() ?? throw new FileNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage));
    }

    /// <summary>
    /// Updates the existing workspace data.
    /// </summary>
    /// <param name="agentsWorkspaceData">The agents workspace data domain model.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> UpdateExistingWorkspaceDataAsync(AgentsWorkspaceDomain agentsWorkspaceData, string currentUserEmail, CancellationToken cancellationToken = default)
    {
        var dbInput = mapper.Map<AgentsWorkspaceDataModel>(agentsWorkspaceData);
        var filter = Builders<AgentsWorkspaceDataModel>.Filter.And(
            Builders<AgentsWorkspaceDataModel>.Filter.Eq(x => x.IsActive, true),
            Builders<AgentsWorkspaceDataModel>.Filter.Eq(x => x.AgentWorkspaceGuid, agentsWorkspaceData.AgentWorkspaceGuid));

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
}
