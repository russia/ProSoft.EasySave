using System;
using System.Diagnostics;
using System.IO;

namespace ProSoft.EasySave.Infrastructure.Helpers
{
    public static class ProcessHelpers
    {
        /// <summary>
        ///     Starts the specified process with arguments and returns the time the process took to run.
        /// </summary>
        /// <param name="processPath">The process path.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The process lifespan.</returns>
        /// <exception cref="Exception">Couldn't start the specified process.</exception>
        /// <exception cref="FileNotFoundException">Couldn't start the specified process.</exception>
        public static TimeSpan UseProcess(string processPath, string args)
        {
            var processStartInfo = new ProcessStartInfo(processPath)
            {
                UseShellExecute = false,
                Arguments = args
            };

            if(!File.Exists(processPath))
                throw new FileNotFoundException("Couldn't start the specified process.", processPath);

            var process = Process.Start(processStartInfo);

            if (process is null)
                throw new Exception("Couldn't start the specified process."); // TODO : create our own exception.

            process.WaitForExit();

            return DateTime.Now - process.StartTime;
        }
    }
}