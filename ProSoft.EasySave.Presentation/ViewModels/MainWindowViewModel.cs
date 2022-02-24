using System.Windows;
using System.Windows.Controls;
using Prism.Regions;
using ProSoft.EasySave.Presentation.Views.PartialViews;
using System.Collections;
using ProSoft.EasySave.Infrastructure.Services;

using System.Windows;

using System.Collections.Generic;

using System.Windows.Controls;

namespace ProSoft.EasySave.Presentation.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly IRegionManager _regionManager;
        public List<string> jobJson;
        public int screenHeight { get; set; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RequestNavigate("ContentRegion", nameof(_HomeView));
            screenHeight = (int)SystemParameters.FullPrimaryScreenHeight;
            //var jodJson = new jobJson();
            //jodJson.name = "Save1";
            //jodJson.FileSource = "C://.//Save";
            //jodJson.FileTarget = "D://.//Save";
            //jodJson.FileSize = 1651.ToString();
        }
    }
}