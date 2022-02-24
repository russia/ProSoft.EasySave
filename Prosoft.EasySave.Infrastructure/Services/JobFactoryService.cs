using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.Infrastructure.Models;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using ProSoft.EasySave.Infrastructure.Models.Network.Events;

namespace ProSoft.EasySave.Infrastructure.Services
{
    public class JobFactoryService : IJobFactoryService
    {
        public delegate void JobCancelled(object sender, JobCancelledEventArgs e);

        public delegate void JobCompleted(object sender, JobCompletedEventArgs e);

        public delegate void JobListUpdated(object sender, JobListUpdatedEventArgs e);

        public delegate void JobPaused(object sender, JobPausedEventArgs e);

        public delegate void JobResumed(object sender, JobResumedEventArgs e);

        public delegate void JobStarted(object sender, JobStartedEventArgs e);

        private readonly IOptions<Configuration> _configuration;
        private readonly IFileService _fileService;
        private readonly List<JobContext> _jobContexts;
        private ExecutionType _executionType;
        private readonly string[] _processes = new string[] { "notepad", "calc" };

        public JobFactoryService(IFileService fileService, IOptions<Configuration> configuration)
        {
            _jobContexts = new List<JobContext>();
            _fileService = fileService;
            _configuration = configuration;
            LoadConfiguration();
        }

        public event JobListUpdated OnJobListUpdated;

        public event JobStarted OnJobStarted;

        public event JobCompleted OnJobCompleted;

        public event JobPaused OnJobPaused;

        public event JobResumed OnJobResumed;

        public event JobCancelled OnJobCancelled;

        public IReadOnlyCollection<JobContext> GetJobs()
        {
            return _jobContexts;
        }

        public void PauseAllJobs()
        {
            _jobContexts.Where(j => j.StateType == StateType.PROCESSING)
                .ToList()
                .ForEach(j =>
                {
                    j.PauseRaised = true;
                    j.StateType = StateType.PAUSED;
                    OnJobPaused?.Invoke(this, new JobPausedEventArgs(j));
                });
        }

        public void ResumeAllJobs()
        {
            _jobContexts.Where(j => j.StateType == StateType.PAUSED)
                .ToList()
                .ForEach(j =>
                {
                    j.PauseRaised = false;
                    j.StateType = StateType.PROCESSING;
                    OnJobResumed?.Invoke(this, new JobResumedEventArgs(j));
                });
        }

        public void RemoveAllJobs()
        {
            _jobContexts.Where(j => j.StateType != StateType.PROCESSING)
                .ToList()
                .ForEach(RemoveJob);
        }

        public void CancelAllJobs()
        {
            _jobContexts.Where(j => !j.IsCompleted)
                .ToList()
                .ForEach(j =>
                {
                    j.CancellationRaised = true;
                    j.StateType = StateType.CANCELLED;
                    OnJobCancelled?.Invoke(this, new JobCancelledEventArgs(j));
                });
        }

        public void PauseJob(JobContext jobContext)
        {
            var jobTask = _jobContexts.SingleOrDefault(j => j.Equals(jobContext));

            if (jobTask is null)
                return;

            jobTask.PauseRaised = true;
            OnJobPaused?.Invoke(this, new JobPausedEventArgs(jobTask));
        }

        public void ResumeJob(JobContext jobContext)
        {
            var jobTask = _jobContexts.SingleOrDefault(j => j.Equals(jobContext));

            if (jobTask is null)
                return;

            jobTask.PauseRaised = false;
            OnJobResumed?.Invoke(this, new JobResumedEventArgs(jobTask));
        }

        public void CancelJob(JobContext jobContext)
        {
            var jobTask = _jobContexts.SingleOrDefault(j => j.Equals(jobContext));

            if (jobTask is null)
                return;

            jobTask.CancellationRaised = true;
            OnJobCancelled?.Invoke(this, new JobCancelledEventArgs(jobTask));
        }

        

