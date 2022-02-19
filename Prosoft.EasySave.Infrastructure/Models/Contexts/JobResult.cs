using System;

namespace ProSoft.EasySave.Infrastructure.Models.Contexts
{

    public class JobResult
    {
        public string Name { get; set; }
        public int FilesNumber { get; set; }
        public ulong TotalFilesWeight { get; set; } // in bytes
        public TimeSpan Duration { get; set; }
       
    }


}