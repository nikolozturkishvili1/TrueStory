using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrueStory.Application.Infrastructure.RestFul.ServiceImplementators;
using TrueStory.Infrastructure.Service.restful_api.dev.RestClient;
using TrueStory.Infrastructure.Service.restful_api.dev.Service;

namespace TrueStory.Infrastructure.Extensions;

/// <summary>
/// Provides extension methods to register infrastructure services.
/// </summary>
public static class InfrastructureServiceExtensions
{
    /// <summary>
    /// Registers infrastructure services in the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRestFulServiceImplementator, RestFulService>();
        services.AddScoped<IRestFulRestClient, RestFulClient>();


        return services;
    }
}
