using ProSoft.EasySave.Application.Enums;
using ProSoft.EasySave.Application.Models.Contexts;

namespace ProSoft.EasySave.Application.Interfaces.Services;

public interface IJobFactoryService
{
    void LoadConfiguration();

    void AddJob(string name, TransferType transferType, string sourcePath, string destinationPath);

    Task<IReadOnlyCollection<JobResult>> StartJobsAsync(ExecutionType? executionType = null);
}