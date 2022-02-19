using System.Windows.Controls;
using Prism.Regions;

namespace ProSoft.EasySave.Presentation.Views.PartialViews
{
    /// <summary>
    ///     Interaction logic for HomeView.xaml
    /// </summary>
    public partial class _HomeView : UserControl
    {
        private readonly IRegionManager _regionManager;

        public _HomeView(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            InitializeComponent();
        }
    }
}