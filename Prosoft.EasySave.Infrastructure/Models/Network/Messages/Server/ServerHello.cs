using System;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Messages.Server
{
    public record ServerHello : ISendable
    {
        public ServerHello(string version)
        {
            Version = version;
        }

        public TimeSpan PingDelay => TimeSpan.FromSeconds(5);
        public string Version { get; set; }
    }
}