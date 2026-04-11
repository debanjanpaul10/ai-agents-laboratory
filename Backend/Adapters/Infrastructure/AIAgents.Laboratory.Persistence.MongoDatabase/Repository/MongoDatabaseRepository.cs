using System.Data.SqlTypes;
using AIAgents.Laboratory.Domain.Contracts;
using AIAgents.Laboratory.Domain.Helpers;
using AIAgents.Laboratory.Persistence.MongoDatabase.Contracts;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using static AIAgents.Laboratory.Persistence.MongoDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.MongoDatabase.Repository;

/// <summary>
/// Provides a repository implementation for interacting with MongoDB, allowing for saving, retrieving, updating, and deleting data in specified databases and collections.
/// </summary>
/// <remarks>This repository interacts with the MongoDB.Driver to perform CRUD operations on the specified database and collection, while also implementing structured logging and exception handling to ensure robust data management. 
/// The repository methods are designed to be asynchronous to optimize performance and scalability when dealing with potentially large datasets in MongoDB.</remarks>
/// <param name="mongoClient">The mongo database client service.</param>
/// <param name="logger">The logger service.</param>
/// <param name="correlationContext">The correlation context used for logging.</param>
/// <seealso cref="IMongoDatabaseRepository"/>
public sealed class MongoDatabaseRepository(
    IMongoClient mongoClient,
    ILogger<MongoDatabaseRepository> logger,
    ICorrelationContext correlationContext) : IMongoDatabaseRepository
{
    /// <summary>
    /// Gets the data from collection asynchronous with a filter condition.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="filter">The filter definition to apply when querying the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The filtered mongo db collection results.</returns>
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
                LoggingConstants.MethodStartedMessageConstant,
                nameof(GetDataFromCollectionAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName })
            );

            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TResult>(collectionName);
            if (collectionData is not null)
            {
                response = await collectionData
                    .Find(filter)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
                return response;
            }

            throw new SqlTypeException(ExceptionConstants.SomethingWentWrongMessageConstant);
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
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
                LoggingConstants.MethodEndedMessageConstant,
                nameof(GetDataFromCollectionAsync), DateTime.UtcNow,
                    JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName, response })
            );
        }
    }

    /// <summary>
    /// Saves the data asynchronous.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="data">The data.</param>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> SaveDataAsync<TInput>(
        TInput data,
        string databaseName,
        string collectionName,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            logger.LogAppInformation(
                LoggingConstants.MethodStartedMessageConstant,
                nameof(SaveDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName })
            );

            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TInput>(collectionName);
            if (collectionData is not null)
            {
                var insertOptions = new InsertOneOptions() { BypassDocumentValidation = true };
                await collectionData.InsertOneAsync(
                    document: data,
                    options: insertOptions,
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);
                return true;
            }

            throw new SqlTypeException(ExceptionConstants.SomethingWentWrongMessageConstant);
        }
        catch (Exception ex)
        {
            logger.LogAppError(
                ex,
                LoggingConstants.MethodFailedWithMessageConstant,
                nameof(SaveDataAsync), DateTime.UtcNow, ex.Message
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
                nameof(SaveDataAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName })
            );
        }
    }

    /// <summary>
    /// Updates the data in collection asynchronous using filter and update definitions.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    /// <param name="filter">The filter definition to identify documents to update.</param>
    /// <param name="update">The update definition specifying the updates to apply.</param>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <returns>The boolean for success/failure.</returns>
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
                LoggingConstants.MethodStartedMessageConstant,
                nameof(UpdateDataInCollectionAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName })
            );

            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TDocument>(collectionName) ?? throw new FileNotFoundException(ExceptionConstants.CollectionDoesNotExistsMessage);
            var result = await collectionData.UpdateManyAsync(
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
                LoggingConstants.MethodFailedWithMessageConstant,
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
                LoggingConstants.MethodEndedMessageConstant,
                nameof(UpdateDataInCollectionAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName })
            );
        }
    }

    /// <summary>
    /// Deletes the data from mongo db collection using filter.
    /// </summary>
    /// <typeparam name="TDocument">The type of document.</typeparam>
    /// <param name="filter">The filter.</param>
    /// <param name="databaseName">The database name.</param>
    /// <param name="collectionName">The collection name.</param>
    /// <returns>The boolean for success/failure.</returns>
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
                LoggingConstants.MethodStartedMessageConstant,
                nameof(DeleteDataFromCollectionAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName })
            );

            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TDocument>(collectionName) ?? throw new FileNotFoundException(ExceptionConstants.CollectionDoesNotExistsMessage);
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
                LoggingConstants.MethodFailedWithMessageConstant, nameof(DeleteDataFromCollectionAsync), DateTime.UtcNow, ex.Message
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
                nameof(DeleteDataFromCollectionAsync), DateTime.UtcNow, JsonConvert.SerializeObject(new { correlationContext.CorrelationId, databaseName, collectionName })
            );
        }
    }
}
