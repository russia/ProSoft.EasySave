using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProSoft.EasySave.Infrastructure.Interfaces.Network.Dispatcher;
using ProSoft.EasySave.Infrastructure.Models.Network.Messages;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Dispatcher
{
    public class PacketReceiver : IPacketReceiver
    {
        private readonly List<PacketData> _methods;

        public PacketReceiver(Assembly assembly)
        {
            _methods = Initialize(assembly);
        }

        public async Task ReceiveAsync(object receiver, Message message)
        {
            try
            {
                var packetType = Type.GetType(message.MessageType);
                var method = _methods.FirstOrDefault(m => m.Key == packetType);

                if (method is null)
                    return;

                // hack to convert an object to a type specified as a variable : https://stackoverflow.com/a/56604759
                dynamic castedObject = JsonConvert.DeserializeObject(message.Content.ToString(), packetType);

                //dynamic castedObject = Activator.CreateInstance(packetType, message.Content);

                var task = (Task) method.Method.Invoke(method.Instance, new[] {receiver, castedObject});
                if (task is not null)
                {
                    await task;
                    if (task.IsFaulted)
                        Console.WriteLine(task.Exception + $" Caught exception while handling message {packetType} ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private List<PacketData> Initialize(Assembly assembly)
        {
            var methods = new List<PacketData>();
            var methodInfos = assembly.GetTypes().SelectMany(x => x.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(PacketType), false).Length > 0);
            foreach (var methodInfo in methodInfos)
            {
                var attr = (PacketType) methodInfo.GetCustomAttributes(typeof(PacketType), true)[0];

                var instance = Activator.CreateInstance(methodInfo.DeclaringType);

                methods.Add(new PacketData(instance, attr.Value, methodInfo));
            }

            Console.WriteLine($"{methods.Count} packets registered!");
            return methods;
        }
    }
}