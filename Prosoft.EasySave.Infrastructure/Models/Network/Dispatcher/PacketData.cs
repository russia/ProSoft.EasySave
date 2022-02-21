using System;
using System.Reflection;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Dispatcher
{
    public class PacketData
    {
        public object Instance;
        public Type Key;
        public MethodInfo Method;

        public PacketData(object instance, Type key, MethodInfo method)
        {
            Instance = instance;
            Key = key;
            Method = method;
        }
    }
}