        public void AddJob(string name, TransferType transferType, string sourcePath, string destinationPath)
        {
            if (_jobContexts.Count > 9999)
            {
                Console.Error.WriteLine("The job contexts limit is reached.");
                return;
            }

            _jobContexts.Add(new JobContext
            {
                Name = name,
                TransferType = transferType,
                SourcePath = sourcePath,
                DestinationPath = destinationPath
            });

            OnJobListUpdated?.Invoke(this, new JobListUpdatedEventArgs(_jobContexts));

            Console.WriteLine("New job context added in the job context list.");
        }

        public async Task<IReadOnlyCollection<JobResult>> StartAllJobsAsync(ExecutionType? executionType = null)
            => await StartJobsAsync(_jobContexts.Where(j => j.StateType == StateType.WAITING).ToList(), executionType ?? _executionType);

        public async Task<IReadOnlyCollection<JobResult>> StartJobsAsync(List<JobContext> jobContexts, ExecutionType? executionType = null)
        {
            var processes = GetProcessInstances(_processes);
            if (processes.Any())
                return new List<JobResult>() { new JobResult(false, $"The following processes are running : {String.Join(", ", processes)}") };

            List<Func<Task<JobResult>>> taskList = new();

            // we would have use Select in C#10.
            foreach (var jobContext in jobContexts)
            {
                Func<Task<JobResult>> task = async () =>
                {
                    while (GetProcessInstances(_processes).Any())
                        await Task.Delay(50);
                    OnJobStarted?.Invoke(this, new JobStartedEventArgs(jobContext));
                    var result = await _fileService.CopyFiles(jobContext);
                    if (!result.Success)
                        return result; // TODO : create a event?
                    OnJobCompleted?.Invoke(this, new JobCompletedEventArgs(jobContext));
                    return result;
                };
                taskList.Add(task);
            }

            return await taskList.StartAsync<IReadOnlyCollection<JobResult>>(executionType ?? _executionType);
        }

        public async Task<JobResult> StartJobAsync(JobContext jobCxt, ExecutionType? executionType = null)
        {
            var processes = GetProcessInstances(_processes);
            if (processes.Any())
                return new JobResult(false, $"The following processes are running : {String.Join(", ", processes)}");

            // TODO : We can compare the object or create the comparison method.
            var jobContext = _jobContexts.FirstOrDefault(j => j.Name == jobCxt.Name);

            if (jobContext is null)
                // TODO : handle this situation..
                return null;

            Func<Task<JobResult>> task = async () =>
            {
                OnJobStarted?.Invoke(this, new JobStartedEventArgs(jobContext));
                var result = await _fileService.CopyFiles(jobContext);
                if (!result.Success)
                    return result; // TODO : create a event?
                OnJobCompleted?.Invoke(this, new JobCompletedEventArgs(jobContext));
                return result;
            };

            return await task.StartAsync<JobResult>(executionType ?? _executionType);
        }

        public void LoadConfiguration()
        {
            Console.WriteLine("Loading configuration..");
            if (_configuration.Value.JobContexts?.Count > 9999)
            {
                Console.Error.WriteLine("Job contexts limit reached!");
                throw new NotImplementedException();
            }

            _executionType = _configuration.Value.ExecutionType;

            if (_configuration.Value.JobContexts is null)
                return;

            _jobContexts.AddRange(_configuration.Value.JobContexts);
            Console.WriteLine($"Successfully loaded {_configuration.Value.JobContexts.Count} job context(s):");
            Console.WriteLine(string.Join(Environment.NewLine,
                _configuration.Value.JobContexts.Select(jobContext => jobContext.Name)));
        }

        public void RemoveJob(JobContext jobContext)
        {
            var item = _jobContexts.SingleOrDefault(j => j.Equals(jobContext));

            if (item is null)
                return;

            _jobContexts.Remove(item);
            OnJobListUpdated?.Invoke(this, new JobListUpdatedEventArgs(_jobContexts));
        }

        public IEnumerable<Process> GetProcessInstances(string[] processes)
        {
            var results = processes.Select(p => new { Name = p, Process = Process.GetProcessesByName(p) });
            return results.Where(r => r.Process.Any()).Select(p => p.Process.First());
        }
    }
}