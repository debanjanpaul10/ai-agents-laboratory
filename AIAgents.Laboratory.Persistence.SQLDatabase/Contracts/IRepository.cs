using System.Linq.Expressions;

namespace AIAgents.Laboratory.Persistence.SQLDatabase.Contracts;

/// <summary>
/// Interface definition of the contract for a generic repository pattern.
/// </summary>
/// <typeparam name="TEntity">The generic Entity type.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The generic entity.</param>
    /// <returns>The generic entity.</returns>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// Adds a range of entities to the repository.
    /// </summary>
    /// <param name="entities">The list of generic entity.</param>
    /// <returns>The list of generic entity.</returns>
    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Finds entities based on the provided predicate.
    /// </summary>
    /// <param name="predicate">The entity finder predicate.</param>
    /// <returns>The list of generic entity.</returns>
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Gets all entities from the repository.
    /// </summary>
    /// <param name="filter">The filter predicate.</param>
    /// <param name="includeProperties">The included properties string.</param>
    /// <param name="isActiveOnly">The isactive only boolean flag.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The list of generic entity.</returns>
    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1, bool isActiveOnly = true);

    /// <summary>
    /// Gets all entities from the repository with pagination.
    /// </summary>
    /// <param name="filter">The filter predicate.</param>
    /// <param name="tracked">The is tracked boolean flag.</param>
    /// <param name="includeProperties">The included properties string.</param>
    /// <param name="isActiveOnly">The isactive only boolean flag.</param>
    /// <returns>The generic entity.</returns>
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>>? filter = null, bool tracked = true, string? includeProperties = null, bool isActiveOnly = true);

    /// <summary>
    /// Gets all entities from the repository with pagination.
    /// </summary>
    /// <param name="filter">The filter predicate.</param>
    /// <param name="orderByProperty">The order by property.</param>
    /// <param name="ascending">The ascending sort order boolean flag.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="includeProperties">The included properties string.</param>
    /// <returns>A tupple containing values.</returns>
    Task<(List<TEntity>, int, bool)> GetAllPagedAsync(
        Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>? orderByProperty = null, bool ascending = true,
        int pageSize = 1000, int pageNumber = 1, params Expression<Func<TEntity, object>>[] includeProperties);

    /// <summary>
    /// Gets the first entity that matches the provided predicate.
    /// </summary>
    /// <param name="predicate">The filter predicate.</param>
    /// <returns>The generic entity.</returns>
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Removes an entity from the repository.
    /// </summary>
    /// <param name="entity">The generic entity.</param>
    void Remove(TEntity entity);

    /// <summary>
    /// Removes a range of entities from the repository.
    /// </summary>
    /// <param name="entities">The list of generic entity.</param>
    void RemoveRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The generic entity.</param>
    /// <returns>The generic entity.</returns>
    TEntity Update(TEntity entity);

    /// <summary>
    /// Saves all changes made in this context to the underlying database.
    /// </summary>
    /// <returns>The integer value.</returns>
    Task<int> SaveChangesAsync();
}
