using MongoDB.Driver;

namespace AIAgents.Laboratory.Domain.DrivenPorts;

/// <summary>
/// The Mongo DB Database Service interface.
/// </summary>
public interface IMongoDatabaseService
{
    /// <summary>
    /// Saves the data asynchronous.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <param name="data">The data.</param>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> SaveDataAsync<TInput>(TInput data, string databaseName, string collectionName);

    /// <summary>
    /// Gets the data from collection asynchronous.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <returns>The mongo db collection.</returns>
    Task<IEnumerable<TResult>> GetDataFromCollectionAsync<TResult>(string databaseName, string collectionName);

    /// <summary>
    /// Gets the data from collection asynchronous with a filter condition.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="filter">The filter definition to apply when querying the collection.</param>
    /// <returns>The filtered mongo db collection results.</returns>
    Task<IEnumerable<TResult>> GetDataFromCollectionAsync<TResult>(string databaseName, string collectionName, FilterDefinition<TResult> filter);

    /// <summary>
    /// Updates the data in collection asynchronous using filter and update definitions.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    /// <param name="filter">The filter definition to identify documents to update.</param>
    /// <param name="update">The update definition specifying the updates to apply.</param>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> UpdateDataInCollectionAsync<TDocument>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, string databaseName, string collectionName);
}
