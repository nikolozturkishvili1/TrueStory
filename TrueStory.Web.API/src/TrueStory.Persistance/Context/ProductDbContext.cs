using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TrueStory.Domain.Base;
namespace TrueStory.Persistence.Context;

/// <summary>
/// Represents the Entity Framework Core database context for the Person Registry.
/// </summary>
public class ProductDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductDbContext"/> class.
    /// </summary>
    /// <param name="dbContextOptions">The database context options.</param>
    /// <param name="configuration">The application configuration.</param>
    public ProductDbContext(DbContextOptions<ProductDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    /// <summary>
    /// Configures the database context settings.
    /// This method is called when EF Core initializes the DbContext, 
    /// allowing additional configuration of database providers and options.
    /// </summary>
    /// <param name="optionsBuilder">The options builder used to configure the DbContext.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    /// <summary>
    /// Configures the database context settings.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(
          bool acceptAllChangesOnSuccess,
          CancellationToken cancellationToken = new CancellationToken()
      )
    {
        var commonObjectEntries = ChangeTracker.Entries()
            .Where(
                a => a.Entity.GetType().BaseType?.IsGenericType == true &&
                     a.Entity.GetType().BaseType?.GetGenericTypeDefinition() == typeof(BaseCommonObject<>) ||
                     a.Entity.GetType().BaseType == typeof(BaseCommonObject)
            )
            .ToList();

        foreach (var entry in commonObjectEntries.Where(a => a.State == EntityState.Added))
        {
            entry.Property(nameof(BaseCommonObject.CreateDate)).IsModified = true;
            entry.Property(nameof(BaseCommonObject.CreateDate)).CurrentValue = DateTime.Now;

        }

        foreach (var entry in commonObjectEntries.Where(a => a.State == EntityState.Modified))
        {
            entry.Property(nameof(BaseCommonObject.DateModified)).IsModified = true;
            entry.Property(nameof(BaseCommonObject.DateModified)).CurrentValue = DateTime.Now;

            entry.Property(nameof(BaseCommonObject.CreateDate)).IsModified = false;
        }

        foreach (var entry in commonObjectEntries.Where(a => a.State == EntityState.Deleted))
        {
            entry.Property(nameof(BaseCommonObject.DateModified)).IsModified = true;
            entry.Property(nameof(BaseCommonObject.DateModified)).CurrentValue = DateTime.Now;

            entry.Property(nameof(BaseCommonObject.IsDeleted)).IsModified = true;
            entry.Property(nameof(BaseCommonObject.IsDeleted)).CurrentValue = true;

            entry.Property(nameof(BaseCommonObject.CreateDate)).IsModified = false;

            entry.State = EntityState.Modified;
        }


        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

}
