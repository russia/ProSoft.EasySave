using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ProSoft.EasySave.Application.Models.Logging;
using Microsoft.Extensions.DependencyInjection;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using Prism.Mvvm;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;

namespace ProSoft.EasySave.Presentation.ViewModels
{

    internal class _ListSaveViewModel : BindableBase
    {
        private List<JobContext> _jobContexts;
        private readonly IRegionManager _regionManager;
        private readonly IJobFactoryService _jobFactoryService;
        public double screenHeight { get; set; }
        public double GridHeight { get; set; }
        public double buttonWidth { get; set; }
        public double buttonHeight { get; set; }
        public double GridWidth { get; set; }

        public List<JobContext> JobContexts
        {
            get => _jobContexts;
            set => SetProperty(ref _jobContexts, value);
        }

        public _ListSaveViewModel(IRegionManager regionManager, IJobFactoryService jobFactoryService)
        {
            _regionManager = regionManager;
            _jobFactoryService = jobFactoryService;
            _jobFactoryService.LoadConfiguration();
            JobContexts = _jobFactoryService.GetJobs().ToList();
            SetParameter();
        }

        protected void SetParameter()
        {
            screenHeight = SystemParameters.FullPrimaryScreenHeight;
            GridHeight = screenHeight / 1.5;
            buttonWidth = SystemParameters.FullPrimaryScreenWidth / 10;
            buttonHeight = screenHeight / 12;
            GridWidth = SystemParameters.FullPrimaryScreenWidth - (SystemParameters.FullPrimaryScreenWidth * 0.1);
        }
    }
}