using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProSoft.EasySave.UI.Views;
using ProSoft.EasySave.Presentation.Views.PartialViews;
using System.Windows;

namespace ProSoft.EasySave.UI.ViewModels
{
    public class _HomeViewModel
    {
        private readonly IRegionManager _regionManager;
        public int screenHeight { get; set; }

        public _HomeViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            screenHeight = (int)SystemParameters.FullPrimaryScreenHeight;
        }
    }
}