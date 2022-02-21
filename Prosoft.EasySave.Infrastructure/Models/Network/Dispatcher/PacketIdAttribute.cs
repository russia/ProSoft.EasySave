using System;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Dispatcher
{
    public class PacketType : Attribute
    {
        public Type Value;

        public PacketType(Type v)
        {
            Value = v;
        }
    }
}