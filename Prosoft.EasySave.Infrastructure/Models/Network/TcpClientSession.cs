using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NetCoreServer;
using ProSoft.EasySave.Infrastructure.Interfaces.Network.Dispatcher;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.Infrastructure.Models.Network.Dispatcher;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages.Server;

namespace ProSoft.EasySave.Infrastructure.Models.Network
{
    public class TcpClientSession : TcpSession
    {
        private readonly Server _tcpServer;
        private readonly IPacketReceiver _packetReceiver;
        public IJobFactoryService JobFactoryService { get; set; }

        public TcpClientSession(Server tcpServer, IPacketReceiver packetReceiver, IJobFactoryService jobFactoryService) : base(tcpServer)
        {
            _tcpServer = tcpServer;
            _packetReceiver = packetReceiver;
            JobFactoryService = jobFactoryService;
        }

        public bool SendMessageAsync(ISendable packet)
        {
            var message = new Message()
            {
                MessageType = packet.GetType().ToString(),
                Content = packet
            };
            return base.SendAsync(JsonSerializer.Serialize(message));
        }

        protected override void OnConnected()
        {
            _tcpServer.AddClient(this);
            Console.WriteLine($"TCP session with Id [{Id}|{Socket.RemoteEndPoint}]  connected!");
            SendMessageAsync(new ServerHello("3.0.0"));
        }

        protected override void OnDisconnected()
        {
            _tcpServer.RemoveClient(this);
            Console.WriteLine($"TCP session with Id {Id} disconnected!");
        }

        protected override async void OnReceived(byte[] buffer, long offset, long size)
        {
            var message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            var deserializedMessage = JsonSerializer.Deserialize<Message>(message);
            await _packetReceiver.ReceiveAsync(this, deserializedMessage);
        }

        protected override void OnError(SocketError error)
        {
            _tcpServer.RemoveClient(this);
            Console.WriteLine($"TCP session caught an error with code {error}");
        }
    }
}