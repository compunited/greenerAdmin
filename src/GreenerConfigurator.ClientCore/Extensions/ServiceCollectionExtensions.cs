using GreenerConfigurator.ClientCore.Options;
using GreenerConfigurator.ClientCore.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GreenerConfigurator.ClientCore.Extensions;

/// <summary>
/// Helper extensions that register the ClientCore services and default options in an IoC container.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGreenerClientCore(this IServiceCollection services, Action<GreenerApiOptions>? configure = null)
    {
        services.AddOptions<GreenerApiOptions>();
        if (configure != null)
        {
            services.Configure(configure);
        }

        services.AddScoped<ApiClient>();
        services.AddScoped<LocationService>();
        services.AddScoped<RuleService>();
        return services;
    }
}
