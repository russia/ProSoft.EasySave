using Prism.Regions;
using System.Windows;

namespace ProSoft.EasySave.Presentation.ViewModels
{
    internal class _SaveViewModel
    {
        private readonly IRegionManager _regionManager;

        public _SaveViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

        }

    }
}
