using Prism.Regions;
using System.Collections.Generic;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using Prism.Mvvm;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using Prism.Services.Dialogs;
using static System.Windows.Input.ICommand;
using Prism.Commands;
using System.Linq;
using System.Collections.ObjectModel;

namespace ProSoft.EasySave.Presentation.ViewModels
{
    internal class _ListSaveViewModel : BindableBase
    {
        private ObservableCollection<JobContext> _jobContexts;
        private readonly IRegionManager _regionManager;
        private readonly IJobFactoryService _jobFactoryService;
        private IDialogService _dialogService;
        public DelegateCommand OpenDialogCommand { get; }

        public DelegateCommand<object> StartSaves { get;  }
        public DelegateCommand<object> PauseSaves { get;  }
        public DelegateCommand<object> StopSaves { get;  }
        public DelegateCommand<object> ResumeSaves { get;  }
        public DelegateCommand<object> RemoveSaves { get;  }

        private string Title { get; set; }

        public double screenHeight { get; set; }
        public double GridHeight { get; set; }
        public double buttonWidth { get; set; }
        public double buttonHeight { get; set; }
        public double GridWidth { get; set; }

        public ObservableCollection<JobContext> JobContexts
        {
            get => _jobContexts;
            set => SetProperty(ref _jobContexts, value);
        }

        public _ListSaveViewModel(IRegionManager regionManager, IJobFactoryService jobFactoryService, IDialogService dialogService)
        {
            var message = "This is a message that should be shown in the dialog.";
            _regionManager = regionManager;
            _jobFactoryService = jobFactoryService;
            _jobFactoryService.OnJobListUpdated+=(sender, args) => JobContexts = args.JobContexts;
            _jobFactoryService.OnJobStarted+=(sender, args) =>
            {
                JobContexts.Remove(JobContexts.First(j => j.Id == args.JobContext.Id));
                JobContexts.Add(args.JobContext);
            };
            _jobFactoryService.OnJobCompleted += (sender, args) =>
            {
                JobContexts.Remove(JobContexts.First(j => j.Id == args.JobContext.Id));
                JobContexts.Add(args.JobContext);
            };

            JobContexts = new ObservableCollection<JobContext>(_jobFactoryService.GetJobs());
            _dialogService = dialogService;
            SetParameter();
            OpenDialogCommand = new DelegateCommand(() => _dialogService.ShowDialog("SaveConfigView", new DialogParameters($"message={message}"), null));
            StartSaves = new DelegateCommand<object>(e =>
            {
                System.Collections.IList items = (System.Collections.IList)e;
                List<JobContext> jobContexts = items.Cast<JobContext>().ToList();
                if(items.Count == JobContexts.Count || items.Count == 0)
                {
                    _jobFactoryService.StartAllJobsAsync();
                    return;
                }
                    _jobFactoryService.StartJobsAsync(jobContexts);
                    
            }, (a) => JobContexts.Any(j => !j.IsCompleted && j.StateType == Infrastructure.Enums.StateType.WAITING));

            PauseSaves = new DelegateCommand<object>(e =>
            {
                System.Collections.IList items = (System.Collections.IList)e;
                List<JobContext> jobContexts = items.Cast<JobContext>().ToList();
                if (items.Count == JobContexts.Count || items.Count == 0)
                {
                    _jobFactoryService.PauseAllJobsAsync();
                    return;
                }
                    foreach (JobContext _jobContext in jobContexts)
                        _jobFactoryService.PauseJob(_jobContext);
                    
            }, (a) => JobContexts.Any(j => j.StateType == Infrastructure.Enums.StateType.PROCESSING));

            ResumeSaves = new DelegateCommand<object>(e =>
            {
                System.Collections.IList items = (System.Collections.IList)e;
                List<JobContext> jobContexts = items.Cast<JobContext>().ToList();
                if (items.Count == JobContexts.Count || items.Count == 0)
                {
                    _jobFactoryService.ResumeAllJobsAsync();
                    return;
                }
                foreach (JobContext _jobContext in jobContexts)
                    _jobFactoryService.ResumeJob(_jobContext);

            }, (a) => JobContexts.Any(j => j.StateType == Infrastructure.Enums.StateType.PAUSED));

            RemoveSaves = new DelegateCommand<object>(e =>
            {
                System.Collections.IList items = (System.Collections.IList)e;
                List<JobContext> jobContexts = items.Cast<JobContext>().ToList();
                if(!jobContexts.Any())
                    foreach (JobContext _jobContext in JobContexts)
                        _jobFactoryService.RemoveJob(_jobContext);

                foreach (JobContext _jobContext in jobContexts)
                    _jobFactoryService.RemoveJob(_jobContext);

            }, (a) => JobContexts.Any());
        }

        protected void SetParameter()
        {
            screenHeight = 1020;
            GridHeight = screenHeight / 1.5;
            buttonWidth = 1980 / 10;
            buttonHeight = screenHeight / 12;
            GridWidth = 1920 - (1920 * 0.08);
        }
    }
}