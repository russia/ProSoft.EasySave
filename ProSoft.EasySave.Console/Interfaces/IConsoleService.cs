using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProSoft.EasySave.Console.Interfaces
{
    internal interface IConsoleService
{
    Task DisplayConsoleInterface();

    void AddJob(string name, TransferType transferType, string sourcePath, string destinationPath);

    Task<IReadOnlyCollection<JobResult>> Start(ExecutionType? executionType = null);
    }
}