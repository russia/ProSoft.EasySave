using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProSoft.EasySave.Infrastructure.Models.Contexts;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Messages.Server
{
    public record InitializeState : ISendable
    {
        public InitializeState(IReadOnlyCollection<JobContext> jobContexts)
        {
            JobContexts = jobContexts;
        }

        public IReadOnlyCollection<JobContext> JobContexts { get; set; }
    }
}