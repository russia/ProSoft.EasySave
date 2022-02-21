using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSoft.EasySave.Infrastructure.Models.Network.Messages
{
    public record Message
    {
        public string MessageType { get; set; }

        public object Content { get; set; }

        public string GetContent()
        {
            var type = Type.GetType(MessageType);
            return Content.ToString();
            //dynamic castedObject = JsonConvert.DeserializeObject(content, type);
            //return castedObject.ToString();
        }

        public string GetType()
        {
            return MessageType.Substring(MessageType.LastIndexOf("."),
                MessageType.Length - MessageType.LastIndexOf("."))
                .Replace(".", "");
        }
    }
}