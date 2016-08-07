using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrfSharp.Interfaces
{
    public interface IProcessLauncher
    {
        string LaunchProcessAndCaptureSTDOUT(string fileName, string arguments);
        void LaunchProcess(string fileName, string arguments, bool useShell); 
    }
}
