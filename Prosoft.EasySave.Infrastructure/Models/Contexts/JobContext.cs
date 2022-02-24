using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Models.Network.Events;
using System;

namespace ProSoft.EasySave.Infrastructure.Models.Contexts
{
    public class JobContext
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }

        private TransferType _transferType;
        public TransferType TransferType
        {
            get => _transferType;
            set
            {
                _transferType = value;
                OnJobContextPropertyUpdated?.Invoke(this, new JobContextPropertyUpdatedEventArgs(this));
            }
        }

        private StateType _stateType = StateType.WAITING;
        public StateType StateType
        {
            get => _stateType;
            set
            {
                _stateType = value;
                OnJobContextPropertyUpdated?.Invoke(this, new JobContextPropertyUpdatedEventArgs(this));
            }
        }

        public bool IsCompleted => StateType.Equals(StateType.COMPLETED);

        private bool _pauseRaised;
        public bool PauseRaised
        {
            get => _pauseRaised;
            set
            {
                _pauseRaised = value;
                OnJobContextPropertyUpdated?.Invoke(this, new JobContextPropertyUpdatedEventArgs(this));
            }
        }

        private bool _cancellationRaised;
        public bool CancellationRaised
        {
            get => _cancellationRaised;
            set
            {
                _cancellationRaised = value;
                OnJobContextPropertyUpdated?.Invoke(this, new JobContextPropertyUpdatedEventArgs(this));
            }
        }
        private uint _progression;
        public uint Progression { get => _progression; 
            set
            {
                _progression = value;
                OnJobContextPropertyUpdated?.Invoke(this, new JobContextPropertyUpdatedEventArgs(this));
            }
        }

        public delegate void JobContextPropertyUpdated(object sender, JobContextPropertyUpdatedEventArgs e);
        public event JobContextPropertyUpdated OnJobContextPropertyUpdated;

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