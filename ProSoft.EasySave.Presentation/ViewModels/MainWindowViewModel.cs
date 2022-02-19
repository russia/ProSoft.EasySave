using System.Windows;
using System.Windows.Controls;
using Prism.Regions;
using ProSoft.EasySave.Presentation.Views.PartialViews;

namespace ProSoft.EasySave.Presentation.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly IRegionManager _regionManager;

        public jobJson jodJson;

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RequestNavigate("ContentRegion", nameof(_HomeView));
            screenHeight = (int)SystemParameters.FullPrimaryScreenHeight;
            var jodJson = new jobJson();
            jodJson.name = "Save1";
            jodJson.FileSource = "C://.//Save";
            jodJson.FileTarget = "D://.//Save";
            jodJson.FileSize = 1651.ToString();
        }

        public int screenHeight { get; set; }

        public class jobJson
        {
            public string name { get; set; }
            public string FileSource { get; set; }
            public string FileTarget { get; set; }
            public string FileSize { get; set; }
            public StackPanel button { get; set; }
        }
    }
}