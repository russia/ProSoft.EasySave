using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace ProSoft.EasySave.Infrastructure.Helpers
{
    public static class ProcessHelpers
    {
        /// <summary>
        /// Starts the specified process with arguments and returns the time the process took to run.
        /// </summary>
        /// <param name="processPath">The process path.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The process lifespan.</returns>
        /// <exception cref="Exception">Couldn't start the specified process.</exception>
        public static TimeSpan UseProcess(string processPath, string args)
        {
            var processStartInfo = new ProcessStartInfo(processPath)
            {
                UseShellExecute = false,
                Arguments = args
            };

            var test = File.Exists(processPath);

            var process = Process.Start(processStartInfo);

            if (process is null)
                throw new Exception("Couldn't start the specified process."); // TODO : create our own exception.

            process.WaitForExit();

            return DateTime.Now - process.StartTime;
        }
    }
}