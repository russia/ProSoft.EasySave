using System.Windows;
using Prism.Regions;

namespace ProSoft.EasySave.Presentation.ViewModels
{
    internal class _AppConfigViewModel
    {
        private readonly IRegionManager _regionManager;

        public _AppConfigViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            screenHeight = SystemParameters.FullPrimaryScreenHeight;
        }

        public double screenHeight { get; set; }
    }
}