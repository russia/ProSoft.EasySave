using System;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages;

namespace ProSoft.EasySave.Remote.Events
{
    public class MessageSentEventArgs : EventArgs
    {
        public MessageSentEventArgs(Message message)
        {
            Message = message;
        }

        public Message Message { get; set; }
    }
}