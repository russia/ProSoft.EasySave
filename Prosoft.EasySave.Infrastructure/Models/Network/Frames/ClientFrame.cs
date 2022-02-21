using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCoreServer;
using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using ProSoft.EasySave.Infrastructure.Models.Network.Dispatcher;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages.Client;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages.Server;
using ProSoft.EasySave.Infrastructure.Services;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Frames
{
    // Ce que le client envoie
    public class ClientFrame
    {
        [PacketId(typeof(ClientHandshake))]
        public static void ClientHandshake(TcpClientSession tcpClientSession, ClientHandshake packet)
            => tcpClientSession.SendMessageAsync(new InitializeState(tcpClientSession.JobFactoryService.GetJobs()));

        [PacketId(typeof(StartSave))]
        public static void StartSave(TcpClientSession tcpClientSession, StartSave packet)
            => tcpClientSession.JobFactoryService.StartJobAsync(packet.JobContext, ExecutionType.SINGLE);
        

        [PacketId(typeof(DeleteSave))]
        public static void DeleteSave(TcpClientSession tcpClientSession, DeleteSave packet)
            => tcpClientSession.JobFactoryService.RemoveJob(packet.JobContext);

        [PacketId(typeof(PauseAll))]
        public static void DeleteSave(TcpClientSession tcpClientSession, PauseAll packet)
        {
        }

        [PacketId(typeof(StartAll))]
        public static void StartAll(TcpClientSession tcpClientSession, StartAll packet)
            => tcpClientSession.JobFactoryService.StartAllJobsAsync(ExecutionType.CONCURRENT);

    }
}