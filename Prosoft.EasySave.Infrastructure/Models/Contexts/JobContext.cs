using ProSoft.EasySave.Infrastructure.Enums;

namespace ProSoft.EasySave.Infrastructure.Models.Contexts
{

    public class JobContext
    {
        public string Name { get; set; } 
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public TransferType TransferType { get; set; }
        public StateType StateType { get; set; } = StateType.WAITING;
        public bool IsCompleted => StateType.Equals(StateType.COMPLETED);
    }
}