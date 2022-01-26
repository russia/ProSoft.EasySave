using Microsoft.Extensions.Logging;
using ProSoft.EasySave.Application.Enums;
using ProSoft.EasySave.Application.Extensions;
using ProSoft.EasySave.Application.Interfaces.Services;
using ProSoft.EasySave.Application.Models.Contexts;
using System.Diagnostics;
using System.Security.Cryptography;

namespace ProSoft.EasySave.Application.Services;

public class FileService : IFileService
{
    private readonly ILogger<SampleService> _logger;

    public FileService(ILogger<SampleService> logger)
    {
        _logger = logger;
    }

    public async Task<JobResult> CopyFiles(JobContext jobContext, CancellationToken cancellationToken = default)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        jobContext.StateType = StateType.PROCESSING;
        _logger.LogInformation($"[{jobContext.Name}] is processing.");

        var sourceDir = new DirectoryInfo(jobContext.SourcePath);
        var destinationDir = new DirectoryInfo(jobContext.DestinationPath);

        var sourceFiles = sourceDir
            .GetFiles()
            .ToList();

        var destinationFiles = destinationDir
            .GetFiles()
            .ToList();

        if (jobContext.TransferType is TransferType.DIFFERENTIAL)
        {
            sourceFiles.RemoveAll(srcFile => destinationFiles.Any(destFile => destFile.GetHash() == srcFile.GetHash()));
        }

        foreach (var sourceFile in sourceFiles)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var destinationPath = Path.Combine(jobContext.DestinationPath, sourceFile.Name);
            sourceFile.CopyTo(destinationPath, true);
            _logger.LogInformation($"[{jobContext.Name}] File {sourceFile.Name} copied to {destinationPath}.");
        }

        jobContext.StateType = StateType.COMPLETED;
        _logger.LogInformation($"[{jobContext.Name}] is completed.");
        stopWatch.Stop();

        return new JobResult()
        {
            Name = jobContext.Name,
            FilesNumber = sourceFiles.Count,
            TotalFilesWeight = (ulong)sourceFiles.Sum(srcFile => srcFile.Length),
            Duration = stopWatch.Elapsed
        };
    }
}