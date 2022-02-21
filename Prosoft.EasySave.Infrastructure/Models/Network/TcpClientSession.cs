using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using NetCoreServer;
using ProSoft.EasySave.Infrastructure.Interfaces.Network.Dispatcher;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages.Server;

namespace ProSoft.EasySave.Infrastructure.Models.Network
{
    public class TcpClientSession : TcpSession
    {
        private readonly IPacketReceiver _packetReceiver;
        private readonly Server _tcpServer;

        public TcpClientSession(Server tcpServer, IPacketReceiver packetReceiver, IJobFactoryService jobFactoryService)
            : base(tcpServer)
        {
            _tcpServer = tcpServer;
            _packetReceiver = packetReceiver;
            JobFactoryService = jobFactoryService;
        }

        public IJobFactoryService JobFactoryService { get; set; }

        public bool SendMessageAsync(ISendable packet)
        {
            var message = new Message
            {
                MessageType = packet.GetType().ToString(),
                Content = packet
            };
            return base.SendAsync(JsonSerializer.Serialize(message) + "\x0A\x0D");
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