using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProSoft.EasySave.Infrastructure.Models.Contexts;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Messages.Server
{
    public record InitializeState : ISendable
    {
        public InitializeState(IReadOnlyCollection<JobContext> jobContexts)
        {
            JobContexts = new ObservableCollection<JobContext>(jobContexts);
        }

        public ObservableCollection<JobContext> JobContexts { get; set; }
    }
}