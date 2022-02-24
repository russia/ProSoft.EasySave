using System.Windows;
using Prism.Commands;
using Prism.Regions;
using ProSoft.EasySave.Presentation.Views.PartialViews;

namespace ProSoft.EasySave.Presentation.ViewModels
{
    public class _HomeViewModel
    {
        private readonly IRegionManager _regionManager;
        public int screenHeight { get; set; }
        public DelegateCommand GoToSettingsCommand { get; set; }
        public DelegateCommand GoToSaveList { get; set; }

        public _HomeViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            screenHeight = 1980;

            GoToSettingsCommand = new DelegateCommand(() =>
                {
                    _regionManager.RequestNavigate("ContentRegion", nameof(_AppConfigView));
                });

            GoToSaveList = new DelegateCommand(() =>
            {
                _regionManager.RequestNavigate("ContentRegion", nameof(_ListSaveView));
            });
        }
    }
}