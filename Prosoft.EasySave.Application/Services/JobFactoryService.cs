using Microsoft.Extensions.Options;
using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.Infrastructure.Models;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProSoft.EasySave.Infrastructure.Services
{

    public class JobFactoryService : IJobFactoryService
    {
        private readonly IOptions<Configuration> _configuration;
        private readonly IFileService _fileService;
        private readonly List<JobContext> _jobContexts;
        private ExecutionType _executionType;

        public JobFactoryService(IFileService fileService, IOptions<Configuration> configuration)
        {
            _jobContexts = new List<JobContext>();
            _fileService = fileService;
            _configuration = configuration;
        }

        public void AddJob(string name, TransferType transferType, string sourcePath, string destinationPath)
        {
            if (_jobContexts.Count > 5)
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

            Console.WriteLine("New job context added in the job context list.");
        }

        public async Task<IReadOnlyCollection<JobResult>> StartJobsAsync(ExecutionType? executionType = null)
        {
            var cancellationToken = new CancellationTokenSource();
            var taskList = _jobContexts.Select(jobContext =>
                new Task<JobResult>(() => _fileService.CopyFiles(jobContext, cancellationToken.Token).Result));
            return await taskList.StartAsync<List<JobResult>>(executionType ?? _executionType, cancellationToken.Token);
        }

        public void LoadConfiguration()
        {
            Console.WriteLine("Loading configuration..");
            if (_configuration.Value.JobContexts.Count() > 5)
            {
                Console.Error.WriteLine("Job contexts limit reached!");
                throw new NotImplementedException();
            }

            _executionType = _configuration.Value.ExecutionType;
            _jobContexts.AddRange(_configuration.Value.JobContexts);
            Console.WriteLine($"Successfully loaded {_configuration.Value.JobContexts.Count()} job context(s):");
            Console.WriteLine(string.Join(Environment.NewLine,
                _configuration.Value.JobContexts.Select(jobContext => jobContext.Name)));
        }
    }
}