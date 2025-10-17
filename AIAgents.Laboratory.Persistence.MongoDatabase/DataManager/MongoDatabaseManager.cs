using System.Globalization;
using AIAgents.Laboratory.Domain.DrivenPorts;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using static AIAgents.Laboratory.Persistence.MongoDatabase.Helpers.Constants;

namespace AIAgents.Laboratory.Persistence.MongoDatabase.DataManager;

/// <summary>
/// The Mongo Database Manager service.
/// </summary>
/// <param name="mongoClient">The mongo db client.</param>
/// <param name="logger">The logger service.</param>
/// <seealso cref="IMongoDatabaseService"/>
public class MongoDatabaseManager(IMongoClient mongoClient, ILogger<MongoDatabaseManager> logger) : IMongoDatabaseService
{
    /// <summary>
    /// Gets the data from collection asynchronous with a filter condition.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="filter">The filter definition to apply when querying the collection.</param>
    /// <returns>The filtered mongo db collection results.</returns>
    public async Task<IEnumerable<TResult>> GetDataFromCollectionAsync<TResult>(string databaseName, string collectionName, FilterDefinition<TResult> filter)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodStartedMessageConstant, nameof(GetDataFromCollectionAsync), DateTime.UtcNow));
            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TResult>(collectionName);
            if (collectionData is not null) return await collectionData.Find(filter).ToListAsync().ConfigureAwait(false);

            throw new Exception(ExceptionConstants.SomethingWentWrongMessageConstant);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodFailedWithMessageConstant, nameof(GetDataFromCollectionAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodEndedMessageConstant, nameof(GetDataFromCollectionAsync), DateTime.UtcNow));
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
    public async Task<bool> SaveDataAsync<TInput>(TInput data, string databaseName, string collectionName)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodStartedMessageConstant, nameof(SaveDataAsync), DateTime.UtcNow));
            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TInput>(collectionName);
            if (collectionData is not null)
            {
                await collectionData.InsertOneAsync(data).ConfigureAwait(false);
                return true;
            }

            throw new Exception(ExceptionConstants.SomethingWentWrongMessageConstant);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodFailedWithMessageConstant, nameof(SaveDataAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodEndedMessageConstant, nameof(SaveDataAsync), DateTime.UtcNow));
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
    public async Task<bool> UpdateDataInCollectionAsync<TDocument>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, string databaseName, string collectionName)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodStartedMessageConstant, nameof(UpdateDataInCollectionAsync), DateTime.UtcNow));

            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TDocument>(collectionName) ?? throw new Exception(ExceptionConstants.CollectionDoesNotExistsMessage);

            var result = await collectionData.UpdateOneAsync(filter, update).ConfigureAwait(false);
            if (result.ModifiedCount == 0) throw new Exception("No document was updated. Document may not exist or no changes were needed.");

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodFailedWithMessageConstant, nameof(UpdateDataInCollectionAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodEndedMessageConstant, nameof(UpdateDataInCollectionAsync), DateTime.UtcNow));
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
    public async Task<bool> DeleteDataFromCollectionAsync<TDocument>(FilterDefinition<TDocument> filter, string databaseName, string collectionName)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodStartedMessageConstant, nameof(DeleteDataFromCollectionAsync), DateTime.UtcNow));

            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TDocument>(collectionName) ?? throw new Exception(ExceptionConstants.CollectionDoesNotExistsMessage);

            var result = await collectionData.DeleteOneAsync(filter).ConfigureAwait(false);
            if (result.DeletedCount == 0) throw new Exception("No document was deleted. Document may not exist or no changes were needed.");

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodFailedWithMessageConstant, nameof(DeleteDataFromCollectionAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodEndedMessageConstant, nameof(DeleteDataFromCollectionAsync), DateTime.UtcNow));
        }
    }

}
