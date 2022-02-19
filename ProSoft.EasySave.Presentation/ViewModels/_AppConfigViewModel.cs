using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProSoft.EasySave.Presentation.ViewModels
{

    internal class _AppConfigViewModel
    {
        private readonly IRegionManager _regionManager;
        public double screenHeight { get; set; }

        public _AppConfigViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            screenHeight = 1980;
        }
    }
}
