using ProSoft.EasySave.Infrastructure.Models.Contexts;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Messages.Server
{
    public class JobCompleted : ISendable
    {
        public JobCompleted(JobContext jobContext)
        {
            JobContext = jobContext;
        }

        public JobContext JobContext { get; set; }
    }
}