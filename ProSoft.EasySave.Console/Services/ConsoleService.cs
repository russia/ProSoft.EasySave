using Microsoft.Extensions.Logging;
using ProSoft.EasySave.Application.Enums;
using ProSoft.EasySave.Application.Interfaces.Services;
using ProSoft.EasySave.Application.Models.Contexts;
using ProSoft.EasySave.Console.Interfaces;
using ProSoft.EasySave.Application.Models;

namespace ProSoft.EasySave.Console.Managers;

public class ConsoleService : IConsoleService
{
    private readonly IJobFactoryService _factoryService;
    private readonly ILogger<ConsoleService> _logger;

    public ConsoleService(ILogger<ConsoleService> logger,
        IJobFactoryService factoryService)
    {
        _logger = logger;
        _factoryService = factoryService;
        _factoryService.LoadConfiguration();
    }

    public void AddJob(string name, TransferType transferType, string sourcePath, string destinationPath)
    {
        _factoryService.AddJob(name, transferType, sourcePath, destinationPath);
    }

    public async Task<IReadOnlyCollection<JobResult>> Start(ExecutionType executionType)
    {
        return await _factoryService.StartJobsAsync(2, executionType);
    }
}