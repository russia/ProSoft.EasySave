using ProSoft.EasySave.Infrastructure.Enums;
using ProSoft.EasySave.Infrastructure.Models.Network.Dispatcher;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages.Client;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages.Server;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Frames
{
    // Ce que le client envoie
    public class ClientFrame
    {
        [PacketType(typeof(ClientHandshake))]
        public static void ClientHandshake(TcpClientSession tcpClientSession, ClientHandshake packet)
        {
            tcpClientSession.SendMessageAsync(new InitializeState(tcpClientSession.JobFactoryService.GetJobs()));
        }

        [PacketType(typeof(StartSave))]
        public static void StartSave(TcpClientSession tcpClientSession, StartSave packet)
        {
            tcpClientSession.JobFactoryService.StartJobAsync(packet.JobContext, ExecutionType.SINGLE);
        }

        [PacketType(typeof(DeleteSave))]
        public static void DeleteSave(TcpClientSession tcpClientSession, DeleteSave packet)
        {
            tcpClientSession.JobFactoryService.RemoveJob(packet.JobContext);
        }

        [PacketType(typeof(StartAll))]
        public static void StartAll(TcpClientSession tcpClientSession, StartAll packet)
        {
            tcpClientSession.JobFactoryService.StartAllJobsAsync(ExecutionType.CONCURRENT);
        }

        [PacketType(typeof(PauseAll))]
        public static void PauseAll(TcpClientSession tcpClientSession, PauseAll packet)
        {
            tcpClientSession.JobFactoryService.PauseAllJobsAsync();
        }

        [PacketType(typeof(ResumeAll))]
        public static void ResumeAll(TcpClientSession tcpClientSession, ResumeAll packet)
        {
            tcpClientSession.JobFactoryService.ResumeAllJobsAsync();
        }

        [PacketType(typeof(CancelAll))]
        public static void CancelAll(TcpClientSession tcpClientSession, CancelAll packet)
        {
            tcpClientSession.JobFactoryService.CancelAllJobsAsync();
        }
    }
}