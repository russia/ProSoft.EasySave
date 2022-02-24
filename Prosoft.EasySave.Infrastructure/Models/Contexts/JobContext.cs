using ProSoft.EasySave.Infrastructure.Enums;
using System;

namespace ProSoft.EasySave.Infrastructure.Models.Contexts
{
    public class JobContext
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public string Name { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public TransferType TransferType { get; set; }
        public StateType StateType { get; set; } = StateType.WAITING;
        public bool IsCompleted => StateType.Equals(StateType.COMPLETED);
        public bool PauseRaised { get; set; }

        public bool CancellationRaised { get; set; }
        // As cancellation tokens (IntPtr) can't be serialized we need to use a native type.
        //public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();

        public override bool Equals(object obj)
        {
            if (obj is not JobContext)
                return false;

            return (obj as JobContext).Id == Id;
        }
    }
}