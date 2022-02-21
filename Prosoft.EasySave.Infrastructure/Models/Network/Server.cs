using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NetCoreServer;
using ProSoft.EasySave.Infrastructure.Interfaces;
using ProSoft.EasySave.Infrastructure.Interfaces.Network.Dispatcher;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages.Server;

namespace ProSoft.EasySave.Infrastructure.Models.Network
{
    public class Server : TcpServer
    {
        private readonly IPacketReceiver _packetReceiver;
        private readonly IJobFactoryService _jobFactoryService;
        private readonly ConcurrentDictionary<string, TcpClientSession> _clients = new();

        public Server(IPacketReceiver packetReceiver, IJobFactoryService jobFactoryService) : base("127.0.0.1", 666)
        {
            _packetReceiver = packetReceiver;
            _jobFactoryService = jobFactoryService;
            _jobFactoryService.OnJobListUpdated += (s, e) => Multicast(new InitializeState(e.JobContexts));
            _jobFactoryService.OnJobStarted += (s, e) => Multicast(new JobStarted(e.JobContext));
            _jobFactoryService.OnJobCompleted += (s,e) =>  Multicast(new JobCompleted(e.JobContext));
            //_jobFactoryService.OnJobPaused += ;
            //_jobFactoryService.OnJobResumed += ;
        }

        public bool Multicast(ISendable packet)
        {
            var message = new Message()
            {
                MessageType = packet.GetType().ToString(),
                Content = packet
            };
            return base.Multicast(JsonSerializer.Serialize(message));
        }

        public void AddClient(TcpClientSession tcpClientSession)
        {
            _clients.AddOrUpdate(tcpClientSession.Id.ToString(), tcpClientSession, (key, oldValue) => tcpClientSession);
        }

        public void RemoveClient(TcpClientSession tcpClientSession)
        {
            _clients.Remove(tcpClientSession.Id.ToString(), out _);
        }

        protected override void OnStarting()
        {
            base.OnStarting();
            Console.WriteLine("TCP server starting..");
        }

        protected override void OnStarted()
        {
            base.OnStarted();
            Console.WriteLine("TCP server started.");
        }

        protected override TcpSession CreateSession()
            => new TcpClientSession(this, _packetReceiver, _jobFactoryService);
        
        protected override void OnError(SocketError error) => Console.WriteLine($"TCP server caught an error with code {error}");
    }
}