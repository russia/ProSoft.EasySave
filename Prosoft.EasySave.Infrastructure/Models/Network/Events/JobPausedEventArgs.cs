﻿using System;
using ProSoft.EasySave.Infrastructure.Models.Contexts;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Events
{
    public class JobPausedEventArgs : EventArgs
    {
        public JobPausedEventArgs(JobContext jobContext)
        {
            JobContext = jobContext;
        }

        public JobContext JobContext { get; set; }
    }
}