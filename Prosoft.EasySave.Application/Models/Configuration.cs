using ProSoft.EasySave.Application.Enums;
using ProSoft.EasySave.Application.Models.Contexts;

namespace ProSoft.EasySave.Application.Models;

public class Configuration
{
    public List<JobContext> JobContexts { get; set; }
    public ExecutionType ExecutionType { get; set; }
}