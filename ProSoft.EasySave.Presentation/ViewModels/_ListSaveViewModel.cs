using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.Infrastructure.Models.Contexts;

namespace ProSoft.EasySave.Presentation.ViewModels
{
    internal class _ListSaveViewModel : BindableBase
    {
        private readonly IJobFactoryService _jobFactoryService;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private ObservableCollection<JobContext> _jobContexts;

        public _ListSaveViewModel(IRegionManager regionManager, IJobFactoryService jobFactoryService,
            IDialogService dialogService)
        {
            _regionManager = regionManager;
            _jobFactoryService = jobFactoryService;
            _dialogService = dialogService;
            JobContexts = new ObservableCollection<JobContext>(_jobFactoryService.GetJobs());
            foreach (var jobContext in JobContexts)
            {
                jobContext.OnJobContextPropertyUpdated += (sender, args) =>
                {
                    JobContexts.Remove(JobContexts.First(j => j.Id == args.JobContext.Id));
                    JobContexts.Add(args.JobContext);
                };
            }

            SetParameter();

            var message = "This is a message that should be shown in the dialog.";

            _jobFactoryService.OnJobListUpdated += (sender, args) =>
            {
                JobContexts = args.JobContexts;
            };

            _jobFactoryService.OnJobStarted += (sender, args) =>
            {
                JobContexts.Remove(JobContexts.First(j => j.Id == args.JobContext.Id));
                args.JobContext.OnJobContextPropertyUpdated += (sender, args) =>
                {
                    JobContexts.Remove(JobContexts.First(j => j.Id == args.JobContext.Id));
                    JobContexts.Add(args.JobContext);
                };
                JobContexts.Add(args.JobContext);
            };

            OpenDialogCommand = new DelegateCommand(() =>
                _dialogService.ShowDialog("SaveConfigView", new DialogParameters($"message={message}"), null));

            StartSaves = new DelegateCommand<object>(e =>
            {
                var items = (IList)e;
                var jobContexts = items.Cast<JobContext>().ToList();

                if (items.Count == JobContexts.Count || items.Count == 0)
                {
                    _jobFactoryService.StartAllJobsAsync();
                    _jobFactoryService.ResumeAllJobs();
                    return;
                }

                _jobFactoryService.StartJobsAsync(jobContexts);
            });

            PauseSaves = new DelegateCommand<object>(e =>
            {
                var items = (IList)e;
                var jobContexts = items.Cast<JobContext>().ToList();
                if (items.Count == JobContexts.Count || items.Count == 0)
                {
                    _jobFactoryService.PauseAllJobs();
                    return;
                }

                foreach (var _jobContext in jobContexts)
                    _jobFactoryService.PauseJob(_jobContext);
            });

            RemoveSaves = new DelegateCommand<object>(e =>
            {
                var items = (IList)e;
                var jobContexts = items.Cast<JobContext>().ToList();
                if (!jobContexts.Any())
                    _jobFactoryService.RemoveAllJobs();

                foreach (var _jobContext in jobContexts)
                    _jobFactoryService.RemoveJob(_jobContext);
            });

            StopSaves = new DelegateCommand<object>(e =>
            {
                var items = (IList)e;
                var jobContexts = items.Cast<JobContext>().ToList();
                if (!jobContexts.Any())
                    _jobFactoryService.CancelAllJobs();

                foreach (var _jobContext in jobContexts)
                    _jobFactoryService.CancelJob(_jobContext);
            });
        }

        public DelegateCommand OpenDialogCommand { get; }

        public DelegateCommand<object> StartSaves { get; }
        public DelegateCommand<object> PauseSaves { get; }
        public DelegateCommand<object> StopSaves { get; }
        public DelegateCommand<object> RemoveSaves { get; }

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

        protected void SetParameter()
        {
            screenHeight = 1020;
            GridHeight = screenHeight / 1.5;
            buttonWidth = 1980 / 10;
            buttonHeight = screenHeight / 12;
            GridWidth = 1920 - 1920 * 0.08;
        }
    }
}