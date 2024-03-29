﻿using System.Threading.Tasks;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages;

namespace ProSoft.EasySave.Infrastructure.Interfaces.Network.Dispatcher
{
    public interface IPacketReceiver
    {
        Task ReceiveAsync(object receiver, Message message);
    }
}