using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using ProSoft.EasySave.Application.Enums;
using ProSoft.EasySave.Application.Interfaces.Services;
using ProSoft.EasySave.Application.Models;
using ProSoft.EasySave.Application.Models.Contexts;
using ProSoft.EasySave.Application.Services;
using ProSoft.EasySave.Console.Interfaces;
using ProSoft.EasySave.Console.Managers;

namespace ProSoft.EasySave.Console;

public static class Program
{
    public static async Task Main()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\Prosoft.EasySave.Application\")))
            .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

        var configuration = builder.Build();

        var serviceProvider = new ServiceCollection()
            .Configure<Configuration>(configuration.GetSection("Configuration"))
            .AddLogging(cfg =>
            {
                cfg.AddDebug();
                cfg.AddConsole();
                cfg.SetMinimumLevel(LogLevel.Debug);
            })
            .AddScoped<IGlobalizationService, GlobalizationService>()
            .AddScoped<IFileService, FileService>()
            .AddSingleton<ISampleService, SampleService>()
            .AddSingleton<IConsoleService, ConsoleService>()
            .AddSingleton<IJobFactoryService, JobFactoryService>()
            .BuildServiceProvider();

        var globalisationService = serviceProvider.GetService<IGlobalizationService>();

        System.Console.WriteLine(globalisationService.GetString("WELCOME_MESSAGE"));

        var consoleService = serviceProvider.GetService<IConsoleService>();
        var result = await consoleService.Start(ExecutionType.CONCURRENT);

        var loggingService = serviceProvider.GetService<ILogger<JobContext>>();
        loggingService.LogInformation($"Work done, took {result.Sum(r => r.Duration.TotalSeconds)} s to move {result.Sum(r => r.FilesNumber)} files, " +
            $"with a total size of {result.Sum(r => (long)r.TotalFilesWeight)} bytes.");

        System.Console.ReadKey();
    }
}