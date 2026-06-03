using System.Diagnostics.CodeAnalysis;
using AIAgents.Laboratory.Domain.Ports.Out;
using AIAgents.Laboratory.Persistence.SQLDatabase.Context;
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        this._transaction = await dbContext.Database
            .BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await dbContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        if (this._transaction is not null)
            await this._transaction
                .CommitAsync(cancellationToken)
                .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (this._transaction is not null)
            await this._transaction
                .RollbackAsync(cancellationToken)
                .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        dbContext.Dispose();
        this._transaction?.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public async Task<List<T>> ExecuteSqlQueryAsync<T>(string sql, params object[] parameters)
    {
        // For scalar types, treat as non-query and return rows affected or success as bool
        if (typeof(T) == typeof(bool))
        {
            var rows = await dbContext.Database.ExecuteSqlRawAsync(sql, parameters).ConfigureAwait(false);
            return [(T)(object)(rows > 0)];
        }

        if (typeof(T) == typeof(int))
        {
            var rows = await dbContext.Database.ExecuteSqlRawAsync(sql, parameters).ConfigureAwait(false);
            return [(T)(object)rows];
        }

        // For SPs that do not return anything, use T=object or T=bool and return an empty list after execution.
        // typeof(void) is not valid in generics, so we cannot check for it directly.
        // Usage: await ExecuteSqlQueryAsync<object>(sql, params) or ExecuteSqlQueryAsync<bool>(sql, params)
        if (typeof(T) == typeof(object))
        {
            await dbContext.Database.ExecuteSqlRawAsync(sql, parameters).ConfigureAwait(false);
            return [];
        }

        throw new NotSupportedException($"Raw SQL query for type {typeof(T).Name} is not supported.");
    }
}
