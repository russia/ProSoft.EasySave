using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ProSoft.EasySave.Remote.Views.PartialViews;

namespace ProSoft.EasySave.Remote.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RequestNavigate("MainRegion", nameof(_ControllerPartialView));
            GoToControllerViewCommand = new DelegateCommand(() =>
                _regionManager.RequestNavigate("MainRegion", nameof(_ControllerPartialView)));
            GoToSocketViewCommand = new DelegateCommand(() =>
                _regionManager.RequestNavigate("MainRegion", nameof(_SocketPartialView)));
        }

        public DelegateCommand GoToControllerViewCommand { get; }
        public DelegateCommand GoToSocketViewCommand { get; }
    }
}