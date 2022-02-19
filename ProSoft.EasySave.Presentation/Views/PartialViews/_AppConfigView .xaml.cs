using System.Windows.Controls;
using Prism.Regions;

namespace ProSoft.EasySave.Presentation.Views.PartialViews
{
    /// <summary>
    ///     Interaction logic for ConfigView.xaml
    /// </summary>
    public partial class _AppConfigView : UserControl
    {
        private IRegionManager _regionManager;

        public _AppConfigView(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            InitializeComponent();
        }
    }
}
