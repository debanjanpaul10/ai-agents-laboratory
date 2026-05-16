using System.Data.SqlTypes;
using AI.Agents.Laboratory.Functions.Data.Contracts;
using AI.Agents.Laboratory.Functions.Shared.Constants;
using AI.Agents.Laboratory.Functions.Shared.Exceptions;
using AI.Agents.Laboratory.Functions.Shared.Helpers;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace AI.Agents.Laboratory.Functions.Data.Services;

/// <summary>
/// Implements the IMongoDatabaseRepository to provide methods for saving, retrieving, updating, and deleting data from specified collections within a MongoDB database. 
/// This class uses the MongoDB.Driver to interact with the database and includes logging and exception handling to ensure robust operations.
/// </summary>
/// <param name="mongoClient">The MongoDB client.</param>
/// <param name="logger">The logger.</param>
/// <param name="correlationContext">The correlation context.</param>
/// <seealso cref="IMongoDatabaseRepository"/>
public sealed class MongoDatabaseRepository(
    IMongoClient mongoClient,
    ILogger<MongoDatabaseRepository> logger,
    ICorrelationContext correlationContext) : IMongoDatabaseRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<TResult>> GetDataFromCollectionAsync<TResult>(
        string databaseName,
        string collectionName,
        FilterDefinition<TResult> filter,
        CancellationToken cancellationToken = default
    )
    {
        IEnumerable<TResult>? response = null;
        try
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodStart,
                nameof(GetDataFromCollectionAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName })
            );

            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TResult>(collectionName);
            response = await collectionData.Find(filter)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggerConstants.LogHelperMethodFailed,
                nameof(GetDataFromCollectionAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodEnd,
                nameof(GetDataFromCollectionAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName, response })
            );
        }
    }

    /// <inheritdoc/>
    public async Task<bool> SaveDataAsync<TInput>(
        TInput data,
        string databaseName,
        string collectionName,
        bool bypassDocumentValidation = false,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodStart,
                nameof(SaveDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName })
            );

            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TInput>(collectionName);
            if (collectionData is not null)
            {
                var insertOptions = new InsertOneOptions() { BypassDocumentValidation = bypassDocumentValidation };
                await collectionData.InsertOneAsync(
                    document: data,
                    options: insertOptions,
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);
                return true;
            }

            throw new SqlTypeException(ExceptionConstants.SomethingWentWrongDefaultMessage);
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggerConstants.LogHelperMethodFailed, nameof(SaveDataAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodEnd,
                nameof(SaveDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName })
            );
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateDataInCollectionAsync<TDocument>(
        FilterDefinition<TDocument> filter,
        UpdateDefinition<TDocument> update,
        string databaseName,
        string collectionName,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodStart,
                nameof(UpdateDataInCollectionAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName })
            );

            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TDocument>(collectionName);
            var result = await collectionData.UpdateOneAsync(
                filter,
                update,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggerConstants.LogHelperMethodFailed,
                nameof(UpdateDataInCollectionAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodEnd,
                nameof(UpdateDataInCollectionAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName })
            );
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteDataFromCollectionAsync<TDocument>(
        FilterDefinition<TDocument> filter,
        string databaseName,
        string collectionName,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodStart,
                nameof(DeleteDataFromCollectionAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName })
            );

            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TDocument>(collectionName);
            var result = await collectionData.DeleteOneAsync(
                filter,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
            return result.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggerConstants.LogHelperMethodFailed,
                nameof(DeleteDataFromCollectionAsync), DateTime.UtcNow, ex.Message
            );
            throw new AIAgentsBusinessException(
                message: ex.Message,
                correlationId: correlationContext.CorrelationId
            );
        }
        finally
        {
            logger.LogAppInformation(
                LoggerConstants.LogHelperMethodEnd,
                nameof(DeleteDataFromCollectionAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName })
            );
        }
    }
}

