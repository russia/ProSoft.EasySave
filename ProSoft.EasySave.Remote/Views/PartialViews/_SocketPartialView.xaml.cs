using System.Windows.Controls;

namespace ProSoft.EasySave.Remote.Views.PartialViews
{
    /// <summary>
    ///     Interaction logic for _SocketPartialView.xaml
    /// </summary>
    public partial class _SocketPartialView : UserControl
    {
        public _SocketPartialView()
        {
            InitializeComponent();
        }

        // TODO : auto scroll down.
        //private void FrameworkElement_OnSizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    var dataGrid = sender as DataGrid;
        //    dataGrid.ScrollIntoView(dataGrid.Items[^1]);
        //}
    }
}