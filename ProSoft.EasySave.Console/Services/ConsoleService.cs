using ProSoft.EasySave.Application.Models.Contexts;
using ProSoft.EasySave.Console.Interfaces;

namespace ProSoft.EasySave.Console.Managers;

public class ConsoleService : IConsoleService
{
    public Task<JobResult> StartJobAsync(JobContext jobContext)
    {
        throw new NotImplementedException();
    }
}