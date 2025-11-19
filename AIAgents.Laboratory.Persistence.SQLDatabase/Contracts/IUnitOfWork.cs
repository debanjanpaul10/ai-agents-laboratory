namespace AIAgents.Laboratory.Persistence.SQLDatabase.Contracts;

/// <summary>
/// The Unit of Work Interface.
/// </summary>
/// <seealso cref="IDisposable"/>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// This method returns a repository for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>The generic entity type.</returns>
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;

    /// <summary>
    /// This method saves all changes made in this context to the database asynchronously.
    /// </summary>
    /// <returns>The save changes count.</returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// This method begins a new transaction asynchronously.
    /// </summary>
    /// <returns>A task to wait on.</returns>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits all changes made in this context to the database asynchronously.
    /// </summary>
    /// <returns>A task to wait on.</returns>
    Task CommitAsync();

    /// <summary>
    /// Rollbacks all changes made in this context to the database asynchronously.
    /// </summary>
    /// <returns>A task to wait on.</returns>
    Task RollbackAsync();

    /// <summary>
    /// Executes the SQL query asynchronous.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql">The SQL.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The SQL query response.</returns>
    Task<List<T>> ExecuteSqlQueryAsync<T>(string sql, params object[] parameters);
}
