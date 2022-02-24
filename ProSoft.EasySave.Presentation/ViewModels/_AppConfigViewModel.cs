using Prism.Commands;
using Prism.Regions;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Diagnostics;
using Prism.Mvvm;

namespace ProSoft.EasySave.Presentation.ViewModels
{

    internal class _AppConfigViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private string _stopAppPath;
        public string StopAppPath
        {
            get { return _stopAppPath; }
            set  { SetProperty(ref _stopAppPath, value); }
        }
        public double screenHeight { get; set; }
        public double GridHeight { get; set; }
        public double buttonWidth { get; set; }
        public double buttonHeight { get; set; }
        public double GridWidth { get; set; }
        public DelegateCommand StopAppPathDialogCommand { get; }
        public DelegateCommand OpenExplorerToLogsCommand { get; }


        public _AppConfigViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            SetParameter();
            StopAppPathDialogCommand = new DelegateCommand(() =>
            {
                StopAppPathSelection();
            });

            OpenExplorerToLogsCommand = new DelegateCommand(OpenExplorerToLogs);


        }

        protected void SetParameter()
        {
            screenHeight = 1020;
            GridHeight = screenHeight / 1.5;
            buttonWidth = 1980 / 10;
            buttonHeight = screenHeight / 12;
            GridWidth = 1920 - (1920 * 0.1);
        }

        private void StopAppPathSelection()
        {
            Microsoft.Win32.OpenFileDialog pathDialog = new Microsoft.Win32.OpenFileDialog();
            pathDialog.DefaultExt = ".exe";
            pathDialog.Filter = "Application (*.exe)|*.exe";
            pathDialog.ShowDialog();
            StopAppPath = pathDialog.SafeFileName;
        }

        public static void OpenExplorerToLogs()
        {
            var currentPath = AppDomain.CurrentDomain.BaseDirectory;
            Process explorer = new Process();
            explorer.StartInfo.UseShellExecute = true;



            //TODO : Open explorer to exact logs location
            var enviroment = System.Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(enviroment).Parent.FullName;
            //explorer.StartInfo.FileName = @"./logs/";
            //explorer.StartInfo.FileName = Directory.GetDirectoryRoot(currentPath);
            //explorer.StartInfo.FileName = AppContext.BaseDirectory;
            explorer.StartInfo.FileName = projectDirectory;
            explorer.Start();
        }

    }
}
