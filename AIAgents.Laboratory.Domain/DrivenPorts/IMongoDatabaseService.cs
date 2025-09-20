namespace AIAgents.Laboratory.Domain.DrivenPorts;

/// <summary>
/// The Mongo DB Database Service interface.
/// </summary>
public interface IMongoDatabaseService
{
    /// <summary>
    /// Saves the data asynchronous.
    /// </summary>
    /// <typeparam name="Tinput">The type of the input.</typeparam>
    /// <param name="data">The data.</param>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <returns>The boolean for success/failure.</returns>
    Task<bool> SaveDataAsync<Tinput>(Tinput data, string databaseName, string collectionName);

    /// <summary>
    /// Gets the data from collection asynchronous.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <returns>The mongo db collection.</returns>
    Task<IEnumerable<TResult>> GetDataFromCollectionAsync<TResult>(string databaseName, string collectionName);

    /// <summary>
    /// Updates the data from collection asynchronous.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="input">The input.</param>
    /// <param name="databaseName">Name of the database.</param>
    /// <param name="collectionName">Name of the collection.</param>
    /// <returns>The mongodb updated collection.</returns>
    Task<TResult> UpdateDataFromCollectionAsync<TInput, TResult>(TInput input, string databaseName, string collectionName);
}
