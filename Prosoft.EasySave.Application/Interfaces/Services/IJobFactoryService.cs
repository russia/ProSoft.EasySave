using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProSoft.EasySave.Infrastructure.Interfaces.Services
{

    public interface IJobFactoryService
    {
        void LoadConfiguration();

        void AddJob(string name, TransferType transferType, string sourcePath, string destinationPath);

        Task<IReadOnlyCollection<JobResult>> StartJobsAsync(ExecutionType? executionType = null);
    }
}