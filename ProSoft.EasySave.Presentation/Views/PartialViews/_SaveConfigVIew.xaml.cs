using System.Windows.Controls;
using Prism.Regions;

namespace ProSoft.EasySave.Presentation.Views.PartialViews
{
    /// <summary>
    ///     Interaction logic for ActivityView.xaml
    /// </summary>
    public partial class _SaveConfigView : UserControl
    {
        private IRegionManager _regionManager;

        public _SaveConfigView(IRegionManager regionManager)
        {
            InitializeComponent();
            _regionManager = regionManager;
        }
    }
}
