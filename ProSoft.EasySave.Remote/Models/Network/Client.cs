using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ProSoft.EasySave.Infrastructure.Interfaces.Network.Dispatcher;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages;
using ProSoft.EasySave.Remote.Events;
using TcpClient = NetCoreServer.TcpClient;

namespace ProSoft.EasySave.Remote.Models.Network
{
    public class Client : TcpClient
    {
        public delegate void PacketReceived(object sender, MessageReceivedEventArgs e);

        public delegate void PacketSent(object sender, MessageSentEventArgs e);

        private readonly IPacketReceiver _packetReceiver;
        public EasySaveContext EasySaveContext;

        public Client(IPacketReceiver packetReceiver, EasySaveContext easySaveContext) : base("127.0.0.1", 666)
        {
            _packetReceiver = packetReceiver;
            EasySaveContext = easySaveContext;
        }

        public event PacketReceived OnPacketReceived;

        public event PacketSent OnPacketSent;

        public void DisconnectAndStop()
        {
            DisconnectAsync();
        }

        protected override void OnConnected()
        {
            Console.WriteLine("Starting ..");
        }

        public void SendPacketAsync(ISendable packet)
        {
            var message = new Message
            {
                MessageType = packet.GetType().ToString(),
                Content = packet
            };

            var messageSerialized = JsonSerializer.Serialize(message);

            SendAsync(messageSerialized);
            OnPacketSent?.Invoke(this, new MessageSentEventArgs(message));
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine("Can't reach server, please try later..");
        }

        protected override async void OnReceived(byte[] buffer, long offset, long size)
        {
            var raw = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);

            foreach (var packet in raw.Replace("\x0A", string.Empty)
                         .Split('\x0D')
                         .Where(x => x != ""))
            {
                var deserializedMessage = JsonSerializer.Deserialize<Message>(packet);
                OnPacketReceived?.Invoke(this, new MessageReceivedEventArgs(deserializedMessage));
                await _packetReceiver.ReceiveAsync(this, deserializedMessage);
            }
        }

        protected override void OnError(SocketError error)
        {
        }
    }
}