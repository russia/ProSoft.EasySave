using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProSoft.EasySave.Presentation.Views.PartialViews
{
    /// <summary>
    /// Interaction logic for _SaveView.xaml
    /// </summary>
    public partial class _SaveView : UserControl
    {
        private IRegionManager _regionManager;


        public _SaveView(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            InitializeComponent();

        }

        private void NavigationView_SelectionChanged5(ModernWpf.Controls.NavigationView sender,
             ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = args.SelectedItem;
            var selectItemTag = selectedItem.ToString()[^9..];


            switch (selectItemTag)
            {
                case "_SaveView":
                    _regionManager.RequestNavigate("ContentRegion", nameof(_SaveView));
                    break;

                case "_ConfView":
                    _regionManager.RequestNavigate("ContentRegion", nameof(_ConfigView));
                    break;

                case "_HomeView":
                    _regionManager.RequestNavigate("ContentRegion", nameof(_HomeView));
                    break;

                default:
                    _regionManager.RequestNavigate("ContentRegion", nameof(_HomeView));
                    break;
            }

        }
    }
      
}