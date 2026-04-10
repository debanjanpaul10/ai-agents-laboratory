using MongoDB.Driver;

namespace AI.Agents.Laboratory.Functions.Data.Contracts;

/// <summary>
/// Defines the contract for a repository that interacts with a MongoDB database, providing methods for saving, retrieving, updating, and deleting data from specified collections.
/// </summary>
public interface IMongoDatabaseRepository
{
    /// <summary>
    /// Saves data to a specified collection within a MongoDB database, returning a boolean indicating the success of the operation.
    /// </summary>
    /// <typeparam name="TInput">The type of the data to save.</typeparam>
    /// <param name="data">The data to save.</param>
    /// <param name="databaseName">The name of the database.</param>
    /// <param name="collectionName">The name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean indicating success.</returns>
    Task<bool> SaveDataAsync<TInput>(
        TInput data,
        string databaseName,
        string collectionName,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves data from a specified collection within a MongoDB database based on a provided filter, returning an enumerable of the specified result type.
    /// </summary>
    /// <typeparam name="TResult">The type of the result to retrieve.</typeparam>
    /// <param name="databaseName">The name of the database.</param>
    /// <param name="collectionName">The name of the collection.</param>
    /// <param name="filter">The filter to apply.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, with an enumerable of the specified result type.</returns>
    Task<IEnumerable<TResult>> GetDataFromCollectionAsync<TResult>(
        string databaseName,
        string collectionName,
        FilterDefinition<TResult> filter,
        CancellationToken cancellationToken = default
    );

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
    Task<bool> UpdateDataInCollectionAsync<TDocument>(
        FilterDefinition<TDocument> filter,
        UpdateDefinition<TDocument> update,
        string databaseName,
        string collectionName,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Deletes data from a specified collection within a MongoDB database based on a provided filter, returning a boolean indicating the success of the operation.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document to delete.</typeparam>
    /// <param name="filter">The filter to apply.</param>
    /// <param name="databaseName">The name of the database.</param>
    /// <param name="collectionName">The name of the collection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean indicating success.</returns>
    Task<bool> DeleteDataFromCollectionAsync<TDocument>(
        FilterDefinition<TDocument> filter,
        string databaseName,
        string collectionName,
        CancellationToken cancellationToken = default
    );
}

