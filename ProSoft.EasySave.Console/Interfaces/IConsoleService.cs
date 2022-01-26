using ProSoft.EasySave.Application.Enums;
using ProSoft.EasySave.Application.Models.Contexts;

namespace ProSoft.EasySave.Console.Interfaces;

internal interface IConsoleService
{
    void AddJob(string name, TransferType transferType, string sourcePath, string destinationPath);

    Task<IReadOnlyCollection<JobResult>> Start(ExecutionType executionType);
}