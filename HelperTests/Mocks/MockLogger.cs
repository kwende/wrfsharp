using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrfSharp.Interfaces;

namespace HelperTests.Mocks
{
    public class MockLogger : ILogger
    {
        public void Log(string content)
        {
        }

        public void LogLine(string content)
        {
        }
    }
}
