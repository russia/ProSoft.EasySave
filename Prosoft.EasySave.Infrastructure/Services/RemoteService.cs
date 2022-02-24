using System.Threading;
using System.Threading.Tasks;
using ProSoft.EasySave.Infrastructure.Interfaces.Network.Dispatcher;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.Infrastructure.Models.Network;

namespace ProSoft.EasySave.Infrastructure.Services
{
    public class RemoteService : IRemoteService
    {
        private readonly IPacketReceiver _packetReceiver;
        private IJobFactoryService _jobFactoryService;
        private Server _server;

        public RemoteService(IPacketReceiver packetReceiver)
        {
            _packetReceiver = packetReceiver;
        }

        // Dirty hack ..
        public void SetJobFactoryService(IJobFactoryService jobFactoryService)
        {
            _jobFactoryService = jobFactoryService;
            _server = new Server(_packetReceiver, _jobFactoryService);
            _server.Start();
        }
    }
}