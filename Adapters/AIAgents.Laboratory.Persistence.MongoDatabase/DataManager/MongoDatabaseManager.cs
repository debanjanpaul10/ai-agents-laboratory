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
	/// Gets the single data from collection asynchronous.
	/// </summary>
	/// <typeparam name="TInput">The type of the input.</typeparam>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="input">The input.</param>
	/// <param name="databaseName">Name of the database.</param>
	/// <param name="collectionName">Name of the collection.</param>
	/// <returns>The mongo db collection.</returns>
	public async Task<TResult> GetDataFromCollectionAsync<TInput, TResult>(TInput input, string databaseName, string collectionName)
	{
		try
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodStartedMessageConstant, nameof(GetDataFromCollectionAsync), DateTime.UtcNow));
			var mongoDatabase = mongoClient.GetDatabase(databaseName);
			var collectionData = mongoDatabase.GetCollection<TResult>(collectionName);
			if (collectionData is not null)
			{
				return await collectionData.Find(_ => true).FirstAsync().ConfigureAwait(false);
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
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodStartedMessageConstant, nameof(GetDataFromCollectionAsync), DateTime.UtcNow));
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
			logger.LogError(ex, string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodFailedWithMessageConstant, nameof(GetDataFromCollectionAsync), DateTime.UtcNow, ex.Message));
			throw;
		}
		finally
		{
			logger.LogInformation(string.Format(CultureInfo.CurrentCulture, LoggingConstants.MethodEndedMessageConstant, nameof(GetDataFromCollectionAsync), DateTime.UtcNow));
		}
	}
}
