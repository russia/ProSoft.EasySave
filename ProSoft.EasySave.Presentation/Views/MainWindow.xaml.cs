using ProSoft.EasySave.Presentation.Views.PartialViews;
using System.Windows;


namespace ProSoft.EasySave.Presentation.Views {


/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
        public void NavigationView_SelectionChanged5(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            /*if (args.IsSettingsSelected)
            {
                contentFrame5.Navigate(typeof(SampleSettingsPage));
            }
            else
            {*/
                var selectedItem = (ModernWpf.Controls.NavigationViewItem)args.SelectedItem;
                string selectedItemTag = (string)selectedItem.Tag;
                sender.Header = "Sample Page " + selectedItemTag.Substring(selectedItemTag.Length - 1);
                string pageName = "SamplesCommon.SamplePages." + selectedItemTag;
                var pageType = typeof(_SaveView).Assembly.GetType(pageName);
                contentFrame5.Navigate(pageType);
            //}
        }
    }
}