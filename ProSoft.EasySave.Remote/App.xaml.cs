using System;
using System.Reflection;
using System.Windows;
using Prism.Ioc;
using Prism.Regions;
using ProSoft.EasySave.Infrastructure.Interfaces.Network.Dispatcher;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.Infrastructure.Models.Network.Dispatcher;
using ProSoft.EasySave.Remote.Models;
using ProSoft.EasySave.Remote.Models.Network;
using ProSoft.EasySave.Remote.Models.Network.Frames;
using ProSoft.EasySave.Remote.ViewModels;
using ProSoft.EasySave.Remote.Views;
using ProSoft.EasySave.Remote.Views.PartialViews;

namespace ProSoft.EasySave.Remote
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
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
            var assembly = Assembly.GetAssembly(typeof(ServerFrame));

            if (assembly is null) throw new ArgumentNullException("Assembly cannot be null.");

            var packetReceiver = new PacketReceiver(assembly);
            var easySaveContext = new EasySaveContext();
            var client = new Client(packetReceiver, easySaveContext);

            // TODO : find a workaround..
            easySaveContext.SetClient(client);

            containerRegistry.RegisterInstance(easySaveContext);
            containerRegistry.RegisterInstance(client);
            containerRegistry.RegisterSingleton<IPacketReceiver>();
            containerRegistry.RegisterSingleton<IFileService>();
            var regionManager = Container.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion("MainRegion", typeof(_SocketPartialView));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(_ControllerPartialView));
            containerRegistry.RegisterForNavigation<_SocketPartialView, _SocketPartialViewModel>(
                nameof(_SocketPartialView));
            containerRegistry.RegisterForNavigation<_ControllerPartialView, _ControllerPartialViewModel>(
                nameof(_ControllerPartialView));
        }
    }
}