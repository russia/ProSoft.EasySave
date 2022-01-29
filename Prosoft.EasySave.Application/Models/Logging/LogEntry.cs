namespace ProSoft.EasySave.Application.Models.Logging;

internal class LogEntry
{
    public string Name { get; set; }
    public string FileSource { get; set; }
    public string FileTarget { get; set; }
    public long FileSize { get; set; }
    public float FileTransferTime { get; set; }
    public DateTime Time { get; set; }
}