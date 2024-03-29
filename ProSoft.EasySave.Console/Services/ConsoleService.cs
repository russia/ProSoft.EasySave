﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ProSoft.EasySave.Console.Interfaces;
using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Helpers;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.Infrastructure.Models.Contexts;

namespace ProSoft.EasySave.Console.Services
{
    public class ConsoleService : IConsoleService
    {
        private readonly IJobFactoryService _factoryService;
        private readonly IGlobalizationService _globalizationService;

        public ConsoleService(IJobFactoryService factoryService,
            IGlobalizationService globalizationService)
        {
            _globalizationService = globalizationService;
            _factoryService = factoryService;
            _factoryService.LoadConfiguration();
        }

        public void AddJob(string name, TransferType transferType, string sourcePath, string destinationPath)
        {
            _factoryService.AddJob(name, transferType, sourcePath, destinationPath);
        }

        public async Task<IReadOnlyCollection<JobResult>> Start(ExecutionType? executionType = null)
        {
            var jobResults = await _factoryService.StartAllJobsAsync(executionType);

            jobResults = jobResults
                .Where(j => j.Success)
                .ToList();

            System.Console.WriteLine(
                $"Work done, took {jobResults.Sum(r => r.Duration.TotalSeconds)} s to move {jobResults.Sum(r => r.FilesNumber)} files, " +
                $"with a total size of {BytesHelpers.FormatBytesSize((ulong)jobResults.Sum(r => (long)r.TotalFilesWeight))}.");
            return jobResults;
        }

        public async Task DisplayConsoleInterface()
        {
            System.Console.WriteLine(
                $"ProSoft - EasySave | {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion}");
            System.Console.WriteLine(_globalizationService.GetString("WELCOME_MESSAGE"));
            System.Console.WriteLine(_globalizationService.GetString("SKIP_USER_INPUT"));
            if (!bool.TryParse(System.Console.ReadLine(), out var skip) || skip)
            {
                await Start();
                return;
            }

            System.Console.WriteLine(_globalizationService.GetString("EXECUTION_TYPE"));
            foreach (var executionTypeEnum in
                     Enum.GetValues(typeof(ExecutionType))) // TODO : create a custom method for this?
                System.Console.WriteLine($"{(int)executionTypeEnum} - {executionTypeEnum}");
            if (!int.TryParse(System.Console.ReadLine(), out var result))
                throw new Exception(); //TODO

            var executionType = (ExecutionType)result;

            while (true)
            {
                System.Console.WriteLine(_globalizationService.GetString("BACKUP_NAME"));

                var name = System.Console.ReadLine();

                System.Console.WriteLine(_globalizationService.GetString("TRANSFERT_TYPE"));
                foreach (var transferTypeEnum in
                         Enum.GetValues(typeof(TransferType))) // TODO : create a custom method for this?
                    System.Console.WriteLine($"{(int)transferTypeEnum} - {transferTypeEnum}");

                if (!int.TryParse(System.Console.ReadLine(), out var transferTypeInt))
                    throw new Exception(); //TODO

                var transferType = (TransferType)transferTypeInt;

                System.Console.WriteLine(_globalizationService.GetString("SOURCE"));

                var source = System.Console.ReadLine(); // TODO : check if folder exists?

                System.Console.WriteLine(_globalizationService.GetString("DESTINATION"));

                var destination = System.Console.ReadLine();

                AddJob(name, transferType, source, destination);

                System.Console.WriteLine(_globalizationService.GetString("ADD_NEW_JOB_CONTEXT"));
                if (!bool.TryParse(System.Console.ReadLine(), out var @continue))
                    return;

                if (!@continue)
                    break;
            }

            await Start(executionType);
        }
    }
}