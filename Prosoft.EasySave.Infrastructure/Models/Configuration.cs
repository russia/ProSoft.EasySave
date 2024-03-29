﻿using System.Collections.Generic;
using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Models.Contexts;

namespace ProSoft.EasySave.Infrastructure.Models
{
    public class Configuration
    {
        public List<JobContext> JobContexts { get; set; }
        public ExecutionType ExecutionType { get; set; }
        public string XorKey { get; set; }
    }
}