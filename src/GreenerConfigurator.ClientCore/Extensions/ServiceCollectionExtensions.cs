using GreenerConfigurator.ClientCore.Options;
using GreenerConfigurator.ClientCore.Services;
using GreenerConfigurator.ClientCore.Services.Rule;
using GreenerConfigurator.ClientCore.Services.Notification;
using GreenerConfigurator.ClientCore.Services.Location;
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

        services.AddHttpClient<IApiService, ApiService>();
        
        services.AddScoped<LocationService>();
        services.AddScoped<RuleService>();
        services.AddScoped<NotificationGroupService>();
        services.AddScoped<CompareConditionService>();
        services.AddScoped<LocationDetailService>();
        services.AddScoped<DeviceStateService>();
        services.AddScoped<LogicalDeviceService>();
        services.AddScoped<LoraWanDataRowsService>();
        services.AddScoped<NavigationCardGroupService>();
        services.AddScoped<NavigationCardService>();
        services.AddScoped<NavigationCategoryService>();
        services.AddScoped<NetworkDeviceService>();
        services.AddScoped<PhysicalDeviceService>();
        services.AddScoped<PlanViewService>();
        services.AddScoped<SimCardService>();
        services.AddScoped<UnassignedPhysicalDeviceService>();
        services.AddScoped<UserSettingsService>();
        services.AddScoped<BusConnectionService>();
        services.AddScoped<ActiveTimeService>();
        services.AddScoped<NotificationGroupDataService>();
        services.AddScoped<RuleDetailService>();

        return services;
    }
}
