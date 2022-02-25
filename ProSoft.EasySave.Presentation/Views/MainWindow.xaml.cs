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
            var selectItemTag = args.SelectedItemContainer.Tag;

            switch (selectItemTag)
            {
                case "_HomeView":
                    _regionManager.RequestNavigate("ContentRegion", nameof(_HomeView));
                    break;

                case "_SaveView":
                    _regionManager.RequestNavigate("ContentRegion", nameof(_SaveConfigView));
                    break;

                case "_ActiView":
                    _regionManager.RequestNavigate("ContentRegion", nameof(_ListSaveView));
                    break;

                case "_ConfigView":
                    _regionManager.RequestNavigate("ContentRegion", nameof(_AppConfigView));
                    break;

                default:
                    _regionManager.RequestNavigate("ContentRegion", nameof(_HomeView));
                    break;
            }
        }
    }

}