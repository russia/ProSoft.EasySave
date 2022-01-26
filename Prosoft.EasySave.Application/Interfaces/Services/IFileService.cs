using ProSoft.EasySave.Application.Models.Contexts;

namespace ProSoft.EasySave.Application.Interfaces.Services;

public interface IFileService
{
    Task<JobResult> CopyFiles(JobContext jobContext, CancellationToken cancellationToken = default);
}