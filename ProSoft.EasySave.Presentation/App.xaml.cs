using Prism.Ioc;
using System.Windows;
using ProSoft.EasySave.Presentation.Views;
using Prism.Regions;
using ProSoft.EasySave.Presentation.Views.PartialViews;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.Presentation.ViewModels;

namespace ProSoft.EasySave.Presentation
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
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(_AppConfigView));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(_ListSaveView));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(_SaveConfigView));

            containerRegistry.RegisterForNavigation<_HomeView, _HomeViewModel>(nameof(_HomeView));
            containerRegistry.RegisterForNavigation<_AppConfigView, _AppConfigViewModel>(nameof(_AppConfigView));
            containerRegistry.RegisterForNavigation<_ListSaveView, ViewModels._ListSaveViewModel>(nameof(_ListSaveView));
            containerRegistry.RegisterForNavigation<_SaveConfigView, _SaveConfigViewModel>(nameof(_SaveConfigView));
        }
    }
}