using System;
using System.Collections.Generic;
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

        public void PauseAllJobsAsync()
        {
            _jobContexts.ForEach(j =>
            {
                j.PauseRaised = true;
                OnJobPaused?.Invoke(this, new JobPausedEventArgs(j));
            });
        }

        public void ResumeAllJobsAsync()
        {
            _jobContexts.ForEach(j =>
            {
                j.PauseRaised = false;
                OnJobResumed?.Invoke(this, new JobResumedEventArgs(j));
            });
        }

        public void CancelAllJobsAsync()
        {
            _jobContexts.Where(j => !j.IsCompleted)
                .ToList()
                .ForEach(j =>
                {
                    j.CancellationRaised = true;
                    OnJobCancelled?.Invoke(this, new JobCancelledEventArgs(j));
                });
        }

        public void PauseJob(JobContext jobContext)
        {
            // TODO : We can compare the object or create the comparison method.
            var jobTask = _jobContexts.FirstOrDefault(j => j.Name == jobContext.Name);

            if (jobTask is null)
                return;

            jobTask.PauseRaised = true;
            OnJobPaused?.Invoke(this, new JobPausedEventArgs(jobTask));
        }

        public void ResumeJob(JobContext jobContext)
        {
            // TODO : We can compare the object or create the comparison method.
            var jobTask = _jobContexts.FirstOrDefault(j => j.Name == jobContext.Name);

            if (jobTask is null)
                return;

            jobTask.PauseRaised = false;
            OnJobResumed?.Invoke(this, new JobResumedEventArgs(jobTask));
        }

        public void CancelJob(JobContext jobContext)
        {
            // TODO : We can compare the object or create the comparison method.
            var jobTask = _jobContexts.FirstOrDefault(j => j.Name == jobContext.Name);

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
        {
            List<Func<Task<JobResult>>> taskList = new();
            // TODO : use select instead.
            foreach (var jobContext in _jobContexts)
            {
                Func<Task<JobResult>> task = async () =>
                {
                    OnJobStarted?.Invoke(this, new JobStartedEventArgs(jobContext));
                    var result = await _fileService.CopyFiles(jobContext);
                    OnJobCompleted?.Invoke(this, new JobCompletedEventArgs(jobContext));
                    return result;
                };
                taskList.Add(task);
            }

            var jobResults = await taskList.StartAsync<IReadOnlyCollection<JobResult>>(executionType ?? _executionType);
            _jobContexts.Clear();
            OnJobListUpdated?.Invoke(this, new JobListUpdatedEventArgs(_jobContexts));
            return jobResults;
        }

        public async Task<JobResult> StartJobAsync(JobContext jobCxt, ExecutionType? executionType = null)
        {
            // TODO : We can compare the object or create the comparison method.
            var jobContext = _jobContexts.FirstOrDefault(j => j.Name == jobCxt.Name);
            if (jobContext is null)
                // TODO : handle this situation..
                return null;

            Func<Task<JobResult>> task = async () =>
            {
                OnJobStarted?.Invoke(this, new JobStartedEventArgs(jobContext));
                var result = await _fileService.CopyFiles(jobContext);
                OnJobCompleted?.Invoke(this, new JobCompletedEventArgs(jobContext));
                return result;
            };

            var jobResults = await task.StartAsync<JobResult>(executionType ?? _executionType);
            return jobResults;
        }

        public void LoadConfiguration()
        {
            Console.WriteLine("Loading configuration..");
            if (_configuration.Value.JobContexts.Count > 9999)
            {
                Console.Error.WriteLine("Job contexts limit reached!");
                throw new NotImplementedException();
            }

            _executionType = _configuration.Value.ExecutionType;
            _jobContexts.AddRange(_configuration.Value.JobContexts);
            Console.WriteLine($"Successfully loaded {_configuration.Value.JobContexts.Count} job context(s):");
            Console.WriteLine(string.Join(Environment.NewLine,
                _configuration.Value.JobContexts.Select(jobContext => jobContext.Name)));
        }

        public void RemoveJob(JobContext jobContext)
        {
            // TODO : We can compare the object or create the comparison method.
            var item = _jobContexts.FirstOrDefault(j => j.Name == jobContext.Name);

            if (item is null)
                return;

            _jobContexts.Remove(item);
            OnJobListUpdated?.Invoke(this, new JobListUpdatedEventArgs(_jobContexts));
        }
    }
}