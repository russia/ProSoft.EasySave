using Prism.Commands;
using Prism.Regions;
using System;
using System.IO;
using System.Diagnostics;
using Prism.Mvvm;
using ProSoft.EasySave.Infrastructure.Helpers;

namespace ProSoft.EasySave.Presentation.ViewModels
{

    internal class _AppConfigViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private string _stopAppFilename;
        private string _maxFileWeight;
        private string _filePriority;
        private string _filePriority2;
        private string _filePriority3;
        private string _filePriorityTotal;

        public string StopAppFilename
        {
            get { return _stopAppFilename; }
            set { SetProperty(ref _stopAppFilename, value); }
        }
        public string MaxFileWeight
        {
            get { return _maxFileWeight; }
            set { SetProperty(ref _maxFileWeight, value); }
        }

        public string FilePriority
        {
            get { return _filePriority; }
            set { SetProperty(ref _filePriority, value); }
        }

        public string FilePriority2
        {
            get { return _filePriority2; }
            set { SetProperty(ref _filePriority2, value); }
        }

        public string FilePriority3
        {
            get { return _filePriority3; }
            set { SetProperty(ref _filePriority3, value); }
        }

        public string FilePriorityTotal
        {
            get { return _filePriorityTotal; }
            set { SetProperty(ref _filePriorityTotal, value); }
        }

        public double screenHeight { get; set; }
        public double GridHeight { get; set; }
        public double buttonWidth { get; set; }
        public double buttonHeight { get; set; }
        public double GridWidth { get; set; }
        public DelegateCommand StopAppFileDialogCommand { get; }
        public DelegateCommand OpenExplorerToLogsCommand { get; }
        public DelegateCommand SaveSettingsCommand { get; }


        public _AppConfigViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            SetParameter();
            StopAppFileDialogCommand = new DelegateCommand(() =>
            {
                StopAppFileSelection();
            });
            OpenExplorerToLogsCommand = new DelegateCommand(OpenExplorerToLogs);
            SaveSettingsCommand = new DelegateCommand(SaveSettings);

            ReadSettings();
        }

        protected void SetParameter()
        {
            screenHeight = 1020;
            GridHeight = screenHeight / 1.5;
            buttonWidth = 1980 / 10;
            buttonHeight = screenHeight / 12;
            GridWidth = 1920 - (1920 * 0.1);
        }

        private void StopAppFileSelection()
        {
            Microsoft.Win32.OpenFileDialog pathDialog = new Microsoft.Win32.OpenFileDialog();
            pathDialog.DefaultExt = ".exe";
            pathDialog.Filter = "Application (*.exe)|*.exe";
            pathDialog.ShowDialog();
            StopAppFilename = pathDialog.SafeFileName.ToLower();
        }

        public void SaveSettings()
        {
            // Update business blocking app.
            ConfigHelpers.UpdateSetting("BusinessApp", StopAppFilename);

            // Update max file weight accepted.
            ConfigHelpers.UpdateSetting("MaxWeight", MaxFileWeight);


            //Update priority files
            FilePriorityTotal = FilePriority + "|" + FilePriority2 + "|" + FilePriority3;
            ConfigHelpers.UpdateSetting("PrioritaryExt", FilePriorityTotal);
        }

        public void ReadSettings()
        {
            StopAppFilename = ConfigHelpers.ReadSetting("BusinessApp").ToString();

            MaxFileWeight = ConfigHelpers.ReadSetting("MaxWeight").ToString();

            var extensions = ConfigHelpers.ReadSetting("PrioritaryExt").Split('|');
            FilePriority = extensions[0];
            FilePriority2 = extensions[1];
            FilePriority3 = extensions[2];
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