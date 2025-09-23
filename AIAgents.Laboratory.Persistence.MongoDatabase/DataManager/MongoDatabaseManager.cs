using System.Globalization;
using AIAgents.Laboratory.Domain.DrivenPorts;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
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
    /// Gets the data from collection asynchronous.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <returns>The mongo db collection.</returns>
    public async Task<IEnumerable<TResult>> GetDataFromCollectionAsync<TResult>(string databaseName, string collectionName)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodStartedMessageConstant, nameof(GetDataFromCollectionAsync), DateTime.UtcNow));
            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TResult>(collectionName);
            if (collectionData is not null)
            {
                return await collectionData.Find(_ => true).ToListAsync().ConfigureAwait(false);
            }

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
    /// <typeparam name="Tinput">The type of the input.</typeparam>
    /// <param name="data">The data.</param>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> SaveDataAsync<Tinput>(Tinput data, string databaseName, string collectionName)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodStartedMessageConstant, nameof(SaveDataAsync), DateTime.UtcNow));
            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<Tinput>(collectionName);
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
    /// Updates the data from collection asynchronous.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="input">The input.</param>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <returns>The boolean for success/failure.</returns>
    public async Task<bool> UpdateDataFromCollectionAsync<TInput>(TInput input, string databaseName, string collectionName)
    {
        try
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodStartedMessageConstant, nameof(UpdateDataFromCollectionAsync), DateTime.UtcNow));

            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            var collectionData = mongoDatabase.GetCollection<TInput>(collectionName) ?? throw new Exception(ExceptionConstants.CollectionDoesNotExistsMessage);
            
            var idProperty = typeof(TInput).GetProperty("Id") ?? throw new Exception("Entity must have an Id property for updates");
            var idValue = idProperty.GetValue(input);
            if (idValue == null || string.IsNullOrEmpty(idValue.ToString()))
            {
                throw new Exception("Id property cannot be null or empty for updates");
            }

            var filter = Builders<TInput>.Filter.Eq("_id", ObjectId.Parse(idValue.ToString()));
            var result = await collectionData.ReplaceOneAsync(filter, input).ConfigureAwait(false);
            if (result.ModifiedCount == 0)
            {
                throw new Exception("No document was updated. Document may not exist.");
            }

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodFailedWithMessageConstant, nameof(UpdateDataFromCollectionAsync), DateTime.UtcNow, ex.Message));
            throw;
        }
        finally
        {
            logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodEndedMessageConstant, nameof(UpdateDataFromCollectionAsync), DateTime.UtcNow));
        }
    }
}
