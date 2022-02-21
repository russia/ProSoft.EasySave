using System;

namespace ProSoft.EasySave.Remote.Models.Network
{
    public class NetworkLog
    {
        public string Date => DateTime.Now.ToString("T");
        public string Way { get; set; }
        public string From { get; set; }

        public string PacketType { get; set; }
        public string Content { get; set; }
    }
}