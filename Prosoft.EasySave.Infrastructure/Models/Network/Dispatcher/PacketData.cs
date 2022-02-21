using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Dispatcher
{
    public class PacketData
    {
        public object Instance;
        public Type Key;
        public MethodInfo Method;

        public PacketData(object instance, Type key, MethodInfo method)
        {
            this.Instance = instance;
            this.Key = key;
            this.Method = method;
        }
    }
}