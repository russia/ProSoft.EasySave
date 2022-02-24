using System;
using System.Globalization;
using System.Security.Principal;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prism.Container.Extensions;
using Microsoft.Win32;
using Prism.Ioc;
using Prism.Regions;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.Infrastructure.Models;
using ProSoft.EasySave.Infrastructure.Services;
using ProSoft.EasySave.Presentation.ViewModels;
using ProSoft.EasySave.Presentation.Views;
using ProSoft.EasySave.Presentation.Views.PartialViews;
using Serilog;
using Serilog.Events;

namespace ProSoft.EasySave.Presentation
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

        private const string RegistryValueName = "AppsUseLightTheme";

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("AppSettings.json", true, true);

            var configuration = builder.Build();

            var containerExtension = base.CreateContainerExtension();

            containerExtension.RegisterServices(
                services =>
                {
                    services.Configure<Configuration>(configuration.GetSection("Configuration"));
                });

            return containerExtension;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Logger(
                    // Hack to allow us to write to two different files and choose which one.
                    x => x.Filter.ByIncludingOnly(y => y.Level is LogEventLevel.Information)
                        .WriteTo.File(
                            @"./logs/logs.json",
                            outputTemplate:
                            "{Message:lj}{NewLine}",
                            rollingInterval: RollingInterval.Day,
                            shared: true))
                .WriteTo.Logger(
                    // Hack to allow us to write to two different files and choose which one.
                    x => x.Filter.ByIncludingOnly(y => y.Level is LogEventLevel.Warning)
                        .WriteTo.File(
                            @"./logs/logs.xml",
                            outputTemplate:
                            "{Message:lj}{NewLine}",
                            rollingInterval: RollingInterval.Day,
                            shared: true))
                .CreateLogger();

            containerRegistry.RegisterInstance(Log.Logger);
            containerRegistry.RegisterSingleton<IGlobalizationService, GlobalizationService>();
            containerRegistry.RegisterSingleton<IFileService, FileService>();
            containerRegistry.RegisterSingleton<IJobFactoryService, JobFactoryService>();

            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(_HomeView));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(_AppConfigView));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(_ListSaveView));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(_SaveConfigView));

            containerRegistry.RegisterForNavigation<_HomeView, _HomeViewModel>(nameof(_HomeView));
            containerRegistry.RegisterForNavigation<_AppConfigView, _AppConfigViewModel>(nameof(_AppConfigView));
            containerRegistry.RegisterForNavigation<_ListSaveView, _ListSaveViewModel>(nameof(_ListSaveView));
            containerRegistry.RegisterForNavigation<_SaveConfigView, _SaveConfigViewModel>(nameof(_SaveConfigView));
            containerRegistry.RegisterDialog<_SaveConfigView, _SaveConfigViewModel>("SaveConfigView");
        }

        private enum WindowsTheme
        {
            Light,
            Dark
        }

        public void WatchTheme()
        {
            var currentUser = WindowsIdentity.GetCurrent();
            string query = string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT * FROM RegistryValueChangeEvent WHERE Hive = 'HKEY_USERS' AND KeyPath = '{0}\\{1}' AND ValueName = '{2}'",
                currentUser.User.Value,
                RegistryKeyPath.Replace(@"\", @"\\"),
                RegistryValueName);

            try
            {
                var watcher = new ManagementEventWatcher(query);
                watcher.EventArrived += (sender, args) =>
                {
                    WindowsTheme newWindowsTheme = GetWindowsTheme();
                    // React to new theme
                };

                // Start listening for events
                watcher.Start();
            }
            catch (Exception)
            {
                // This can fail on Windows 7
            }

            WindowsTheme initialTheme = GetWindowsTheme();
        }

        private static WindowsTheme GetWindowsTheme()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
            {
                object registryValueObject = key?.GetValue(RegistryValueName);
                if (registryValueObject == null)
                {
                    return WindowsTheme.Light;
                }

                int registryValue = (int)registryValueObject;

                return registryValue > 0 ? WindowsTheme.Light : WindowsTheme.Dark;
            }
        }
    }
}