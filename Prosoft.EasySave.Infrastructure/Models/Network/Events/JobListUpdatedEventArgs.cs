using System;
using System.Collections.Generic;
using ProSoft.EasySave.Infrastructure.Models.Contexts;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Events
{
    public class JobListUpdatedEventArgs : EventArgs
    {
        public JobListUpdatedEventArgs(List<JobContext> jobContexts)
        {
            JobContexts = jobContexts;
        }

        public List<JobContext> JobContexts { get; set; }
    }
}