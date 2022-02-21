using ProSoft.EasySave.Infrastructure.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
