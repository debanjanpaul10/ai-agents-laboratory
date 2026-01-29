using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using MongoDB.Driver;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The tool skills service business class.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="mongoDatabaseService">The mongo database service.</param>
/// <param name="configuration">The configuration service.</param>
/// <param name="mcpClientServices">The MCP client services.</param>
/// <seealso cref="IToolSkillsService"/>
public sealed class ToolSkillsService(ILogger<ToolSkillsService> logger, IConfiguration configuration, IMongoDatabaseService mongoDatabaseService, IMcpClientServices mcpClientServices) : IToolSkillsService
{
    /// <summary>
    /// The mongo database name configuration value.
    /// </summary>
    private readonly string MongoDatabaseName = configuration[MongoDbCollectionConstants.AiAgentsPrimaryDatabase] ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// The tool skills collection name configuration value.
    /// </summary>
    private readonly string ToolSkillsCollectionName = configuration[MongoDbCollectionConstants.ToolSkillsCollectionName] ?? throw new KeyNotFoundException(ExceptionConstants.ConfigurationKeyNotFoundExceptionMessage);

    /// <summary>
    /// Adds a new tool skill asynchronously.
    /// </summary>
    /// <param name="toolSkillData">The tool skill data domain model.</param>
    /// <param name="userEmail">The user email.</param>
    /// <returns>The boolean for <c>success/failure</c></returns>
    public async Task<bool> AddNewToolSkillAsync(ToolSkillDomain toolSkillData, string userEmail)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(AddNewToolSkillAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { userEmail, toolSkillData.ToolSkillDisplayName }));

            toolSkillData.ToolSkillGuid = Guid.NewGuid().ToString();
            toolSkillData.PrepareAuditEntityData(userEmail);
            return await mongoDatabaseService.SaveDataAsync(toolSkillData, MongoDatabaseName, ToolSkillsCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(AddNewToolSkillAsync), DateTime.UtcNow, ex.Message);
            return false;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(AddNewToolSkillAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { userEmail, toolSkillData.ToolSkillDisplayName }));
        }
    }

    /// <summary>
    /// Associates a skill and an agent asynchronously.
    /// </summary>
    /// <param name="agentData">The agent data containing agent name and agent guid.</param>
    /// <param name="toolSkillId">The tool skill guid id.</param>
    /// <param name="currentUserEmail">The current user email.</param>
    /// <returns>A boolean for <c>success/failure.</c></returns>
    public async Task<bool> AssociateSkillAndAgentAsync(IList<AssociatedAgentsSkillDataDomain> agentData, string toolSkillId, string currentUserEmail)
    {
        ArgumentNullException.ThrowIfNull(agentData);
        ArgumentException.ThrowIfNullOrWhiteSpace(toolSkillId);
        ArgumentException.ThrowIfNullOrWhiteSpace(currentUserEmail);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(AssociateSkillAndAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { toolSkillId, currentUserEmail }));

            var toolData = await this.GetToolSkillBySkillIdAsync(toolSkillId, currentUserEmail).ConfigureAwait(false);
            if (toolData is null) return false;

            toolData.AssociatedAgents = agentData;
            await this.UpdateExistingToolSkillDataAsync(toolData, currentUserEmail).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(AssociateSkillAndAgentAsync), DateTime.UtcNow, ex.Message);
            return false;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(AssociateSkillAndAgentAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { toolSkillId, currentUserEmail }));
        }
    }

    /// <summary>
    /// Deletes an existing tool by tool skill id asynchronously.
    /// </summary>
    /// <param name="toolSkillId">The tool skill id to delete.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <returns>A boolean for success/failure.</returns>
    public async Task<bool> DeleteExistingToolSkillBySkillIdAsync(string toolSkillId, string currentUserEmail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(toolSkillId);
        ArgumentException.ThrowIfNullOrWhiteSpace(currentUserEmail);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteExistingToolSkillBySkillIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { currentUserEmail, toolSkillId }));

            var filter = Builders<ToolSkillDomain>.Filter.Where(tsd => tsd.IsActive && tsd.ToolSkillGuid == toolSkillId);
            var allToolSkills = await mongoDatabaseService.GetDataFromCollectionAsync(MongoDatabaseName, ToolSkillsCollectionName, filter);
            var updateToolSkill = allToolSkills.FirstOrDefault() ?? throw new FileNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);

            if (updateToolSkill.CreatedBy != currentUserEmail)
                throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);

            var updates = new List<UpdateDefinition<ToolSkillDomain>>
            {
                Builders<ToolSkillDomain>.Update.Set(x => x.IsActive, false),
                Builders<ToolSkillDomain>.Update.Set(x => x.DateModified, DateTime.UtcNow),
                Builders<ToolSkillDomain>.Update.Set(x => x.ModifiedBy, currentUserEmail)
            };
            var update = Builders<ToolSkillDomain>.Update.Combine(updates);
            return await mongoDatabaseService.UpdateDataInCollectionAsync(filter, update, MongoDatabaseName, ToolSkillsCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(DeleteExistingToolSkillBySkillIdAsync), DateTime.UtcNow, ex.Message);
            return false;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteExistingToolSkillBySkillIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { currentUserEmail, toolSkillId }));
        }
    }

    /// <summary>
    /// Gets all MCP tools available asynchronously.
    /// </summary>
    /// <param name="serverUrl">The MCP server url.</param>
    /// <param name="currentUserEmail">The current user email.</param>
    /// <returns>The list of <see cref="McpClientTool"/></returns>
    public async Task<IEnumerable<McpClientTool>> GetAllMcpToolsAvailableAsync(string serverUrl, string currentUserEmail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serverUrl);
        ArgumentException.ThrowIfNullOrWhiteSpace(currentUserEmail);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllMcpToolsAvailableAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { serverUrl, currentUserEmail }));
            return await mcpClientServices.GetAllMcpToolsAsync(serverUrl).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllMcpToolsAvailableAsync), DateTime.UtcNow, ex.Message);
            return [];
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllMcpToolsAvailableAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { serverUrl, currentUserEmail }));
        }
    }

    /// <summary>
    /// Gets all the tool skill data asynchronously.
    /// </summary>
    /// <param name="userEmail">The current logged in user email.</param>
    /// <returns>The list of <see cref="ToolSkillDomain"/></returns>
    public async Task<IEnumerable<ToolSkillDomain>> GetAllToolSkillsAsync(string userEmail)
    {
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetAllToolSkillsAsync), DateTime.UtcNow, userEmail);

            var filter = Builders<ToolSkillDomain>.Filter.And(Builders<ToolSkillDomain>.Filter.Eq(x => x.IsActive, true));
            return await mongoDatabaseService.GetDataFromCollectionAsync(MongoDatabaseName, ToolSkillsCollectionName, filter).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetAllToolSkillsAsync), DateTime.UtcNow, ex.Message);
            return [];
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetAllToolSkillsAsync), DateTime.UtcNow, userEmail);
        }
    }

    /// <summary>
    /// Gets a single tool skill data by its skill id asynchronously.
    /// </summary>
    /// <param name="toolSkillId">The tool skill id to be fetched.</param>
    /// <param name="currentUserEmail">The current logged in user email.</param>
    /// <returns>The tool skill domain model.</returns>
    public async Task<ToolSkillDomain> GetToolSkillBySkillIdAsync(string toolSkillId, string currentUserEmail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(toolSkillId);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetToolSkillBySkillIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { currentUserEmail, toolSkillId }));

            var filter = Builders<ToolSkillDomain>.Filter.And(
                Builders<ToolSkillDomain>.Filter.Eq(x => x.IsActive, true), Builders<ToolSkillDomain>.Filter.Eq(x => x.ToolSkillGuid, toolSkillId));
            var allData = await mongoDatabaseService.GetDataFromCollectionAsync(MongoDatabaseName, ToolSkillsCollectionName, filter).ConfigureAwait(false);
            return allData?.First() ?? throw new FileNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(GetToolSkillBySkillIdAsync), DateTime.UtcNow, ex.Message);
            return new();
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(GetToolSkillBySkillIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { currentUserEmail, toolSkillId }));
        }
    }

    /// <summary>
    /// Updates an existing tool skill data asynchronously.
    /// </summary>
    /// <param name="updateToolSkillData">The tool skill data domain model.</param>
    /// <param name="currentUserEmail">The user email.</param>
    /// <returns>The boolean for <c>success/failure</c></returns>
    public async Task<bool> UpdateExistingToolSkillDataAsync(ToolSkillDomain updateToolSkillData, string currentUserEmail)
    {
        ArgumentNullException.ThrowIfNull(updateToolSkillData);
        ArgumentException.ThrowIfNullOrWhiteSpace(currentUserEmail);

        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingToolSkillDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { currentUserEmail, updateToolSkillData.ToolSkillGuid }));

            var filter = Builders<ToolSkillDomain>.Filter.And(
                Builders<ToolSkillDomain>.Filter.Eq(x => x.IsActive, true),
                Builders<ToolSkillDomain>.Filter.Eq(x => x.ToolSkillGuid, updateToolSkillData.ToolSkillGuid));
            var toolSkillsData = await mongoDatabaseService.GetDataFromCollectionAsync(MongoDatabaseName, ToolSkillsCollectionName, filter).ConfigureAwait(false);
            var existingToolSkill = toolSkillsData.FirstOrDefault() ?? throw new FileNotFoundException(ExceptionConstants.DataNotFoundExceptionMessage);

            if (existingToolSkill.CreatedBy != currentUserEmail)
                throw new UnauthorizedAccessException(ExceptionConstants.UnauthorizedUserExceptionMessage);

            var updates = new List<UpdateDefinition<ToolSkillDomain>>
            {
                Builders<ToolSkillDomain>.Update.Set(x => x.AssociatedAgents, updateToolSkillData.AssociatedAgents),
                Builders<ToolSkillDomain>.Update.Set(x => x.ToolSkillDisplayName, updateToolSkillData.ToolSkillDisplayName),
                Builders<ToolSkillDomain>.Update.Set(x => x.ToolSkillMcpServerUrl, updateToolSkillData.ToolSkillMcpServerUrl),
                Builders<ToolSkillDomain>.Update.Set(x => x.ToolSkillTechnicalName, updateToolSkillData.ToolSkillTechnicalName),
                Builders<ToolSkillDomain>.Update.Set(x => x.DateModified, DateTime.UtcNow),
                Builders<ToolSkillDomain>.Update.Set(x => x.ModifiedBy, currentUserEmail),
            };

            var update = Builders<ToolSkillDomain>.Update.Combine(updates);
            return await mongoDatabaseService.UpdateDataInCollectionAsync(filter, update, MongoDatabaseName, ToolSkillsCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, LoggingConstants.LogHelperMethodFailed, nameof(UpdateExistingToolSkillDataAsync), DateTime.UtcNow, ex.Message);
            return false;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(UpdateExistingToolSkillDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { currentUserEmail, updateToolSkillData.ToolSkillGuid }));
        }
    }
}
