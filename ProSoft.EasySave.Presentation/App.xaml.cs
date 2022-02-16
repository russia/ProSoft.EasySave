using Prism.Ioc;
using System.Windows;
using ProSoft.EasySave.UI.Views;
using Prism.Regions;
using ProSoft.EasySave.Presentation.Views.PartialViews;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.UI.ViewModels;

namespace ProSoft.EasySave.UI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IFileService>();

            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(_HomeView));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(_ConfigView));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(_SaveView));

            containerRegistry.RegisterForNavigation<_HomeView, _HomeViewModel>(nameof(_HomeView));
        }
    }
}