using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace HelperTests.Mocks
{
    public class MockProcessLauncherForWgrib2 : IProcessLauncher
    {
        public void LaunchProcess(string fileName, string arguments, bool useShell)
        {
            throw new NotImplementedException();
        }

        public string LaunchProcessAndCaptureSTDOUT(string fileName, string arguments)
        {
            if(arguments.EndsWith("f000"))
            {
                return File.ReadAllText("TestFiles/WgribStartOutput.txt");
            }
            else
            {
                return File.ReadAllText("TestFiles/WgribEndOutput.txt");
            }
        }
    }
}
