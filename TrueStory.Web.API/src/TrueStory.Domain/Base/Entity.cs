namespace TrueStory.Domain.Base;

/// <summary>
/// Represents a base entity with an identifier and soft delete functionality.
/// </summary>
/// <typeparam name="T">The type of the identifier.</typeparam>
public abstract class Entity<T>
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    public virtual T ID { get; protected set; }
   
}
