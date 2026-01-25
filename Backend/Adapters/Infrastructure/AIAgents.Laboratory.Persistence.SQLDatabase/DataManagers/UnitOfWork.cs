using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Persistence.SQLDatabase.Context;
using AIAgents.Laboratory.Persistence.SQLDatabase.Contracts;
using AIAgents.Laboratory.Persistence.SQLDatabase.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.DataManagers;

/// <summary>
/// The Unit of Work Class.
/// </summary>
/// <param name="dbContext">The sql db context.</param>
/// <seealso cref="IUnitOfWork"/>
[ExcludeFromCodeCoverage]
public sealed class UnitOfWork(SqlDbContext dbContext) : IUnitOfWork
{
    /// <summary>
    /// The repositories dictionary to hold repositories for different entity types.
    /// </summary>
    private readonly Dictionary<Type, object> _repositories = [];

    /// <summary>
    /// The transaction for the unit of work.
    /// </summary>
#pragma warning disable CS8618
    private IDbContextTransaction _transaction;

    /// <summary>
    /// This method returns a repository for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>The generic entity type.</returns>
    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        if (!this._repositories.TryGetValue(type, out var repository))
        {
            repository = new GenericRepository<TEntity>(dbContext);
            this._repositories[type] = repository;
        }

        return (IRepository<TEntity>)repository;
    }

    /// <summary>
    /// This method begins a new transaction asynchronously.
    /// </summary>
    /// <returns>A task to wait on.</returns>
    public async Task BeginTransactionAsync()
    {
        this._transaction = await dbContext.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Commits all changes made in this context to the database asynchronously.
    /// </summary>
    /// <returns>A task to wait on.</returns>
    public async Task CommitAsync()
    {
        await dbContext.SaveChangesAsync();
        if (this._transaction is not null)
            await this._transaction.CommitAsync();
    }

    /// <summary>
    /// Rollbacks all changes made in this context to the database asynchronously.
    /// </summary>
    /// <returns>A task to wait on.</returns>
    public async Task RollbackAsync()
    {
        if (this._transaction is not null)
            await this._transaction.RollbackAsync();
    }

    /// <summary>
    /// This method saves all changes made in this context to the database asynchronously.
    /// </summary>
    /// <returns>The save changes count.</returns>
    public async Task<int> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Disposes the unit of work, releasing all resources.
    /// </summary>
    public void Dispose()
    {
        dbContext.Dispose();
        this._transaction?.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Executes the SQL query or command asynchronously. For entity types, returns mapped results. For scalar types (bool, int), returns result based on rows affected.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="sql">The SQL or stored procedure command.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The SQL query response as a list of T.</returns>
    public async Task<List<T>> ExecuteSqlQueryAsync<T>(string sql, params object[] parameters)
    {
        // For scalar types, treat as non-query and return rows affected or success as bool
        if (typeof(T) == typeof(bool))
        {
            var rows = await dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
            return [(T)(object)(rows > 0)];
        }

        if (typeof(T) == typeof(int))
        {
            var rows = await dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
            return [(T)(object)rows];
        }

        // For SPs that do not return anything, use T=object or T=bool and return an empty list after execution.
        // typeof(void) is not valid in generics, so we cannot check for it directly.
        // Usage: await ExecuteSqlQueryAsync<object>(sql, params) or ExecuteSqlQueryAsync<bool>(sql, params)
        if (typeof(T) == typeof(object))
        {
            await dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
            return [];
        }

        throw new NotSupportedException($"Raw SQL query for type {typeof(T).Name} is not supported.");
    }
}
