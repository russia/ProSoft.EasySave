using Microsoft.Extensions.Logging;
using ProSoft.EasySave.Application.Interfaces.Services;

namespace ProSoft.EasySave.Application.Services;

public class SampleService : ISampleService
{
    private readonly ILogger<SampleService> _logger;
    private readonly IFileService _fooService;

    public SampleService(ILogger<SampleService> logger, IFileService fooService)
    {
        _logger = logger;
        _fooService = fooService;
    }

    public void TestMethod()
    {
        _logger.LogInformation("Test method :)");
    }
}