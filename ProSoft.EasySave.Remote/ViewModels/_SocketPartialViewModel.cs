using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ProSoft.EasySave.Remote.Models.Network;

namespace ProSoft.EasySave.Remote.ViewModels
{
    public class _SocketPartialViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private ObservableCollection<NetworkLog> _networkLog;

        public _SocketPartialViewModel(IRegionManager regionManager, Client client)
        {
            _regionManager = regionManager;
            NetworkLogs = new ObservableCollection<NetworkLog>();
            ConnectCommand = new DelegateCommand(() => client.ConnectAsync());
            DisconnectCommand = new DelegateCommand(() => client.DisconnectAsync());

            client.OnPacketReceived += (s, e) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    NetworkLogs.Insert(0, new NetworkLog
                    {
                        Way = "In",
                        PacketType = e.Message.GetType(),
                        Content = e.Message.Content.ToString()
                    });
                });
            };

            client.OnPacketSent += (s, e) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    var test = e.Message.GetContent();
                    NetworkLogs.Insert(0, new NetworkLog
                    {
                        Way = "Out",
                        PacketType = e.Message.GetType(),
                        Content = test
                    });
                });
            };
        }

        public DelegateCommand ConnectCommand { get; }
        public DelegateCommand DisconnectCommand { get; }

        public ObservableCollection<NetworkLog> NetworkLogs
        {
            get => _networkLog;
            set => SetProperty(ref _networkLog, value);
        }
    }
}