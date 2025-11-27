using GreenerConfigurator.Properties;

using GreenerConfigurator.ViewModels;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Desktop;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Extensions.DependencyInjection;
using GreenerConfigurator.ClientCore.Services;

using GreenerConfigurator.ClientCore.Services.Location;
using GreenerConfigurator.ClientCore.Services.Notification;

namespace GreenerConfigurator
{
    /// <summary>
    /// Stellt das anwendungsspezifische Verhalten bereit, um die Standardanwendungsklasse zu ergänzen.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initialisiert das Singletonanwendungsobjekt. Dies ist die erste Zeile von erstelltem Code
        /// und daher das logische Äquivalent von main() bzw. WinMain().
        /// </summary>
        public App()
        {
            UIParent = this;
        }

        public static object UIParent { get; set; } = null;
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var defCulture = Settings.Default.CurrentLanguage;

            var _CurrentCulture = new CultureInfo(defCulture);
            Thread.CurrentThread.CurrentCulture = _CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = _CurrentCulture;

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IApiService, ApiService>(client =>
            {
                string tempServerUrl = string.Empty;
#if DEV_PC
                tempServerUrl = "https://localhost:5001";
#else
                tempServerUrl = "https://api.greener.software";
#endif
                client.BaseAddress = new Uri(tempServerUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.Timeout = TimeSpan.FromSeconds(120);
            });

            services.AddTransient<LocationService>();
            services.AddTransient<NetworkDeviceService>();
            services.AddTransient<PhysicalDeviceService>();
            services.AddTransient<LogicalDeviceService>();
            services.AddTransient<GreenerConfigurator.ClientCore.Services.Rule.RuleService>();
            services.AddTransient<GreenerConfigurator.ClientCore.Services.Rule.ActiveTimeService>();
            services.AddTransient<GreenerConfigurator.ClientCore.Services.Rule.CompareConditionService>();
            services.AddTransient<GreenerConfigurator.ClientCore.Services.Rule.NotificationGroupDataService>();
            services.AddTransient<GreenerConfigurator.ClientCore.Services.Rule.RuleDetailService>();
            services.AddTransient<NotificationGroupService>();
            services.AddTransient<LocationDetailService>();
            services.AddTransient<SimCardService>();
            services.AddTransient<NavigationCategoryService>();
            services.AddTransient<NavigationCardService>();
            services.AddTransient<NavigationCardGroupService>();
            services.AddTransient<BusConnectionService>();
            services.AddTransient<DeviceStateService>();
            services.AddTransient<LoraWanDataRowsService>();
            services.AddTransient<PlanViewService>();
            services.AddTransient<UserSettingsService>();
            services.AddTransient<GreenerConfigurator.ClientCore.Services.UnassignedPhysicalDeviceService>();
        }

        public static void CurrentView(ViewModelBase currentViewModel)
        {
            ((MainWindow)App.Current.MainWindow).MainWindowModel.Navigator.CurrentViewModel = currentViewModel;
        }
    }
}

