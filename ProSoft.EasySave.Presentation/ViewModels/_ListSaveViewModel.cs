using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProSoft.EasySave.Presentation.ViewModels
{

    internal class _ListSaveViewModel
        {
            private readonly IRegionManager _regionManager;
            public double screenHeight { get; set; }

            public _ListSaveViewModel(IRegionManager regionManager)
            {
                _regionManager = regionManager;
                screenHeight = SystemParameters.FullPrimaryScreenHeight;
            }
        }
}
