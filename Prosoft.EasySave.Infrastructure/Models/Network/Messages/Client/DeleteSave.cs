using ProSoft.EasySave.Infrastructure.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Messages.Client
{
    public class DeleteSave : ISendable
    {
        public JobContext JobContext { get; set; }

        public DeleteSave(JobContext jobContext)
        {
            JobContext = jobContext;
        }
    }
}
