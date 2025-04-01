using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonRegistry.Persistence.Repositories;
using TrueStory.Domain.Aggregate.Product;
using TrueStory.Domain.Interfaces;
using TrueStory.Persistence.Context;
using TrueStory.Persistence.Repositories.UnitOfWork;

namespace TrueStory.Persistance.Extensions;

/// <summary>
/// Provides extension methods for configuring persistence services.
/// </summary>
public static class PersistenceServiceExtension
{
    /// <summary>
    /// Adds database and repository services to the application's dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration containing database connection settings.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
    IConfiguration configuration)
    {
        services.AddDbContext<ProductDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));


        /// Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register repositories
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}