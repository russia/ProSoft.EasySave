using ProSoft.EasySave.Infrastructure.Models.Contexts;
using System.Threading;
using System.Threading.Tasks;

namespace ProSoft.EasySave.Infrastructure.Interfaces.Services
{

    public interface IFileService
    {
        Task<JobResult> CopyFiles(JobContext jobContext, CancellationToken cancellationToken = default);
    }
}