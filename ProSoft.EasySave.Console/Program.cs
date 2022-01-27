using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProSoft.EasySave.Application.Interfaces.Services;
using ProSoft.EasySave.Application.Models;
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

        var consoleService = serviceProvider.GetService<IConsoleService>();
        consoleService.DisplayConsoleInterface();

        System.Console.ReadKey();
    }
}