using System;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using System.Windows;
using System.Windows.Forms;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using System.IO;

namespace ProSoft.EasySave.Presentation.ViewModels
{
    internal class _SaveConfigViewModel : BindableBase, IDialogAware
    {
        private readonly IJobFactoryService _jobFactoryService;
        private readonly IRegionManager _regionManager;
        public double screenHeight { get; set; }
        public string StackPanelColor { get; set; }
        private string _name;
        private string _destinationPath;
        private string _sourcePath;
        private string _tranferType;
        private TransferType transferType;

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }


        public event Action<IDialogResult> RequestClose;

        public DelegateCommand SourceDialogCommand { get; }
        public DelegateCommand DestDialogCommand { get; }
        public DelegateCommand ValidSaveCommand { get; }

        public _SaveConfigViewModel(IRegionManager regionManager, IJobFactoryService jobFactoryService)
        {
            _regionManager = regionManager;
            _jobFactoryService = jobFactoryService;
            screenHeight = SystemParameters.FullPrimaryScreenHeight;
            StackPanelColor = "Gray";

            SourceDialogCommand = new DelegateCommand(() =>
            {
                var openfolder = new FolderBrowserDialog();
                openfolder.ShowDialog();
                SourcePath = openfolder.SelectedPath;
            });

            DestDialogCommand = new DelegateCommand(() =>
            {
                var openfolder = new FolderBrowserDialog();
                openfolder.ShowDialog();
                DestinationPath = openfolder.SelectedPath;
            });

            ValidSaveCommand = new DelegateCommand(() =>
            {
                if (Enum.TryParse(StrTransferType, out transferType)) ;


                var jobContext = new JobContext
                {
                    Name = Name,
                    SourcePath = SourcePath,
                    DestinationPath = DestinationPath,
                    TransferType = transferType
                };

                System.Windows.MessageBox.Show("Job created !");


                _jobFactoryService.AddJob(jobContext.Name, jobContext.TransferType, jobContext.SourcePath, jobContext.DestinationPath);
                CloseDialog();
            },
            () =>
            !string.IsNullOrEmpty(Name) && SourcePath!=DestinationPath && Directory.Exists(SourcePath) && Directory.Exists(DestinationPath))
                .ObservesProperty(() => Name)
                .ObservesProperty(() => SourcePath)
                .ObservesProperty(() => DestinationPath);
        }

        public string DestinationPath
        {
            get => _destinationPath;
            set => SetProperty(ref _destinationPath, value);
        }

        public string SourcePath
        {
            get => _sourcePath;
            set => SetProperty(ref _sourcePath, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string StrTransferType
        {
            get => _tranferType;
            set => SetProperty(ref _tranferType, value);
        }

        public string _title = "SaveConfigView";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        protected virtual void CloseDialog()
        {
            ButtonResult result = ButtonResult.OK;

            RaiseRequestClose(new Prism.Services.Dialogs.DialogResult(result));
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("message");
        }

        public bool CanCloseDialog() => true;
    }
}