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
    /// <summary>
    /// Saves data to a specified collection within a MongoDB database, returning a boolean indicating the success of the operation. 
    /// The method includes logging at the start and end of the operation, as well as error logging in case of exceptions. 
    /// It uses the InsertOneAsync method of the MongoDB driver to insert the document into the collection.
    /// </summary>
    /// <typeparam name="TResult">The type of the result to retrieve.</typeparam>
    /// <param name="databaseName">The name of the database.</param>
    /// <param name="collectionName">The name of the collection.</param>
    /// <param name="filter">The filter to apply.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean indicating success.</returns>
    public async Task<IEnumerable<TResult>> GetDataFromCollectionAsync<TResult>(
        string databaseName,
        string collectionName,
        FilterDefinition<TResult> filter,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<TResult>? response = null;
        try
        {
            logger.LogAppInformation(LoggerConstants.LogHelperMethodStart, nameof(GetDataFromCollectionAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName }));

            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TResult>(collectionName);
            response = await collectionData.Find(filter).ToListAsync(cancellationToken).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggerConstants.LogHelperMethodFailed, nameof(GetDataFromCollectionAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggerConstants.LogHelperMethodEnd, nameof(GetDataFromCollectionAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName, response }));
        }
    }

    /// <summary>
    /// Saves data to a specified collection within a MongoDB database, returning a boolean indicating the success of the operation. 
    /// The method includes logging at the start and end of the operation, as well as error logging in case of exceptions. 
    /// It uses the InsertOneAsync method of the MongoDB driver to insert the document into the collection.
    /// </summary>
    /// <typeparam name="TInput">The type of the input data.</typeparam>
    /// <param name="data">The data to save.</param>
    /// <param name="databaseName">The name of the database.</param>
    /// <param name="collectionName">The name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean indicating success.</returns>
    public async Task<bool> SaveDataAsync<TInput>(
        TInput data,
        string databaseName,
        string collectionName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogAppInformation(LoggerConstants.LogHelperMethodStart, nameof(SaveDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName }));

            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TInput>(collectionName);

            var insertOptions = new InsertOneOptions { BypassDocumentValidation = true };
            await collectionData.InsertOneAsync(
                document: data,
                options: insertOptions,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogAppError(ex, LoggerConstants.LogHelperMethodFailed, nameof(SaveDataAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggerConstants.LogHelperMethodEnd, nameof(SaveDataAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName }));
        }
    }

    /// <summary>
    /// Updates data in a specified collection within a MongoDB database based on a provided filter and update definition, returning a boolean indicating the success of the operation.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document to update.</typeparam>
    /// <param name="filter">The filter to apply.</param>
    /// <param name="update">The update definition.</param>
    /// <param name="databaseName">The name of the database.</param>
    /// <param name="collectionName">The name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean indicating success.</returns>
    public async Task<bool> UpdateDataInCollectionAsync<TDocument>(
        FilterDefinition<TDocument> filter,
        UpdateDefinition<TDocument> update,
        string databaseName,
        string collectionName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogAppInformation(LoggerConstants.LogHelperMethodStart, nameof(UpdateDataInCollectionAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName }));

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
            logger.LogAppError(ex, LoggerConstants.LogHelperMethodFailed, nameof(UpdateDataInCollectionAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggerConstants.LogHelperMethodEnd, nameof(UpdateDataInCollectionAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName }));
        }
    }

    /// <summary>
    /// Deletes data from a specified collection within a MongoDB database based on a provided filter, returning a boolean indicating the success of the operation.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document to delete.</typeparam>
    /// <param name="filter">The filter to apply.</param>
    /// <param name="databaseName">The name of the database.</param>
    /// <param name="collectionName">The name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean indicating success.</returns>
    public async Task<bool> DeleteDataFromCollectionAsync<TDocument>(
        FilterDefinition<TDocument> filter,
        string databaseName,
        string collectionName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogAppInformation(LoggerConstants.LogHelperMethodStart, nameof(DeleteDataFromCollectionAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName }));

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
            logger.LogAppError(ex, LoggerConstants.LogHelperMethodFailed, nameof(DeleteDataFromCollectionAsync), DateTime.UtcNow, ex.Message);
            throw new AIAgentsBusinessException(ex.Message, correlationContext.CorrelationId);
        }
        finally
        {
            logger.LogAppInformation(LoggerConstants.LogHelperMethodEnd, nameof(DeleteDataFromCollectionAsync), DateTime.UtcNow,
                JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName }));
        }
    }
}

