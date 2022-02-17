using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProSoft.EasySave.Console.Interfaces;
using ProSoft.EasySave.Console.Services;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.Infrastructure.Models;
using ProSoft.EasySave.Infrastructure.Services;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using Serilog.Events;

namespace ProSoft.EasySave.Console
{
    public static class Program
    {
        public static async Task Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("AppSettings.json", true, true);

            var configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Logger(
                    // Hack to allow us to write to two different files and choose which one.
                    x => x.Filter.ByIncludingOnly(y => y.Level is LogEventLevel.Information)
                        .WriteTo.File(
                           @"./logs/logs.json",
                           outputTemplate:
                           "{Message:lj}{NewLine}",
                           rollingInterval: RollingInterval.Day,
                           shared: true))
                .WriteTo.Logger(
                    // Hack to allow us to write to two different files and choose which one.
                    x => x.Filter.ByIncludingOnly(y => y.Level is LogEventLevel.Warning)
                        .WriteTo.File(
                            @"./logs/logs.xml",
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
}