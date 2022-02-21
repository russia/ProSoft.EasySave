using Prism.Commands;
using Prism.Mvvm;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages.Client;
using ProSoft.EasySave.Remote.Models;

namespace ProSoft.EasySave.Remote.ViewModels
{
    public class _ControllerPartialViewModel : BindableBase
    {
        public _ControllerPartialViewModel(EasySaveContext easySaveContext)
        {
            EasySaveContext = easySaveContext;
            StartJobContextCommand =
                new DelegateCommand<JobContext>(o => EasySaveContext.Client.SendPacketAsync(new StartSave(o)));
            DeleteJobContextCommand =
                new DelegateCommand<JobContext>(o => EasySaveContext.Client.SendPacketAsync(new DeleteSave(o)));
            StartAllCommand =
              new DelegateCommand<JobContext>(o => EasySaveContext.Client.SendPacketAsync(new StartAll()));
            PauseAllCommand =
              new DelegateCommand<JobContext>(o => EasySaveContext.Client.SendPacketAsync(new PauseAll()));

        }

        public EasySaveContext EasySaveContext { get; set; }

        public DelegateCommand<JobContext> StartJobContextCommand { get; set; }
        public DelegateCommand<JobContext> DeleteJobContextCommand { get; set; }
        public DelegateCommand<JobContext> StartAllCommand { get; set; }
        public DelegateCommand<JobContext> PauseAllCommand { get; set; }
    }
}