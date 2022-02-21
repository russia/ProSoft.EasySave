using System;
using ProSoft.EasySave.Infrastructure.Models.Contexts;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Events
{
    public class JobCompletedEventArgs : EventArgs
    {
        public JobCompletedEventArgs(JobContext jobContext)
        {
            JobContext = jobContext;
        }

        public JobContext JobContext { get; set; }
    }
}