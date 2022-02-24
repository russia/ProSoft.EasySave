using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ProSoft.EasySave.Application.Models.Logging;
using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Extensions;
using ProSoft.EasySave.Infrastructure.Helpers;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.Infrastructure.Models;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using Serilog;

namespace ProSoft.EasySave.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IOptions<Configuration> _configuration;
        private readonly ILogger _logger;

        public FileService(ILogger logger, IOptions<Configuration> configuration)
        {
            _configuration = configuration;
            _logger = logger.ForContext<FileService>();
        }

        public async Task<JobResult> CopyFiles(JobContext jobContext)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            jobContext.StateType = StateType.PROCESSING;
            Console.WriteLine($"[{jobContext.Name}] is processing.");

            if (!Directory.Exists(jobContext.SourcePath) || !Directory.Exists(jobContext.DestinationPath))
            {
                Console.WriteLine($"[{jobContext.Name}] has been cancelled because a directory doesn't exist.");
                return new JobResult(false, "One of the directory doesn't exist.");
            }

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
                        destFile.Compare(srcFile)).Result));

            var totalWeight = sourceFiles.Sum(sourceFile => sourceFile.Length);

            if (totalWeight > 9999999999)
            {
                Console.WriteLine($"[{jobContext.Name}] Skipping save job, as total files weight is to large ({totalWeight} bytes).");
                return new JobResult(false, "Files total weight is too large.");
            }

            sourceFiles = sourceFiles.OrderBy(s => s.Extension is ".exe" or ".pdf").ToList();

            foreach (var sourceFile in sourceFiles)
            {
                await Task.Delay(2000); // Simulating a heavy file transfer.

                if (jobContext.PauseRaised)
                    while (jobContext.PauseRaised)
                        await Task.Delay(50);

                if (jobContext.CancellationRaised)
                {
                    Console.WriteLine($"{jobContext.Name} cancelled as per as the user request.");
                    return new JobResult(false, "Cancellation token raised.");
                }

                if (sourceFile.Length > 9555599995)
                {
                    Console.WriteLine($"Skipping file {sourceFile.Name} as it's to large.");
                    continue;
                }

                var destinationPath = Path.Combine(jobContext.DestinationPath, sourceFile.ComputeEncryptedName());
                var stopwatchFile = new Stopwatch();
                stopwatchFile.Start();

                var encryptedFileSourcePath =
                    Path.Combine(sourceFile.Directory.FullName, sourceFile.ComputeEncryptedName());
                var encryptionTime = ProcessHelpers.UseProcess(
                    @"C:\Users\user\source\repos\ProSoft.EasySave\ProSoft.CryptoSoft\bin\Debug\net5.0\ProSoft.CryptoSoft.exe",
                    $"{_configuration.Value.XorKey} {StringsHelpers.Base64Encode(sourceFile.FullName)} {StringsHelpers.Base64Encode(encryptedFileSourcePath)}");
                var encryptedFile = new FileInfo(encryptedFileSourcePath);
                encryptedFile.CopyTo(destinationPath, true);
                encryptedFile.Delete();

                stopwatchFile.Stop();
                Console.WriteLine($"[{jobContext.Name}] File {sourceFile.Name} copied to {destinationPath}.");

                var logEntry = new LogEntry
                {
                    Name = jobContext.Name,
                    FileSource = sourceFile.FullName,
                    FileTarget = destinationPath,
                    FileSize = sourceFile.Length,
                    FileTransferTime = stopwatchFile.ElapsedMilliseconds,
                    EncryptionTime = (float)encryptionTime.TotalMilliseconds,
                    Time = DateTime.UtcNow
                };

                _logger.Information(logEntry.AsJson());
                _logger.Warning(logEntry.AsXML());
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
}