using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PersonRegistry.Application.Common.Behaviours;
using System.Reflection;

namespace TrueStory.Application.Extensions;

/// <summary>
/// Provides extension methods for registering application-layer services.
/// </summary>
public static class ApplicationServiceExtension
{
    /// <summary>
    /// Registers MediatR, FluentValidation, and pipeline behaviors in the service collection.
    /// </summary>
    /// <param name="services">The service collection to register dependencies into.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register MediatR with the current assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        // Register all FluentValidation validators in the current assembly
        services.AddValidatorsFromAssembly(assembly);

        // Register pipeline behaviors for transaction and validation handling
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestTransactionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

        return services;
    }
}