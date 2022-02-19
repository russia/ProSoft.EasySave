using System.Windows;
using Prism.Regions;

namespace ProSoft.EasySave.Presentation.ViewModels
{
    public class _HomeViewModel
    {
        private readonly IRegionManager _regionManager;
        public int screenHeight { get; set; }

        public _HomeViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            screenHeight = 1980;
        }

    }
}