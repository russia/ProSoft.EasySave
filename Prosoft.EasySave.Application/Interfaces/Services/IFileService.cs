using ProSoft.EasySave.Application.Models.Contexts;

namespace ProSoft.EasySave.Application.Interfaces.Services;

public interface IFileService
{
    /// <summary>
    /// Method allowing to copy files.
    /// </summary>
    /// <param name="jobContext">The job context.</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task containing the job result.</returns>
    Task<JobResult> CopyFiles(JobContext jobContext, CancellationToken cancellationToken = default);
}