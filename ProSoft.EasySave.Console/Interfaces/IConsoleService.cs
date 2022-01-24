using ProSoft.EasySave.Application.Models.Contexts;

namespace ProSoft.EasySave.Console.Interfaces;

internal interface IConsoleService
{
    public Task<JobResult> StartJobAsync(JobContext jobContext);
}