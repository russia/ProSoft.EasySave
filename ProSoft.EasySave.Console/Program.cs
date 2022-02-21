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
using System.Reflection;
using System.Threading.Tasks;
using ProSoft.EasySave.Infrastructure.Models.Network.Dispatcher;
using ProSoft.EasySave.Infrastructure.Models.Network.Frames;
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

            var assembly = Assembly.GetAssembly(typeof(ClientFrame));

            if (assembly is null)
            {
                throw new ArgumentNullException("Assembly cannot be null.");
            }

            var packetReceiver = new PacketReceiver(assembly);
            var remoteService = new RemoteService(packetReceiver);

            var serviceProvider = new ServiceCollection()
                .Configure<Configuration>(configuration.GetSection("Configuration"))
                .AddScoped<IGlobalizationService, GlobalizationService>()
                .AddScoped<IFileService, FileService>()
                .AddSingleton<IConsoleService, ConsoleService>()
                .AddSingleton<IJobFactoryService, JobFactoryService>()
                .AddSingleton(Log.Logger)
                .AddSingleton(packetReceiver)
                .AddSingleton(remoteService)
                .BuildServiceProvider();

            var jobFactoryService = serviceProvider.GetService<IJobFactoryService>();
            remoteService.SetJobFactoryService(jobFactoryService); // TODO : find a wordaround for this..

            var consoleService = serviceProvider.GetService<IConsoleService>();
            await consoleService.DisplayConsoleInterface();

            for (; ; ) await Task.Delay(50);
        }
    }
}