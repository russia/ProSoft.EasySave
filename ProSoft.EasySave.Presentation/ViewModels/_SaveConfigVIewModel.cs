using Prism.Regions;
using System.Windows;
namespace ProSoft.EasySave.Presentation.ViewModels
{
    internal class _SaveConfigViewModel
    {
        private readonly IRegionManager _regionManager;
        public double screenHeight { get; set; }


        public _SaveConfigViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            screenHeight = SystemParameters.FullPrimaryScreenHeight;
        }
    }
}

