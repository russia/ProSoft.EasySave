using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProSoft.EasySave.Infrastructure.Models.Contexts;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Events
{
    public class JobListUpdatedEventArgs : EventArgs
    {
        public JobListUpdatedEventArgs(List<JobContext> jobContexts)
        {
            JobContexts = new ObservableCollection<JobContext>(jobContexts);
        }

        public ObservableCollection<JobContext> JobContexts { get; set; }
    }
}