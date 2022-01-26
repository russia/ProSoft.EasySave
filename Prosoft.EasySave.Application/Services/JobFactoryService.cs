using Newtonsoft.Json;
using ProSoft.EasySave.Application.Models.Contexts;
using ProSoft.EasySave.Application.Enums;
using ProSoft.EasySave.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using ProSoft.EasySave.Application.Models;
using Microsoft.Extensions.Options;

namespace ProSoft.EasySave.Application.Services
{
    public class JobFactoryService : IJobFactoryService
    {
        private readonly List<JobContext> _jobContexts;
        private readonly ILogger<JobFactoryService> _logger;
        private readonly IFileService _fileService;
        private readonly IOptions<Configuration> _configuration;
        private ExecutionType _executionType;

        public JobFactoryService(ILogger<JobFactoryService> logger, IFileService fileService, IOptions<Configuration> configuration)
        {
            _jobContexts = new List<JobContext>();
            _logger = logger;
            _fileService = fileService;
            _configuration = configuration;
        }

        public void AddJob(string name, TransferType transferType, string sourcePath, string destinationPath)
        {
            _jobContexts.Add(new JobContext()
            {
                Name = name,
                TransferType = transferType,
                SourcePath = sourcePath,
                DestinationPath = destinationPath
            });

            _logger.LogDebug("New job context added in pool.");
        }

        public async Task<IReadOnlyCollection<JobResult>> StartJobsAsync(int count, ExecutionType executionType)
        {
            var cancellationToken = new CancellationTokenSource();
            var taskList = _jobContexts.Select(jobContext => new Task<JobResult>(() => _fileService.CopyFiles(jobContext, cancellationToken.Token).Result));
            return await taskList.StartAsync<List<JobResult>>(executionType, cancellationToken.Token);
        }

        public void LoadConfiguration()
        {
            _logger.LogInformation("Loading configuration..");
            if (_configuration.Value.JobContexts.Count > 5)
            {
                throw new NotImplementedException();
            }
            _executionType = _configuration.Value.ExecutionType;
            _jobContexts.AddRange(_configuration.Value.JobContexts);
        }
    }
}