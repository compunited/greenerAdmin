using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GreenerConfigurator.Web.Client;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using AKSoftware.Localization.MultiLanguages;
using Radzen;
using System.Globalization;
using System.Reflection;
using Serilog;
using GreenerConfigurator.ClientCore.Services;
using GreenerConfigurator.ClientCore.Services.Rule;
using GreenerConfigurator.ClientCore.Services.Location;
using GreenerConfigurator.ClientCore.Utilities;

namespace GreenerConfigurator.Web.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebAssemblyHostBuilder.CreateDefault(args);
                var applicationBaseAddress = builder.HostEnvironment.BaseAddress;

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    //.WriteTo.BrowserConsole() // Removed to fix build error
                    .CreateLogger();
                builder.Logging.ClearProviders();
                builder.Logging.AddFilter("Microsoft.AspNetCore.Components.WebAssembly.Authentication", LogLevel.None);

                builder.Logging.AddSerilog();

                builder.RootComponents.Add<App>("#app");
                builder.RootComponents.Add<HeadOutlet>("head::after");

                builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly(),
                    CultureInfo.GetCultureInfo("de-DE"));
                builder.Services.AddScoped(sp => new HttpClient
                    { BaseAddress = new Uri(applicationBaseAddress) });
                builder.Services.AddScoped<Radzen.DialogService>();
                builder.Services.AddScoped<Radzen.TooltipService>();


                // Register CustomAuthorizationMessageHandler
                builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

                // Configure HttpClient for ServerAPI (if needed for local calls)
                builder.Services.AddHttpClient("ServerAPI", client => 
                        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

                // Configure GreenerApi to use the external API with CustomAuthorizationMessageHandler
                builder.Services.AddHttpClient("GreenerApi", client => 
                        client.BaseAddress = new Uri("https://api.greener.software"))
                    .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

                builder.Services.AddMsalAuthentication(options =>
                {
                    var azureB2COptions = builder.Configuration.GetSection("AzureB2C").Get<AzureB2COptions>()
                        ?? AzureB2COptions.CreateDefault();

                    options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
                    options.ProviderOptions.DefaultAccessTokenScopes.Add("offline_access");
                   
                    
                    options.ProviderOptions.LoginMode = "redirect"; //important... dont remove. otherwise popup filter blocks login


                    options.ProviderOptions.Authentication.Authority = azureB2COptions.Authority;
                    options.ProviderOptions.Authentication.ClientId = azureB2COptions.ClientId;
                    options.ProviderOptions.Authentication.ValidateAuthority = azureB2COptions.ValidateAuthority;
                });
                
                var brandingOptions = builder.Configuration.GetSection("Branding").Get<BrandingOptions>()
                    ?? BrandingOptions.CreateDefault();
                // SessionHelper might not exist
                // SessionHelper.ClientBackgroundColor = brandingOptions.ClientBackgroundColor;

                ManageDependencies(builder);


                var host = builder.Build();

                // LogHelper might not exist or be different
                // LogHelper.ConfigureLogger(host.Services.GetRequiredService<ILoggerFactory>());
                // await LogHelper.WriteLineAsync(applicationBaseAddress);

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                // await LogHelper.WriteErrorAsync($"Error while initialization:", ex);
                Console.WriteLine($"Error while initialization: {ex}");
                throw;
            }
        }


        private static void ManageDependencies(WebAssemblyHostBuilder builder)
        {
            var tenantOptions = builder.Configuration.GetSection("Tenant").Get<TenantOptions>()
                ?? TenantOptions.CreateDefault();

            /* Missing Services/Helpers - Commented out for now
            builder.Services.AddSingleton<IApplicationParameterHelper, ApplicationParameterHelper>
            (serviceProvider => new ApplicationParameterHelper(
                tenantId: tenantOptions.Id,
                tenantName: tenantOptions.Name,
                signInPolicy: tenantOptions.SignInPolicy
            ));

            builder.Services.AddTransient<IAPIHelper, APIHelper>();
            builder.Services.AddTransient<ISessionHelper, SessionHelper>();
            builder.Services.AddTransient<ILogicalDeviceNavigationCategoryService, LogicalDeviceNavigationCategoryService>();
            builder.Services.AddTransient<ILogicalDeviceNavigationCardService, LogicalDeviceNavigationCardService>();
            builder.Services.AddTransient<IAlarmService, AlarmService>();
            builder.Services.AddTransient<IDownload, DownloadService>();
            builder.Services.AddTransient<ITenantService, TenantService>();
            builder.Services.AddTransient<IIotMessageDataService, IotMessageDataService>();
            builder.Services.AddTransient<IMeterDownload, MeterDownloadService>();
            builder.Services.AddSingleton<CollapseHelper>();
            builder.Services.AddSingleton<LocationStateProvider>();
            builder.Services.AddSingleton<GlobalFilterHelper>();
            builder.Services.AddSingleton<GlobalStateHelper>();
            builder.Services.AddScoped<DashboardState>();
            builder.Services.AddScoped<IDashboardContext, DashboardContext>();
            */

            // Existing Services - Registered as Concrete Types because Interfaces are missing in ClientCore
            builder.Services.AddTransient<UserSettingsService>();
            builder.Services.AddTransient<LocationService>();
            builder.Services.AddTransient<DeviceStateService>();
            builder.Services.AddTransient<PhysicalDeviceService>();
            builder.Services.AddTransient<LogicalDeviceService>();
            builder.Services.AddTransient<LoraWanDataRowsService>();
            builder.Services.AddTransient<PlanViewService>();
            
            // Register NetworkDeviceService and RuleService (from previous Program.cs)
            builder.Services.AddScoped<NetworkDeviceService>();
            builder.Services.AddScoped<RuleService>();
            
            // Register IApiService
             builder.Services.AddScoped<GreenerConfigurator.ClientCore.Services.IApiService>(sp => 
                new GreenerConfigurator.ClientCore.Services.ApiService(
                    sp.GetRequiredService<IHttpClientFactory>().CreateClient("GreenerApi"),
                    sp.GetRequiredService<ILogger<GreenerConfigurator.ClientCore.Services.ApiService>>()));
        }

        private sealed record AzureB2COptions
        {
            public string Authority { get; init; } = string.Empty;
            public string ClientId { get; init; } = string.Empty;
            public bool ValidateAuthority { get; init; } = false;

            public static AzureB2COptions CreateDefault() => new()
            {
                Authority = "https://greenerswkunden.b2clogin.com/customer.greener.software/B2C_1_signup_signin",
                ClientId = "09ad5890-e2b6-4798-a8f0-db69086e9e9f",
                ValidateAuthority = false
            };
        }

        private sealed record BrandingOptions
        {
            public string ClientBackgroundColor { get; init; } = "greener";

            public static BrandingOptions CreateDefault() => new();
        }

        private sealed record TenantOptions
        {
            public string Id { get; init; } = "9d7640c0-2f52-4db7-86dc-35aa2f93add5";
            public string Name { get; init; } = "greenerswkunden";
            public string SignInPolicy { get; init; } = "B2C_1_signup_signin";

            public static TenantOptions CreateDefault() => new();
        }
    }
}
