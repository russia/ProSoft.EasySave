using System.Threading.Tasks;
using ProSoft.EasySave.Infrastructure.Models.Contexts;

namespace ProSoft.EasySave.Infrastructure.Interfaces.Services
{
    public interface IFileService
    {
        Task<JobResult> CopyFiles(JobContext jobContext);
    }
}