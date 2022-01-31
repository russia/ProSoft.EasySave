namespace ProSoft.EasySave.Application.Models.Contexts;

/// <summary>
/// Class allowing to save work's results to be stored.
/// </summary>
public class JobResult
{
    public string Name { get; set; }
    public int FilesNumber { get; set; }
    public ulong TotalFilesWeight { get; set; } // in bytes
    public TimeSpan Duration { get; set; }
}