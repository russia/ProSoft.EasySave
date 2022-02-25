using System.Threading;
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

            Properties.Settings.Default.languageCode = "en-US";
            Properties.Settings.Default.Save();

            InitializeComponent();
        }
        

        public void ChangeLanguage(object sender, SelectedCellsChangedEventArgs e)
        {
           
                Properties.Settings.Default.languageCode = "fr_FR";
            
                //Properties.Settings.Default.languageCode = "fr-US";

                Properties.Settings.Default.Save();
            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb.SelectedIndex == 0)
            {
                Properties.Settings.Default.languageCode = "en_US";
            }
            else
            {
                Properties.Settings.Default.languageCode = "fr-FR";
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr_FR");

            }

            Properties.Settings.Default.Save();
        }
    }
}