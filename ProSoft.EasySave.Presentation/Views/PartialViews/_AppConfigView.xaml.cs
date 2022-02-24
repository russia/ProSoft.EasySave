using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Prism.Regions;

namespace ProSoft.EasySave.Presentation.Views.PartialViews
{
    /// <summary>
    ///     Interaction logic for ConfigView.xaml
    /// </summary>
    public partial class _AppConfigView : UserControl
    {
        private IRegionManager _regionManager;
        private TextBox pathTxt;

        public _AppConfigView(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            InitializeComponent();
        }

    }
}
