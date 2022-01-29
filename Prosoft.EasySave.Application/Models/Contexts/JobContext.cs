using ProSoft.EasySave.Application.Enums;

namespace ProSoft.EasySave.Application.Models.Contexts;

public class JobContext
{
    public string Name { get; set; } // TODO : make these properties private?
    public string SourcePath { get; set; }
    public string DestinationPath { get; set; }
    public TransferType TransferType { get; set; }
    public StateType StateType { get; set; } = StateType.WAITING;
    public bool IsCompleted => StateType.Equals(StateType.COMPLETED);
}