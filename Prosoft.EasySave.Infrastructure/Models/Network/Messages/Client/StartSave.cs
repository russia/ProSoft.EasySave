using ProSoft.EasySave.Infrastructure.Models.Contexts;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Messages.Client
{
    public class StartSave : ISendable
    {
        public StartSave(JobContext jobContext)
        {
            JobContext = jobContext;
        }

        public JobContext JobContext { get; set; }
    }
}