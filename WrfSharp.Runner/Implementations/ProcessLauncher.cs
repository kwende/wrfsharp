using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace WrfSharp.Runner.Implementations
{
    public class ProcessLauncher : IProcessLauncher
    {
        public string LaunchProcessAndCaptureSTDOUT(string fileName, string arguments)
        {
            ProcessStartInfo procStart = new ProcessStartInfo();
            procStart.FileName = fileName;
            procStart.Arguments = arguments;
            procStart.UseShellExecute = false;
            procStart.RedirectStandardOutput = true;

            using (Process proc = Process.Start(procStart))
            {
                using (StreamReader sr = proc.StandardOutput)
                {
                    string output = sr.ReadToEnd();
                    return output;
                }
            }
        }

        public void LaunchProcess(string fileName, string arguments, bool useShell)
        {
            ProcessStartInfo procStart = new ProcessStartInfo();
            procStart.FileName = fileName;
            procStart.Arguments = arguments;
            procStart.UseShellExecute = useShell;

            Process.Start(procStart).WaitForExit(); 
        }
    }
}
