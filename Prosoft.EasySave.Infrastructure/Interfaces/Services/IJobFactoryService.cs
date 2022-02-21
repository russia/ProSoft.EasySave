using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using ProSoft.EasySave.Infrastructure.Models.Network.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ProSoft.EasySave.Infrastructure.Services.JobFactoryService;

namespace ProSoft.EasySave.Infrastructure.Interfaces.Services
{
    public interface IJobFactoryService
    {
        event JobListUpdated OnJobListUpdated;
        event JobStarted OnJobStarted;
        event JobCompleted OnJobCompleted;
        void LoadConfiguration();

        IReadOnlyCollection<JobContext> GetJobs();

        void RemoveJob(JobContext jobContext);

        void AddJob(string name, TransferType transferType, string sourcePath, string destinationPath);

        Task<IReadOnlyCollection<JobResult>> StartAllJobsAsync(ExecutionType? executionType = null);

        Task<JobResult> StartJobAsync(JobContext jobContext, ExecutionType? executionType = null);
    }
}