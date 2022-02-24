using System.Windows;
using ModernWpf.Controls;
using Prism.Regions;
using ProSoft.EasySave.Presentation.Views.PartialViews;


namespace ProSoft.EasySave.Presentation.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IRegionManager _regionManager;

        public MainWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            _regionManager = regionManager;
        }

        private void NavigationView_SelectionChanged5(NavigationView sender,
            NavigationViewSelectionChangedEventArgs args)
        {
            var selectItemTag = args.SelectedItemContainer.Content;

            switch (selectItemTag)
            {
                case "Home":
                    _regionManager.RequestNavigate("ContentRegion", nameof(_HomeView));
                    break;

                case "Save in Progress":
                    _regionManager.RequestNavigate("ContentRegion", nameof(_SaveConfigView));
                    break;

                case "Save selection":
                    _regionManager.RequestNavigate("ContentRegion", nameof(_ListSaveView));
                    break;

                case "Configuration":
                    _regionManager.RequestNavigate("ContentRegion", nameof(_AppConfigView));
                    break;

                default:
                    _regionManager.RequestNavigate("ContentRegion", nameof(_HomeView));
                    break;
            }
        }
    }

}