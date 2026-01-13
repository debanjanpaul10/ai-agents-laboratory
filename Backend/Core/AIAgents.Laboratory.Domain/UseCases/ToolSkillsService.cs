using AIAgents.Laboratory.Domain.DomainEntities;
using AIAgents.Laboratory.Domain.DrivenPorts;
using AIAgents.Laboratory.Domain.DrivingPorts;
using AIAgents.Laboratory.Domain.Helpers;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Domain.Helpers.Constants;

namespace AIAgents.Laboratory.Domain.UseCases;

/// <summary>
/// The tool skills service business class.
/// </summary>
/// <param name="logger">The logger service.</param>
/// <param name="mongoDatabaseService">The mongo database service.</param>
/// <seealso cref="IToolSkillsService"/>
public class ToolSkillsService(ILogger<ToolSkillsService> logger, IMongoDatabaseService mongoDatabaseService) : IToolSkillsService
{
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
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(AddNewToolSkillAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { userEmail, toolSkillData }));

            toolSkillData.ToolSkillGuid = Guid.NewGuid().ToString();
            toolSkillData.PrepareAuditEntityData(userEmail);

            return await mongoDatabaseService.SaveDataAsync(toolSkillData, MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.ToolSkillsCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(AddNewToolSkillAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(AddNewToolSkillAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { userEmail, toolSkillData }));
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
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(DeleteExistingToolSkillBySkillIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { currentUserEmail, toolSkillId }));

            var filter = Builders<ToolSkillDomain>.Filter.Where(tsd => tsd.IsActive && tsd.ToolSkillGuid == toolSkillId);
            var allToolSkills = await mongoDatabaseService.GetDataFromCollectionAsync(MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.ToolSkillsCollectionName, filter);
            var updateToolSkill = allToolSkills.First() ?? throw new Exception(ExceptionConstants.DataNotFoundExceptionMessage);

            var updates = new List<UpdateDefinition<ToolSkillDomain>>
            {
                Builders<ToolSkillDomain>.Update.Set(x => x.IsActive, false),
                Builders<ToolSkillDomain>.Update.Set(x => x.DateModified, DateTime.UtcNow),
                Builders<ToolSkillDomain>.Update.Set(x => x.ModifiedBy, currentUserEmail)
            };
            var update = Builders<ToolSkillDomain>.Update.Combine(updates);
            return await mongoDatabaseService.UpdateDataInCollectionAsync(filter, update, MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.ToolSkillsCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(DeleteExistingToolSkillBySkillIdAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(DeleteExistingToolSkillBySkillIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { currentUserEmail, toolSkillId }));
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
            return await mongoDatabaseService.GetDataFromCollectionAsync(MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.ToolSkillsCollectionName, filter).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(GetAllToolSkillsAsync), DateTime.UtcNow, ex.Message);
            throw;
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
        try
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(GetToolSkillBySkillIdAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { currentUserEmail, toolSkillId }));

            var filter = Builders<ToolSkillDomain>.Filter.And(
                Builders<ToolSkillDomain>.Filter.Eq(x => x.IsActive, true),
                Builders<ToolSkillDomain>.Filter.Eq(x => x.ToolSkillGuid, toolSkillId));
            var allData = await mongoDatabaseService.GetDataFromCollectionAsync(
                MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.ToolSkillsCollectionName, filter).ConfigureAwait(false);

            return allData?.First() ?? throw new Exception(ExceptionConstants.DataNotFoundExceptionMessage);
        }
        catch (Exception ex)
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(GetToolSkillBySkillIdAsync), DateTime.UtcNow, ex.Message);
            throw;
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
            logger.LogInformation(LoggingConstants.LogHelperMethodStart, nameof(UpdateExistingToolSkillDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { currentUserEmail, updateToolSkillData }));

            var filter = Builders<ToolSkillDomain>.Filter.And(
                Builders<ToolSkillDomain>.Filter.Eq(x => x.IsActive, true),
                Builders<ToolSkillDomain>.Filter.Eq(x => x.ToolSkillGuid, updateToolSkillData.ToolSkillGuid));
            var toolSkillsData = await mongoDatabaseService.GetDataFromCollectionAsync(MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.ToolSkillsCollectionName, filter).ConfigureAwait(false);
            var existingToolSkill = toolSkillsData.FirstOrDefault() ?? throw new Exception(ExceptionConstants.DataNotFoundExceptionMessage);

            var updates = new List<UpdateDefinition<ToolSkillDomain>>
            {
                Builders<ToolSkillDomain>.Update.Set(x => x.AssociatedAgentGuids, updateToolSkillData.AssociatedAgentGuids),
                Builders<ToolSkillDomain>.Update.Set(x => x.ToolSkillDisplayName, updateToolSkillData.ToolSkillDisplayName),
                Builders<ToolSkillDomain>.Update.Set(x => x.ToolSkillMcpServerUrl, updateToolSkillData.ToolSkillMcpServerUrl),
                Builders<ToolSkillDomain>.Update.Set(x => x.ToolSkillTechnicalName, updateToolSkillData.ToolSkillTechnicalName),
                Builders<ToolSkillDomain>.Update.Set(x => x.DateModified, DateTime.UtcNow),
                Builders<ToolSkillDomain>.Update.Set(x => x.ModifiedBy, currentUserEmail),
            };

            var update = Builders<ToolSkillDomain>.Update.Combine(updates);
            return await mongoDatabaseService.UpdateDataInCollectionAsync(filter, update, MongoDbCollectionConstants.AiAgentsPrimaryDatabase, MongoDbCollectionConstants.ToolSkillsCollectionName).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodFailed, nameof(UpdateExistingToolSkillDataAsync), DateTime.UtcNow, ex.Message);
            throw;
        }
        finally
        {
            logger.LogInformation(LoggingConstants.LogHelperMethodEnd, nameof(UpdateExistingToolSkillDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { currentUserEmail, updateToolSkillData }));
        }
    }
}
