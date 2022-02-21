using System;

namespace ProSoft.EasySave.Infrastructure.Models.Contexts
{

    public class JobResult 
    {
        public JobResult()
        {

        }

        public JobResult(bool success, string error)
        {
            Success = success;
            Error = error;
        }

        public bool Success { get; set; } = true;
        public string Error { get; set; } = null;

        public string Name { get; set; }
        public int FilesNumber { get; set; }
        public ulong TotalFilesWeight { get; set; } // in bytes
        public TimeSpan Duration { get; set; }
    }
}