using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The workspace service implementation.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="mongoDatabaseService">The mongo db service.</param>
public class WorkspacesService(ILogger<WorkspacesService> logger, IConfiguration configuration, IMongoDatabaseService mongoDatabaseService) : IWorkspacesService
{
    /// <summary>
    /// The mongo database name configuration value.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbCollectionConstants.AiAgentsPrimaryDatabase] ?? throw new Exception(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The workspaces collection name configuration value.
    /// </summary>
    private readonly string WorkspacesCollectionName = configuration[MongoDbCollectionConstants.WorkspaceCollectionName] ?? throw new Exception(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// Gets the collection of all available workspaces.
    /// </summary>
    /// <param name="userName">The current logged in user name.</param>
    /// <returns>The list of <see cref="AgentsWorkspaceDomain"/></returns>
    public async Task<IEnumerable<AgentsWorkspaceDomain>> GetAllWorkspacesAsync(string userName)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllWorkspacesAsync), DateTime.UtcNow, userName);

            var filter = Builders<AgentsWorkspaceDomain>.Filter.And(Builders<AgentsWorkspaceDomain>.Filter.Eq(x => x.IsActive, true));
            return await mongoDatabaseService.GetDataFromCollectionAsync(MongoDatabaseName, WorkspacesCollectionName, filter).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(GetAllWorkspacesAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllWorkspacesAsync), DateTime.UtcNow, userName);
        }
    }
}
