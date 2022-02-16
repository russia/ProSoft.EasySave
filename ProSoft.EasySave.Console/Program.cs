using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProSoft.EasySave.Application.Interfaces.Services;
using ProSoft.EasySave.Application.Models;
using ProSoft.EasySave.Application.Services;
using ProSoft.EasySave.Console.Interfaces;
using ProSoft.EasySave.Console.Services;
using Serilog;
using Serilog.Events;

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
            .WriteTo.Logger(
                // Hack to allow us to write to two different files and choose which one.
                x => x.Filter.ByIncludingOnly(y => y.Level is LogEventLevel.Information)
                    .WriteTo.File(
                       @"logs.json",
                       outputTemplate:
                       "{Message:lj}{NewLine}",
                       rollingInterval: RollingInterval.Day,
                       shared: true))
            .WriteTo.Logger(
                // Hack to allow us to write to two different files and choose which one.
                x => x.Filter.ByIncludingOnly(y => y.Level is LogEventLevel.Warning)
                    .WriteTo.File(
                        @"logs.xml",
                        outputTemplate:
                        "{Message:lj}{NewLine}",
                        rollingInterval: RollingInterval.Day,
                        shared: true))
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
        await consoleService.DisplayConsoleInterface();

        System.Console.ReadKey();
    }
}