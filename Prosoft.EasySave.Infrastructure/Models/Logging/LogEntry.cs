using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ProSoft.EasySave.Application.Models.Logging
{
    public class LogEntry
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public long FileSize { get; set; }
        public float FileTransferTime { get; set; }
        public DateTime Time { get; set; }

        public string AsXML()
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(stringwriter, this);
                return stringwriter.ToString();
            }
        }

        public string AsJson()
        {
            return JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }
    }
}