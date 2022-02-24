using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using static ProSoft.EasySave.Infrastructure.Services.JobFactoryService;

namespace ProSoft.EasySave.Infrastructure.Interfaces.Services
{
    public interface IJobFactoryService
    {
        event JobListUpdated OnJobListUpdated;

        event JobStarted OnJobStarted;

        event JobCompleted OnJobCompleted;

        event JobPaused OnJobPaused;

        event JobResumed OnJobResumed;

        event JobCancelled OnJobCancelled;

        void LoadConfiguration();

        IReadOnlyCollection<JobContext> GetJobs();

        void PauseAllJobsAsync();

        void ResumeAllJobsAsync();

        void CancelAllJobsAsync();

        void PauseJob(JobContext jobContext);

        void ResumeJob(JobContext jobContext);

        void CancelJob(JobContext jobContext);

        void RemoveJob(JobContext jobContext);

        void AddJob(string name, TransferType transferType, string sourcePath, string destinationPath);

        IEnumerable<Process> GetProcessInstances(string[] processes);

        Task<IReadOnlyCollection<JobResult>> StartAllJobsAsync(ExecutionType? executionType = null);

        Task<IReadOnlyCollection<JobResult>> StartJobsAsync(List<JobContext> jobContexts, ExecutionType? executionType = null);

        Task<JobResult> StartJobAsync(JobContext jobContext, ExecutionType? executionType = null);
    }
}