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
        public float EncryptionTime { get; set; }
        public DateTime Time { get; set; }
        public string Details
        {
            get
            {
                return String.Format("Crypt time : {0}ms \nFile transfert Time : {1}ms \nDate : {2}", this.FileTransferTime, this.FileTransferTime, this.Time.ToString());
            }
        }

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