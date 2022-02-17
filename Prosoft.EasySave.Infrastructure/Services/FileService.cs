﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
                var destinationPath = Path.Combine(jobContext.DestinationPath, sourceFile.ComputeEncryptedName());// TODO : create an extension.
                var stopwatchFile = new Stopwatch();
                stopwatchFile.Start();

                var encryptedFileSourcePath = Path.Combine(sourceFile.Directory.FullName, sourceFile.ComputeEncryptedName());
                var encryptionTime = ProcessHelpers.UseProcess(@"C:\Users\user\source\repos\ProSoft.EasySave\ProSoft.CryptoSoft\bin\Debug\net5.0\ProSoft.CryptoSoft.exe", $"{_configuration.Value.XorKey} {StringsHelpers.Base64Encode(sourceFile.FullName)} {StringsHelpers.Base64Encode(encryptedFileSourcePath)}");
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