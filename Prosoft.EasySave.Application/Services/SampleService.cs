using Microsoft.Extensions.Logging;
using ProSoft.EasySave.Application.Interfaces.Services;

namespace ProSoft.EasySave.Application.Services;

public class SampleService : ISampleService
{
    private readonly ILogger<SampleService> _logger;
    private readonly IFooService _fooService;

    public SampleService(ILogger<SampleService> logger, IFooService fooService)
    {
        _logger = logger;
        _fooService = fooService;
    }

    public Task TestMethod()
    {
        _logger.LogInformation("Test method :)");
        return Task.CompletedTask;
    }
}