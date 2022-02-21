using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Dispatcher
{
    public class PacketIdAttribute : Attribute
    {
        public Type Value;

        public PacketIdAttribute(Type v)
        {
            this.Value = v;
        }
    }
}