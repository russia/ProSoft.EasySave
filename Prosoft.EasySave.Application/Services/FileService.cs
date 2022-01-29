using System.Diagnostics;
using Newtonsoft.Json;
using ProSoft.EasySave.Application.Enums;
using ProSoft.EasySave.Application.Extensions;
using ProSoft.EasySave.Application.Interfaces.Services;
using ProSoft.EasySave.Application.Models.Contexts;
using ProSoft.EasySave.Application.Models.Logging;
using Serilog;

namespace ProSoft.EasySave.Application.Services;

public class FileService : IFileService
{
    private readonly ILogger _logger;

    public FileService(ILogger logger)
    {
        _logger = logger.ForContext<FileService>();
    }

    public async Task<JobResult> CopyFiles(JobContext jobContext, CancellationToken cancellationToken = default)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        jobContext.StateType = StateType.PROCESSING;
        Console.WriteLine($"[{jobContext.Name}] is processing.");

        var sourceDir = new DirectoryInfo(jobContext.SourcePath);
        var destinationDir = new DirectoryInfo(jobContext.DestinationPath);

        var sourceFiles = sourceDir
            .GetFiles()
            .ToList();

        var destinationFiles = destinationDir
            .GetFiles()
            .ToList();

        if (jobContext.TransferType is TransferType.DIFFERENTIAL)
            sourceFiles.RemoveAll(srcFile => 
                destinationFiles.Any(destFile => srcFile.Length == destFile.Length && Task.Run(() =>
                                                           destFile.Compare(srcFile), cancellationToken).Result));

        foreach (var sourceFile in sourceFiles)
        {
            cancellationToken.ThrowIfCancellationRequested(); // TODO : handle cancellation tokens.
            var destinationPath = Path.Combine(jobContext.DestinationPath, sourceFile.Name);
            var stopwatchFile = new Stopwatch();
            stopwatchFile.Start();
            sourceFile.CopyTo(destinationPath, true);
            stopwatchFile.Stop();
            Console.WriteLine($"[{jobContext.Name}] File {sourceFile.Name} copied to {destinationPath}.");
            var jsonLog = new LogEntry
            {
                Name = jobContext.Name,
                FileSource = sourceFile.FullName,
                FileTarget = destinationPath,
                FileSize = sourceFile.Length,
                FileTransferTime = stopwatchFile.ElapsedMilliseconds,
                Time = DateTime.UtcNow
            };

            _logger.Information(JsonConvert.SerializeObject(jsonLog, Formatting.Indented));
        }

        jobContext.StateType = StateType.COMPLETED;
        Console.WriteLine($"[{jobContext.Name}] is completed.");
        stopWatch.Stop();

        return new JobResult
        {
            Name = jobContext.Name,
            FilesNumber = sourceFiles.Count,
            TotalFilesWeight = (ulong)sourceFiles.Sum(srcFile => srcFile.Length),
            Duration = stopWatch.Elapsed
        };
    }
}