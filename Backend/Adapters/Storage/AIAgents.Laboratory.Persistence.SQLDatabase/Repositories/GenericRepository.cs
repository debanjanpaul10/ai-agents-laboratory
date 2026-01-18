using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using AIAgents.Laboratory.Persistence.SQLDatabase.Context;
using AIAgents.Laboratory.Persistence.SQLDatabase.Contracts;
using AIAgents.Laboratory.Persistence.SQLDatabase.Helpers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.Repositories;

/// <summary>
/// The generic repository implementation.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <param name="context">The SQL db context.</param>
[ExcludeFromCodeCoverage]
public sealed class GenericRepository<TEntity>(SqlDbContext context) : IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The generic entity.</param>
    /// <returns>
    /// The generic entity.
    /// </returns>
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
        return entity;
    }

    /// <summary>
    /// Adds a range of entities to the repository.
    /// </summary>
    /// <param name="entities">The list of generic entity.</param>
    /// <returns>
    /// The list of generic entity.
    /// </returns>
    public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await context.Set<TEntity>().AddRangeAsync(entities);
        return entities;
    }

    /// <summary>
    /// Finds entities based on the provided predicate.
    /// </summary>
    /// <param name="predicate">The entity finder predicate.</param>
    /// <returns>
    /// The list of generic entity.
    /// </returns>
    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await context.Set<TEntity>().Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Gets the first entity that matches the provided predicate.
    /// </summary>
    /// <param name="predicate">The filter predicate.</param>
    /// <returns>
    /// The generic entity.
    /// </returns>
    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// Gets all entities from the repository.
    /// </summary>
    /// <param name="filter">The filter predicate.</param>
    /// <param name="includeProperties">The included properties string.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="isActiveOnly">The isactive only boolean flag.</param>
    /// <returns>
    /// The list of generic entity.
    /// </returns>
    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1, bool isActiveOnly = true)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        query = query.WhereIsActive(isActiveOnly);

        if (filter is not null)
            query = query.Where(filter);

        if (pageSize > 0)
        {
            if (pageSize > 100) pageSize = 100;
            query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }

        if (includeProperties is not null)
            foreach (var includeProperty in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

        return await query.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Gets all entities from the repository with pagination.
    /// </summary>
    /// <param name="filter">The filter predicate.</param>
    /// <param name="orderByProperty">The order by property.</param>
    /// <param name="ascending">The ascending sort order boolean flag.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="includeProperties">The included properties string.</param>
    /// <returns>
    /// A tupple containing values.
    /// </returns>
    public async Task<(List<TEntity>, int, bool)> GetAllPagedAsync(Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>? orderByProperty = null, bool ascending = true, int pageSize = 1000, int pageNumber = 1, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        bool hasNextPage = false;
        IQueryable<TEntity> query = context.Set<TEntity>();
        if (filter is not null)
            query = query.Where(filter);

        if (orderByProperty is not null)
            query = ascending ? query.OrderBy(orderByProperty) : query.OrderByDescending(orderByProperty);

        int totalDbRecords = await query.CountAsync();
        if (pageSize > 0)
            query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize + 1);

        if (includeProperties is not null)
            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

        var result = await query.AsNoTracking().ToListAsync();
        if (result.Count > pageSize)
        {
            hasNextPage = true;
            result.RemoveAt(result.Count - 1);
        }

        return (result, totalDbRecords, hasNextPage);
    }

    /// <summary>
    /// Gets all entities from the repository with pagination.
    /// </summary>
    /// <param name="filter">The filter predicate.</param>
    /// <param name="tracked">The is tracked boolean flag.</param>
    /// <param name="includeProperties">The included properties string.</param>
    /// <param name="isActiveOnly">The isactive only boolean flag.</param>
    /// <returns>
    /// The generic entity.
    /// </returns>
    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>>? filter = null, bool tracked = true, string? includeProperties = null, bool isActiveOnly = true)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        query = query.WhereIsActive(isActiveOnly);
        if (!tracked)
            query = query.AsNoTracking();

        if (filter is not null)
            query = query.Where(filter);

        if (includeProperties is not null)
            foreach (var includedProperty in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includedProperty);

        return await query.FirstOrDefaultAsync() ?? default!;
    }

    /// <summary>
    /// Removes an entity from the repository.
    /// </summary>
    /// <param name="entity">The generic entity.</param>
    public void Remove(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
    }

    /// <summary>
    /// Removes a range of entities from the repository.
    /// </summary>
    /// <param name="entities">The list of generic entity.</param>
    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        context.Set<TEntity>().RemoveRange(entities);
    }

    /// <summary>
    /// Saves all changes made in this context to the underlying database.
    /// </summary>
    /// <returns>
    /// The integer value.
    /// </returns>
    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The generic entity.</param>
    /// <returns>
    /// The generic entity.
    /// </returns>
    public TEntity Update(TEntity entity)
    {
        context.Set<TEntity>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        return entity;
    }
}
