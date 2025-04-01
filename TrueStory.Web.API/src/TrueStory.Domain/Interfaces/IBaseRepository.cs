using System.Linq.Expressions;
using TrueStory.Domain.Base;

namespace TrueStory.Domain.Interfaces;

/// <summary>
/// Generic repository interface that defines standard CRUD operations for an entity.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public interface IBaseRepository<T, TKey> where T : Entity<TKey>
{
    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<T> GetByIdAsync(TKey id);

    /// <summary>
    /// Retrieves all entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <returns>A collection of all entities.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks an entity for deletion.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(T entity);

    /// <summary>
    /// Counts the number of entities in a given query.
    /// </summary>
    /// <param name="entities">The queryable collection of entities.</param>
    /// <returns>The count of entities.</returns>
    Task<int> CountAsync(IQueryable<T> entities);

    /// <summary>
    /// Checks if any entity matches the given condition.
    /// </summary>
    /// <param name="predicate">The condition to check.</param>
    /// <returns>True if at least one entity matches; otherwise, false.</returns>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a single entity that matches the given condition.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Returns a queryable collection of entities, optionally including related data.
    /// </summary>
    /// <param name="includes">Navigation properties to include.</param>
    /// <returns>A queryable collection of entities.</returns>
    IQueryable<T> Query(params Expression<Func<T, object>>[] includes);
}