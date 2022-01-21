using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProSoft.EasySave.Application.Interfaces.Services;
using ProSoft.EasySave.Application.Services;
using System;

namespace ProSoft.EasySave.Console;

public class Program
{
    public static async Task Main(string[] args)
    {
        var serviceCollection = new ServiceCollection()
            .AddLogging(cfg => cfg.AddConsole())
            .AddSingleton<ISampleService, SampleService>()
            .AddSingleton<IFooService, FooService>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var sampleService = serviceProvider.GetService<ISampleService>();
        await Startup(args, sampleService).ConfigureAwait(false);
    }

    public static async Task Startup(string[] args, ISampleService sampleService) // StartUp ?
    {
        await sampleService.TestMethod();
        System.Console.Read();
    }
}