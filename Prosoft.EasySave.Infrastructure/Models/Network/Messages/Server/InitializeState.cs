using System.Collections.Generic;
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