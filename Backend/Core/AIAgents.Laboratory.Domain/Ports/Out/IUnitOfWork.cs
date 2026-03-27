namespace AIAgents.Laboratory.Domain.Ports.Out;

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
    /// <param name="cancellationToken">The cancellation token to be used to cancel the asynchronous process. Optional.</param>
    /// <returns>The generic entity type.</returns>
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;

    /// <summary>
    /// This method saves all changes made in this context to the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to be used to cancel the asynchronous process. Optional.</param>
    /// <returns>The save changes count.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// This method begins a new transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to be used to cancel the asynchronous process. Optional.</param>
    /// <returns>A task to wait on.</returns>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits all changes made in this context to the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to be used to cancel the asynchronous process. Optional.</param>
    /// <returns>A task to wait on.</returns>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rollbacks all changes made in this context to the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to be used to cancel the asynchronous process. Optional.</param>
    /// <returns>A task to wait on.</returns>
    Task RollbackAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the SQL query asynchronous.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql">The SQL.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The SQL query response.</returns>
    Task<List<T>> ExecuteSqlQueryAsync<T>(string sql, params object[] parameters);
}
