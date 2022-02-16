using Prism.Regions;
using ProSoft.EasySave.Presentation.Views.PartialViews;
using System.Collections;
using ProSoft.EasySave.Infrastructure.Services;
using System.Windows;
using System.Collections.Generic;

namespace ProSoft.EasySave.UI.ViewModels
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

            //jobJson = FileService.readJson();
        }
    }
}