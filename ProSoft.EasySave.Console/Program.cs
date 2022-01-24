using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProSoft.EasySave.Application.Interfaces.Services;
using ProSoft.EasySave.Application.Services;
using ProSoft.EasySave.Console.Interfaces;
using ProSoft.EasySave.Console.Managers;

namespace ProSoft.EasySave.Console;

public class Program
{
    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(cfg => cfg.AddConsole())
            .AddSingleton<ISampleService, SampleService>()
            .AddSingleton<IGlobalizationService, GlobalizationService>()
            .AddSingleton<IConsoleService, ConsoleService>()
            .AddSingleton<IFooService, FooService>()
            .BuildServiceProvider();

        var globalisationService = serviceProvider.GetService<IGlobalizationService>();

        System.Console.WriteLine(globalisationService.GetString("WELCOME_MESSAGE"));
        // TODO : read user inputs.

        var consoleService = serviceProvider.GetService<IConsoleService>();

        var result = await consoleService.StartJobAsync(null);
    }
}