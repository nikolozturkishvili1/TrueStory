using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrueStory.Domain.Interfaces;

namespace TrueStory.Persistence.Context;

/// <summary>
/// Initializes the Person Registry database with default seed data.
/// </summary>
/// <param name="_dbContext">The database context.</param>
public static class ProductDbInitializer
{
    /// <summary>
    /// Seeds initial data into the database if it is empty.
    /// </summary>
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<ProductDbContext>>();

        try
        {
            logger.LogInformation("Initializing database...");
            var context = services.GetRequiredService<ProductDbContext>();

            logger.LogInformation("Ensuring database exists...");
            await context.Database.EnsureCreatedAsync();

            logger.LogInformation("Applying migrations...");
            await context.Database.MigrateAsync();
       

            logger.LogInformation("Database initialization completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

}