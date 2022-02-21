using System.Collections.ObjectModel;
using Prism.Mvvm;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using ProSoft.EasySave.Remote.Models.Network;

namespace ProSoft.EasySave.Remote.Models
{
    public class EasySaveContext : BindableBase
    {
        private ObservableCollection<JobContext> _jobContexts;

        public Client Client { get; set; }

        public ObservableCollection<JobContext> JobContexts
        {
            get => _jobContexts;
            set => SetProperty(ref _jobContexts, value);
        }

        public void SetClient(Client client)
        {
            Client = client;
        }
    }
}