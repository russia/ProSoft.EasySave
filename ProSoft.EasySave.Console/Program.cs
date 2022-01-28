using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProSoft.EasySave.Application.Interfaces.Services;
using ProSoft.EasySave.Application.Models;
using ProSoft.EasySave.Application.Services;
using ProSoft.EasySave.Console.Interfaces;
using ProSoft.EasySave.Console.Managers;
using Serilog;

namespace ProSoft.EasySave.Console;

public static class Program
{
    public static async Task Main()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory,
                @"..\..\..\..\Prosoft.EasySave.Application\")))
            .AddJsonFile("AppSettings.json", true, true);

        var configuration = builder.Build();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                @"logs.json",
                outputTemplate:
                "{Message:lj}{NewLine}",
                rollingInterval: RollingInterval.Day,
                shared: true)
            .CreateLogger();

        var serviceProvider = new ServiceCollection()
            .Configure<Configuration>(configuration.GetSection("Configuration"))
            .AddScoped<IGlobalizationService, GlobalizationService>()
            .AddScoped<IFileService, FileService>()
            .AddSingleton<IConsoleService, ConsoleService>()
            .AddSingleton<IJobFactoryService, JobFactoryService>()
            .AddSingleton(Log.Logger)
            .BuildServiceProvider();

        var consoleService = serviceProvider.GetService<IConsoleService>();
        consoleService.DisplayConsoleInterface();

        System.Console.ReadKey();
    }
}