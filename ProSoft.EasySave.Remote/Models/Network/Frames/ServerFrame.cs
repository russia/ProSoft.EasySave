using System;
using System.Collections.ObjectModel;
using ProSoft.EasySave.Infrastructure.Models.Contexts;
using ProSoft.EasySave.Infrastructure.Models.Network.Dispatcher;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages.Client;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages.Server;

namespace ProSoft.EasySave.Remote.Models.Network.Frames
{
    // Ce que le serveur envoie
    public class ServerFrame
    {
        [PacketType(typeof(ServerHello))]
        public static void ServerHello(Client client, ServerHello packet)
        {
            client.SendPacketAsync(new ClientHandshake(Environment.MachineName));
        }


        [PacketType(typeof(InitializeState))]
        public static void InitializeState(Client client, InitializeState packet)
        {
            client.EasySaveContext.JobContexts = packet.JobContexts;
        }


        [PacketType(typeof(JobStarted))]
        public static void JobStarted(Client client, JobStarted packet)
        {
        }

        [PacketType(typeof(JobCompleted))]
        public static void JobCompleted(Client client, JobCompleted packet)
        {
        }
    }
}