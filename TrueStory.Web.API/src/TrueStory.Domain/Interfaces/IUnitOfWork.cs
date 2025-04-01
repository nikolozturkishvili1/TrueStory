using TrueStory.Domain.Aggregate.Product;

namespace TrueStory.Domain.Interfaces;

/// <summary>
/// Defines the Unit of Work pattern, ensuring all database operations are managed under a single transaction.
/// </summary>
public interface IUnitOfWork : IDisposable
{

    /// <summary>
    /// Gets the repository for the <see cref="T_Product"/> entity.
    /// </summary>
    IProductRepository ProductRepository { get; }

    /// <summary>
    /// Begins a database transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the transaction if needed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current database transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the commit if needed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current database transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the rollback if needed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists changes made in the current transaction.
    /// </summary>
    /// <returns>The number of affected database rows.</returns>
    int SaveChanges();

    /// <summary>
    /// Persists changes made in the current transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the number of affected rows.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}