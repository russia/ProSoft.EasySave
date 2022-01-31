using ProSoft.EasySave.Application.Enums;
using ProSoft.EasySave.Application.Models.Contexts;

namespace ProSoft.EasySave.Application.Models;

/// <summary>
/// Class representing the save works configuration's.
/// </summary>
public class Configuration
{
    public List<JobContext> JobContexts { get; set; }
    public ExecutionType ExecutionType { get; set; }
}